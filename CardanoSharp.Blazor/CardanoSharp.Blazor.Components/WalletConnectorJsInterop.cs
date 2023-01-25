using CardanoSharp.Blazor.Components.Enums;
using CardanoSharp.Blazor.Components.Extensions;
using CardanoSharp.Blazor.Components.Models;
using CardanoSharp.Wallet.CIPs.CIP30.Models;
using Microsoft.JSInterop;

namespace CardanoSharp.Blazor.Components
{
	// This class provides an example of how JavaScript functionality can be wrapped
	// in a .NET class for easy consumption. The associated JavaScript module is
	// loaded on demand when first needed.
	//
	// This class can be registered as scoped DI service and then injected into Blazor
	// components for use.

	public class WalletConnectorJsInterop : IAsyncDisposable
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
		private IJSObjectReference? _jsWalletConnector;

		public bool Initialized { get; private set; }

		public WalletExtension? ConnectedWallet { get; private set; }

		public List<WalletExtensionState>? Wallets { get; private set; }

		public WalletConnectorJsInterop(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_moduleTask = new(() => _jsRuntime.InvokeAsync<IJSObjectReference>(
			   "import", "./_content/CardanoSharp.Blazor.Components/WalletConnectorJsInterop.js").AsTask());
		}

		public async ValueTask<List<WalletExtensionState>> Init(IEnumerable<WalletExtension> supportedWallets, DotNetObjectReference<WalletConnector> walletObj)
		{
			var module = await _moduleTask.Value;
			_jsWalletConnector = await module.InvokeAsync<IJSObjectReference>("createWalletConnector");

			await _jsWalletConnector.InvokeVoidAsync("init", walletObj);

			Wallets = new List<WalletExtensionState>();
			foreach (var wallet in supportedWallets)
			{
				var walletState = new WalletExtensionState(wallet);
				if (!String.IsNullOrEmpty(walletState.Key))
				{
					walletState.Installed = await IsWalletInstalled(walletState.Key);
					Wallets.Add(walletState);
				}
			}
			Initialized = true;
			return Wallets;
		}

		public async ValueTask<bool> IsWalletInstalled(string key)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				return await _jsWalletConnector!.InvokeAsync<bool>("isWalletInstalled", key);
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<bool> IsWalletEnabled(string key)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				return await _jsWalletConnector!.InvokeAsync<bool>("isWalletEnabled", key);
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string> GetWalletApiVersion(string key)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				return await _jsWalletConnector!.InvokeAsync<string>("getWalletApiVersion", key);
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string> GetWalletName(string key)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				return await _jsWalletConnector!.InvokeAsync<string>("getWalletName", key);
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string> GetWalletIcon(string key)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				return await _jsWalletConnector!.InvokeAsync<string>("getWalletIcon", key);
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<bool> ConnectWallet(string key)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				return await _jsWalletConnector!.InvokeAsync<bool>("connectWallet", key);
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask Disconnect()
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				ConnectedWallet = null;
				await _jsWalletConnector!.InvokeVoidAsync("disconnect");
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<int> GetNetworkId()
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var networkId = await _jsWalletConnector!.InvokeAsync<int>("getNetworkId");
				return networkId;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string[]> GetUtxos(string? amountCbor = null, Paginate? paginate = null)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var utxos = await _jsWalletConnector!.InvokeAsync<string[]>("getUtxos", amountCbor, paginate);
				return utxos;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string[]> GetCollateral(CollateralParams collateralParams)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var utxos = await _jsWalletConnector!.InvokeAsync<string[]>("getCollateral", collateralParams);
				return utxos;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string> GetBalance()
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var balance = await _jsWalletConnector!.InvokeAsync<string>("getBalance");
				return balance;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string[]> GetUsedAddresses(Paginate? paginate = null)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var addresses = await _jsWalletConnector!.InvokeAsync<string[]>("getUsedAddresses", paginate);
				return addresses;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string[]> GetUnusedAddresses()
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var addresses = await _jsWalletConnector!.InvokeAsync<string[]>("getUnusedAddresses");
				return addresses;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string> GetChangeAddress()
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var address = await _jsWalletConnector!.InvokeAsync<string>("getChangeAddress");
				return address;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string[]> GetRewardAddresses()
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var addresses = await _jsWalletConnector!.InvokeAsync<string[]>("getRewardAddresses");
				return addresses;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException();
			}
		}

		public async ValueTask<string> SignTx(string txCbor, bool partialSign = false)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var cborWitnessSet = await _jsWalletConnector!.InvokeAsync<string>("signTx", txCbor, partialSign);
				return cborWitnessSet;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException(WebWalletErrorType.TxSign);
			}
		}

		public async ValueTask<DataSignature> SignData(string address, string hexData)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var dataSignature = await _jsWalletConnector!.InvokeAsync<DataSignature>("signData", address, hexData);
				return dataSignature;
			}
			catch (JSException ex)
			{
				throw ex.ToWebWalletException(WebWalletErrorType.DataSign);
			}
		}

		public async ValueTask<string> SubmitTx(string txCbor)
		{
			_jsWalletConnector.ThrowIfNull();
			try
			{
				var txHash = await _jsWalletConnector!.InvokeAsync<string>("submitTx", txCbor);
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

			if (_moduleTask.IsValueCreated)
			{
				var module = await _moduleTask.Value;
				await module.DisposeAsync();
			}
		}
	}
}
