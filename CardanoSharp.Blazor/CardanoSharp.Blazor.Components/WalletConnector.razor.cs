using CardanoSharp.Blazor.Components.Enums;
using CardanoSharp.Blazor.Components.Exceptions;
using CardanoSharp.Blazor.Components.Models;
using CardanoSharp.Blazor.Components.Utils;
using CardanoSharp.Blazor.Components.ViewModels;
using CardanoSharp.Wallet.CIPs.CIP30.Models;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Extensions.Models.Transactions;
using CardanoSharp.Wallet.Models;
using CardanoSharp.Wallet.Models.Addresses;
using CardanoSharp.Wallet.Models.Transactions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PeterO.Cbor2;

namespace CardanoSharp.Blazor.Components;

public partial class WalletConnector
{
    private const string _connectedWalletKey = "connectedWalletKey";

    [Parameter]
    public bool PersistConnectedWallet { get; set; }

    [Parameter]
    public bool OutputDebug { get; set; }

    [Parameter]
    public string Header { get; set; } = "Choose Your Wallet";

    [Parameter]
    public bool AutoCloseOnConnect { get; set; }

    [Parameter]
    public bool AutoCloseOnDisconnect { get; set; }

    [Parameter]
    public ModalClickHideOptions HideOnModalClick { get; set; } = ModalClickHideOptions.Always;

    [Parameter]
    public bool HideCloseButton { get; set; } = false;

    [Parameter]
    public EscapeKeyOptions EscapeKeyPress { get; set; } = EscapeKeyOptions.Always;

    [Parameter]
    public bool DisconnectOnConnectButtonClick { get; set; } = false;

    [Parameter]
    public List<WalletExtension> SupportedExtensions { get; set; } = new List<WalletExtension>()
    {
        new WalletExtension() {
            Key = "eternl",
            Name = "Eternl",
            Icon = WalletIcons.EternlIcon,
            Url = "https://eternl.io"
        },
        new WalletExtension() {
            Key = "gerowallet",
            Name = "Gero",
            Icon = WalletIcons.GeroIcon,
            Url = "https://gerowallet.io"
        },
        new WalletExtension() {
            Key = "nami",
            Name = "Nami",
            Icon = WalletIcons.NamiIcon,
            Url = "https://namiwallet.io"
        },
        new WalletExtension() {
            Key = "typhoncip30",
            Name = "Typhon",
            Icon = WalletIcons.TyphonIcon,
            Url = "https://typhonwallet.io"
        },
        new WalletExtension() {
            Key = "flint",
            Name = "Flint",
            Icon = WalletIcons.FlintIcon,
            Url = "https://flint-wallet.com"
        },
    };

    [Parameter]
    public WalletConnectorLayout Layout { get; set; } = WalletConnectorLayout.Card;

    [Parameter]
    public EventCallback OnConnectStart { get; set; }

    [Parameter]
    public EventCallback OnConnect { get; set; }

    [Parameter]
    public EventCallback<Exception> OnConnectError { get; set; }

    [Parameter]
    public EventCallback OnDisconnect { get; set; }

    [Parameter]
    public RenderFragment<WalletButtonViewModel> WalletConnectorButtonContent { get; set; } = null!;

    [Parameter]
    public RenderFragment<WalletsModalContentViewModel> WalletsModalContent { get; set; } = null!;

    public WalletExtensionState? ConnectedWallet { get; private set; }

    private string ModalContentPaddingStyle
    {
        get
        {
            if (HideCloseButton)
                return "pad-10";
            return "pad-tlb-10";
        }
    }

    private List<WalletExtensionState>? _wallets = new List<WalletExtensionState>();
    private DotNetObjectReference<WalletConnector>? _selfReference;
    private WalletConnectorJsInterop? _walletConnectorJs;
    private bool _connecting;
    private bool _showPopup;

    protected string DisconnectedButtonContent = "Initializing...";
    protected bool IsDisconnectedButtonDisabled = true;

    public bool Initialized { get; private set; }

    public bool Connected
    {
        get
        {
            return ConnectedWallet != null;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (_walletConnectorJs == null)
        {
            _walletConnectorJs = new WalletConnectorJsInterop(JS);
        }
        //check which wallets are installed
        //set up initial _wallets state from above
        _selfReference = DotNetObjectReference.Create(this);
        _wallets = await _walletConnectorJs.Init(SupportedExtensions, _selfReference);

        Initialized = true;

        await InitializePersistedWalletAsync();

        if (!Connected)
        {
            DisconnectedButtonContent = "Connect";
            IsDisconnectedButtonDisabled = false;
        }

        return;
    }

    private async Task InitializePersistedWalletAsync()
    {
        if (!PersistConnectedWallet) { return; }

        var supportedWalletKeys = SupportedExtensions?.Select(s => s.Key)?.ToArray();

        if (supportedWalletKeys != null && supportedWalletKeys.Length > 0)
        {
            var storedWalletKey = await GetStoredWalletKeyAsync(supportedWalletKeys);

            if (!string.IsNullOrWhiteSpace(storedWalletKey))
            {
                if (!await ConnectWalletAsync(storedWalletKey, false))
                {
                    await RemoveStoredWalletKeyAsync();
                }
            }
        }

        StateHasChanged();
    }

    private async Task<string> GetStoredWalletKeyAsync(params string[] supportedWalletKeys)
    {
        var result = string.Empty;

        try
        {
            if (_localStorage != null && supportedWalletKeys != null)
            {
                var walletKey = await _localStorage.GetItemAsStringAsync(_connectedWalletKey);

                if (!string.IsNullOrWhiteSpace(walletKey) && supportedWalletKeys.Any(w => w.Equals(walletKey, StringComparison.OrdinalIgnoreCase)))
                {
                    result = walletKey;
                }
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
        }

        return result;
    }

    private async Task SetStoredWalletKeyAsync(string walletKey)
    {
        if (PersistConnectedWallet && _localStorage != null && !string.IsNullOrWhiteSpace(walletKey))
        {
            await _localStorage.SetItemAsStringAsync(_connectedWalletKey, walletKey.ToString());
        }
    }

    private async Task RemoveStoredWalletKeyAsync()
    {
        if (PersistConnectedWallet && _localStorage != null)
        {
            await _localStorage.RemoveItemAsync(_connectedWalletKey);
        }
    }

    private void ModalClick()
    {
        Debug("MODAL CLICK");
        if (ShouldModalHide())
        {
            HideConnectWalletDialog();
        }
    }

    private bool ShouldModalHide()
    {
        if (HideOnModalClick == ModalClickHideOptions.Always)
            return true;
        if (HideOnModalClick == ModalClickHideOptions.WhenConnected && Connected)
            return true;
        return false;
    }

    private void Debug(string text)
    {
        if (OutputDebug)
        {
            Console.WriteLine(text);
        }
    }

    public async ValueTask RefreshConnectedWallet()
    {
        var balance = await GetBalance();
        if (balance != null)
        {
            ConnectedWallet!.TokenCount = 0;
            ConnectedWallet.TokenPreservation = 0;
            if (balance.MultiAsset != null && balance.MultiAsset.Count > 0)
            {
                ConnectedWallet.TokenPreservation = balance.MultiAsset.CalculateMinUtxoLovelace();
                ConnectedWallet.TokenCount = balance.MultiAsset.Sum(x => x.Value.Token.Keys.Count);
            }
            ConnectedWallet.Balance = balance.Coin - ConnectedWallet.TokenPreservation;
        }
        ConnectedWallet!.Network = await GetNetworkType();
        StateHasChanged();
    }

    public async ValueTask<bool> ConnectWalletAsync(string walletKey, bool suppressEvent = false)
    {
        if (walletKey == null)
        {
            _ = OnConnectError.InvokeAsync(new ArgumentNullException(nameof(walletKey))).ConfigureAwait(false);
            return false;
        }

        try
        {
            if (!suppressEvent)
                _ = OnConnectStart.InvokeAsync().ConfigureAwait(false);

            _connecting = true;
            StateHasChanged();

            var result = await _walletConnectorJs!.ConnectWallet(walletKey);
            if (result)
            {
                ConnectedWallet = _wallets!.First(x => x.Key == walletKey);
                await RefreshConnectedWallet();
                _wallets!.First(x => x.Key == walletKey).Connected = true;

                await SetStoredWalletKeyAsync(walletKey);

                if (AutoCloseOnConnect)
                {
                    _showPopup = false;
                }
            }
            if (!suppressEvent)
                _ = OnConnect.InvokeAsync();
            return result;
        }
        //suppress all errors as it could be valid user refusal
        //(cant get enough detail out of gero wallet to ensure specific handling)
        catch (ErrorCodeException ecex)
        {
            Debug("Caught error code exception: " + ecex.Code + " - " + ecex.Info);
            _ = OnConnectError.InvokeAsync(ecex);
        }
        catch (PaginateException pex)
        {
            Debug("Caught paginate exception: " + pex.MaxSize);
            _ = OnConnectError.InvokeAsync(pex);
        }
        catch (WebWalletException wex)
        {
            Debug("Caught web wallet exception: " + wex.Data.Keys.ToString());
            _ = OnConnectError.InvokeAsync(wex);
        }
        catch (Exception ex)
        {
            Debug("Caught exception: " + ex.Message);
            _ = OnConnectError.InvokeAsync(ex);
        }
        finally
        {
            _connecting = false;
            StateHasChanged();
        }
        return false;
    }

    public async ValueTask DisconnectWalletAsync(bool suppressEvent = false)
    {
        await _walletConnectorJs!.Disconnect();
        while (_wallets!.Any(x => x.Connected))
        {
            _wallets!.First(x => x.Connected).Connected = false;
        }
        ConnectedWallet = null;

        await RemoveStoredWalletKeyAsync();

        if (AutoCloseOnDisconnect)
        {
            _showPopup = false;
        }
        if (!suppressEvent)
            await OnDisconnect.InvokeAsync();
        return;
    }

    private void CheckInitialized()
    {
        if (!Initialized || _walletConnectorJs == null)
        {
            throw new InvalidOperationException("Component not initialized");
        }
    }

    private void CheckInitializedAndConnected()
    {
        CheckInitialized();
        if (!Connected)
        {
            throw new InvalidOperationException("No wallet connected");
        }
    }

    public async ValueTask OnConnectButtonClickAsync()
    {
        if (DisconnectOnConnectButtonClick)
            await DisconnectWalletAsync().ConfigureAwait(false);
        ShowConnectWalletDialog();
    }

    public void ShowConnectWalletDialog()
    {
        if (Connected)
        {
            _ = RefreshConnectedWallet();
        }
        _showPopup = true;
        StateHasChanged();
    }

    [JSInvokable]
    public void HideConnectWalletDialog()
    {
        _showPopup = false;
        StateHasChanged();
    }

    [JSInvokable]
    public void EscapeKeyPressed()
    {
        if (EscapeKeyPress == EscapeKeyOptions.Always ||
            EscapeKeyPress == EscapeKeyOptions.WhenConnected && Connected)
            HideConnectWalletDialog();
    }

    public List<WalletExtensionState> GetWalletExtensionStates()
    {
        CheckInitialized();
        return _walletConnectorJs!.Wallets!.Select(x => new WalletExtensionState(x)).ToList();
    }

    public async ValueTask<NetworkType> GetNetworkType()
    {
        var networkId = await GetNetworkTypeId();
        return ComponentUtils.GetNetworkType(networkId);
    }

    public async ValueTask<int> GetNetworkTypeId()
    {
        CheckInitializedAndConnected();
        var networkId = await _walletConnectorJs!.GetNetworkId();
        Debug($"NETWORK ID: {networkId}");
        return networkId;
    }

    public async ValueTask<Utxo[]> GetUtxos(TransactionOutputValue? requiredOutput = null, Paginate? paginate = null)
    {
        string? amountCbor = null;
        if (requiredOutput != null)
        {
            amountCbor = requiredOutput.Serialize().ToStringHex();
        }
        var utxoCbors = await GetUtxosCbor(amountCbor, paginate);
        var utxoList = new List<Utxo>();
        foreach (var utxoCbor in utxoCbors)
        {
            utxoList.Add(utxoCbor.HexToByteArray().DeserializeUtxo());
        }
        return utxoList.ToArray();
    }

    public async ValueTask<string[]> GetUtxosCbor(string? requiredOutputCbor = null, Paginate? paginate = null)
    {
        CheckInitializedAndConnected();
        var utxoCbors = await _walletConnectorJs!.GetUtxos(requiredOutputCbor, paginate);
        foreach (var utxoCbor in utxoCbors)
        {
            Debug($"UTXO CBOR: {utxoCbor}");
        }
        return utxoCbors;
    }

    public async ValueTask<Utxo[]> GetCollateral(CollateralParams collateralParams)
    {
        var utxoCbors = await GetCollateralCbor(collateralParams);
        var utxoList = new List<Utxo>();
        foreach (var utxoCbor in utxoCbors)
        {
            utxoList.Add(utxoCbor.HexToByteArray().DeserializeUtxo());
        }
        return utxoList.ToArray();
    }

    public async ValueTask<string[]> GetCollateralCbor(CollateralParams collateralParams)
    {
        CheckInitializedAndConnected();
        var utxoCbors = await _walletConnectorJs!.GetCollateral(collateralParams);
        foreach (var utxoCbor in utxoCbors)
        {
            Debug($"COLLATERAL CBOR: {utxoCbor}");
        }
        return utxoCbors;
    }

    public async ValueTask<TransactionOutputValue> GetBalance()
    {
        var result = await GetBalanceCbor();
        var cborObj = CBORObject.DecodeFromBytes(result.HexToByteArray());
        if (cborObj.Type == CBORType.Integer)
        {
            var number = cborObj.AsNumber();
            var coin = number.ToUInt64Checked();
            return new TransactionOutputValue() { Coin = coin };
        }

        var outputValue = result.HexToByteArray().DeserializeTransactionOutputValue();
        return outputValue;
    }

    public async ValueTask<string> GetBalanceCbor()
    {
        CheckInitializedAndConnected();
        var result = await _walletConnectorJs!.GetBalance();
        Debug($"BALANCE CBOR: {result}");
        return result;
    }

    public async ValueTask<Address[]> GetUsedAddresses(Paginate? paginate = null)
    {
        CheckInitializedAndConnected();
        var addresses = await GetUsedAddressesHex(paginate);
        var response = new List<Address>();
        foreach (var address in addresses)
        {
            response.Add(new Address(address.HexToByteArray()));
        }
        return response.ToArray();
    }

    public async ValueTask<string[]> GetUsedAddressesHex(Paginate? paginate = null)
    {
        CheckInitializedAndConnected();
        var addresses = await _walletConnectorJs!.GetUsedAddresses(paginate);
        foreach (var address in addresses)
        {
            Debug($"USED ADDRESS: {address}");
        }
        return addresses;
    }

    public async ValueTask<Address[]> GetUnusedAddresses()
    {
        var addresses = await GetUnusedAddressesHex();
        var response = new List<Address>();
        foreach (var address in addresses)
        {
            Debug($"UNUSED ADDRESS: {address}");
            response.Add(new Address(address.HexToByteArray()));
        }
        return response.ToArray();
    }

    public async ValueTask<string[]> GetUnusedAddressesHex()
    {
        CheckInitializedAndConnected();
        var addresses = await _walletConnectorJs!.GetUnusedAddresses();
        foreach (var address in addresses)
        {
            Debug($"UNUSED ADDRESS: {address}");
        }
        return addresses;
    }

    public async ValueTask<Address> GetChangeAddress()
    {
        var address = await GetChangeAddressHex();
        return new Address(address.HexToByteArray());
    }

    public async ValueTask<string> GetChangeAddressHex()
    {
        CheckInitializedAndConnected();
        var address = await _walletConnectorJs!.GetChangeAddress();
        Debug($"CHANGE ADDRESS: {address}");
        return address;
    }

    public async ValueTask<Address[]> GetRewardAddresses()
    {
        var addresses = await GetRewardAddressesHex();
        var response = new List<Address>();
        foreach (var address in addresses)
        {
            response.Add(new Address(address.HexToByteArray()));
        }
        return response.ToArray();
    }

    public async ValueTask<string[]> GetRewardAddressesHex()
    {
        CheckInitializedAndConnected();
        var addresses = await _walletConnectorJs!.GetRewardAddresses();
        foreach (var address in addresses)
        {
            Debug($"REWARD ADDRESS: {address}");
        }
        return addresses;
    }

    public async ValueTask<string> SignTx(Transaction tx, bool partialSign = false)
    {
        var txCbor = tx.Serialize().ToStringHex();
        return await SignTxCbor(txCbor, partialSign);
    }

    public async ValueTask<string> SignTxCbor(string txCbor, bool partialSign = false)
    {
        CheckInitializedAndConnected();
        Debug($"TX CBOR: {txCbor}");
        return await _walletConnectorJs!.SignTx(txCbor, partialSign);
    }

    public async ValueTask<DataSignature> SignData(string address, string hexData)
    {
        CheckInitializedAndConnected();
        return await _walletConnectorJs!.SignData(address, hexData);
    }

    public async ValueTask<string> SubmitTx(Transaction tx)
    {
        var txCbor = tx.Serialize().ToStringHex();
        return await SubmitTxCbor(txCbor);
    }

    public async ValueTask<string> SubmitTxCbor(string txCbor)
    {
        CheckInitializedAndConnected();
        Debug($"TX CBOR: {txCbor}");
        return await _walletConnectorJs!.SubmitTx(txCbor);
    }
}