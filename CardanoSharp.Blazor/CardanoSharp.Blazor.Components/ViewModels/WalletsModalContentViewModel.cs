using CardanoSharp.Blazor.Components.Models;

namespace CardanoSharp.Blazor.Components.ViewModels;

public class WalletsModalContentViewModel
{
	public WalletsModalContentViewModel(bool initialized, List<WalletExtensionState> wallets, bool isWalletConnected, WalletExtensionState connectedWallet, Func<string, bool, ValueTask<bool>> connectWalletAsync, Func<bool, ValueTask> disconnectWalletAsync)
	{
		Initialized = initialized;
		Wallets = wallets;
		IsWalletConnected = isWalletConnected;
		ConnectedWallet = connectedWallet;
		ConnectWalletAsync = connectWalletAsync;
		DisconnectWalletAsync = disconnectWalletAsync;
	}

	public bool Initialized { get; set; }

	public List<WalletExtensionState> Wallets { get; set; }

	public bool IsWalletConnected { get; set; }

	public WalletExtensionState ConnectedWallet { get; set; }

	public Func<string, bool, ValueTask<bool>> ConnectWalletAsync { get; set; }

	public Func<bool, ValueTask> DisconnectWalletAsync { get; set; }
}