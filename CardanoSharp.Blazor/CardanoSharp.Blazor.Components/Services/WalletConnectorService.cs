using CardanoSharp.Blazor.Components.Enums;
using CardanoSharp.Blazor.Components.Exceptions;
using CardanoSharp.Blazor.Components.Extensions;
using CardanoSharp.Blazor.Components.Models;
using CardanoSharp.Blazor.Components.Utils;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Extensions.Models.Transactions;
using CardanoSharp.Wallet.Models.Addresses;
using CardanoSharp.Wallet.Models.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using PeterO.Cbor2;

namespace CardanoSharp.Blazor.Components.Services
{
    // This class can be registered as scoped DI service (by adding services.AddCardanoWalletConnector()) and then
    // injected into Blazor components for use
    public class WalletConnectorService : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> _walletJsModuleTask;
        private IJSObjectReference? _jsWalletConnector;

        public WalletExtensionState? ConnectedWallet { get; private set; }

        public bool Connecting { get; set; }
        
        public async ValueTask<List<WalletExtensionState>> GetWallets()
        {
            var wallets = new List<WalletExtensionState>();
            foreach (WalletExtensionState walletState in SupportedWallets
                         .Select(wallet => new WalletExtensionState(wallet))
                         .Where(walletState => !string.IsNullOrEmpty(walletState.Key)))
            {
                walletState.Installed = await IsWalletInstalled(walletState.Key);
                walletState.Connected = walletState.Key.Equals(ConnectedWallet?.Key);
                wallets.Add(walletState);
            }

            return wallets;
        }

        public WalletConnectorService(IJSRuntime jsRuntime, params WalletExtension[] acceptedWallets)
        {
            _walletJsModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/CardanoSharp.Blazor.Components/WalletConnectorJsInterop.js").AsTask());

            if (acceptedWallets.Length > 0)
            {
                SupportedWallets = acceptedWallets.ToList();
            }
        }

        private async ValueTask<IJSObjectReference> GetWalletConnector()
        {
            if (_jsWalletConnector is not null)
            {
                return _jsWalletConnector;
            }

            IJSObjectReference module = await _walletJsModuleTask.Value;
            _jsWalletConnector = await module.InvokeAsync<IJSObjectReference>("createWalletConnector");

            return _jsWalletConnector;
        }

        public async ValueTask<bool> IsWalletInstalled(string key)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                return await jsWalletConnector.InvokeAsync<bool>("isWalletInstalled", key);
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        public async ValueTask<bool> IsWalletEnabled(string key)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                return await jsWalletConnector.InvokeAsync<bool>("isWalletEnabled", key);
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        public async ValueTask<string> GetWalletApiVersion(string key)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                return await jsWalletConnector.InvokeAsync<string>("getWalletApiVersion", key);
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        public async ValueTask<string> GetWalletName(string key)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                return await jsWalletConnector.InvokeAsync<string>("getWalletName", key);
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        public async ValueTask<string> GetWalletIcon(string key)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                return await jsWalletConnector.InvokeAsync<string>("getWalletIcon", key);
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<bool> ConnectWallet(string key)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                return await jsWalletConnector.InvokeAsync<bool>("connectWallet", key);
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        public async ValueTask Disconnect()
        {
            try
            {
                ConnectedWallet = null;
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                await jsWalletConnector.InvokeVoidAsync("disconnect");
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<int> GetNetworkIdCbor()
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var networkId = await jsWalletConnector.InvokeAsync<int>("getNetworkId");
                return networkId;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string[]> GetUtxosCbor(string? amountCbor = null, Paginate? paginate = null)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var utxos = await jsWalletConnector.InvokeAsync<string[]>("getUtxos", amountCbor, paginate);
                return utxos;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string[]> GetCollateralCbor(CollateralParams collateralParams)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var utxos = await jsWalletConnector.InvokeAsync<string[]>("getCollateral", collateralParams);
                return utxos;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string> GetBalanceCbor()
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var balance = await jsWalletConnector.InvokeAsync<string>("getBalance");
                return balance;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string[]> GetUsedAddressesHex(Paginate? paginate = null)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var addresses = await jsWalletConnector.InvokeAsync<string[]>("getUsedAddresses", paginate);
                return addresses;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string[]> GetUnusedAddressesHex()
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var addresses = await jsWalletConnector.InvokeAsync<string[]>("getUsedAddresses");
                return addresses;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string> GetChangeAddressHex()
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var address = await jsWalletConnector.InvokeAsync<string>("getChangeAddress");
                return address;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string[]> GetRewardAddressesHex()
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var addresses = await jsWalletConnector.InvokeAsync<string[]>("getRewardAddresses");
                return addresses;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException();
            }
        }

        private async ValueTask<string> SignTxCbor(string txCbor, bool partialSign = false)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var cborWitnessSet = await jsWalletConnector.InvokeAsync<string>("signTx", txCbor, partialSign);
                return cborWitnessSet;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException(WebWalletErrorType.TxSign);
            }
        }

        public async ValueTask<DataSignature> SignDataHex(string address, string hexData)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var dataSignature = await jsWalletConnector.InvokeAsync<DataSignature>("signData", address, hexData);
                return dataSignature;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException(WebWalletErrorType.DataSign);
            }
        }

        private async ValueTask<string> SubmitTxCbor(string txCbor)
        {
            try
            {
                IJSObjectReference jsWalletConnector = await GetWalletConnector();
                var txHash = await jsWalletConnector.InvokeAsync<string>("submitTx", txCbor);
                return txHash;
            }
            catch (JSException ex)
            {
                throw ex.ToWebWalletException(WebWalletErrorType.TxSend);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_jsWalletConnector != null)
            {
                await _jsWalletConnector.DisposeAsync();
            }

            if (_walletJsModuleTask.IsValueCreated)
            {
                var module = await _walletJsModuleTask.Value;
                await module.DisposeAsync();
            }
        }

        public async ValueTask RefreshConnectedWallet()
        {
            TransactionOutputValue balance = await GetBalance();
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
        }

        public async ValueTask<bool> ConnectWalletAsync(string walletKey, bool suppressEvent = false)
        {
            Connecting = true;
            try
            {
                bool connected = await ConnectWallet(walletKey);
                if (!connected)
                {
                    return false;
                }
                
                ConnectedWallet = (await GetWallets()).First(x => x.Key == walletKey);
                await RefreshConnectedWallet();

                return true;
            }
            catch (ErrorCodeException ecex)
            {
                throw new Exception("Caught error code exception: " + ecex.Code + " - " + ecex.Info);
            }
            catch (PaginateException pex)
            {
                throw new Exception("Caught paginate exception: " + pex.MaxSize);
            }
            catch (WebWalletException wex)
            {
                throw new Exception("Caught web wallet exception: " + wex.Message);
            }
            finally
            {
                Connecting = false;
            }
        }
        
        public async ValueTask DisconnectWalletAsync(bool suppressEvent = false)
        {
            await Disconnect();
            ConnectedWallet = null;
        }

        public async ValueTask<NetworkType> GetNetworkType()
        {
            var networkId = await GetNetworkIdCbor();
            return ComponentUtils.GetNetworkType(networkId);
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

        public async ValueTask<TransactionOutputValue> GetBalance()
        {
            var result = await GetBalanceCbor();
            var cborObj = CBORObject.DecodeFromBytes(result.HexToByteArray());
            if (cborObj.Type == CBORType.Integer)
            {
                var number = cborObj.AsNumber();
                var coin = number.ToUInt64Checked();
                return new TransactionOutputValue() {Coin = coin};
            }

            var outputValue = result.HexToByteArray().DeserializeTransactionOutputValue();
            return outputValue;
        }

        public async ValueTask<Address[]> GetUsedAddresses(Paginate? paginate = null)
        {
            var addresses = await GetUsedAddressesHex(paginate);
            var response = new List<Address>();
            foreach (var address in addresses)
            {
                response.Add(new Address(ComponentUtils.GetPrefix(AddressType.Base, ConnectedWallet!.Network), address.HexToByteArray()));
            }

            return response.ToArray();
        }

        public async ValueTask<Address[]> GetUnusedAddresses()
        {
            var addresses = await GetUnusedAddressesHex();
            var response = new List<Address>();
            foreach (var address in addresses)
            {
                response.Add(new Address(ComponentUtils.GetPrefix(AddressType.Base, ConnectedWallet!.Network), address.HexToByteArray()));
            }

            return response.ToArray();
        }

        public async ValueTask<Address> GetChangeAddress()
        {
            var address = await GetChangeAddressHex();
            return new Address(ComponentUtils.GetPrefix(AddressType.Base, ConnectedWallet!.Network), address.HexToByteArray());
        }

        public async ValueTask<Address[]> GetRewardAddresses()
        {
            var addresses = await GetRewardAddressesHex();
            var response = new List<Address>();
            foreach (var address in addresses)
            {
                response.Add(new Address(ComponentUtils.GetPrefix(AddressType.Reward, ConnectedWallet!.Network),
                    address.HexToByteArray()));
            }

            return response.ToArray();
        }

        public async ValueTask<string> SignTx(Transaction tx, bool partialSign = false)
        {
            var txCbor = tx.Serialize().ToStringHex();
            return await SignTxCbor(txCbor, partialSign);
        }

        public async ValueTask<string> SubmitTx(Transaction tx)
        {
            var txCbor = tx.Serialize().ToStringHex();
            return await SubmitTxCbor(txCbor);
        }

        public List<WalletExtension> SupportedWallets { get; } = new()
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
        };
    }
    
    public static class ServicesConfiguration
    {
        public static void AddCardanoWalletConnector(this IServiceCollection services, params WalletExtension[] acceptedWallets)
        {
            services.AddScoped(x => new WalletConnectorService(x.GetRequiredService<IJSRuntime>(), acceptedWallets));
        }
    }

}