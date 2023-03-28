using CardanoSharp.Blazor.Components.Models;

namespace CardanoSharp.Blazor.Components.ViewModels;

public class WalletButtonViewModel
{
	public WalletButtonViewModel(bool isConnected, WalletExtensionState? connectedWallet, bool initialized)
	{
		IsConnected = isConnected;
		ConnectedWallet = connectedWallet;
		Initialized = initialized;
	}

	public bool Initialized { get; set; }

	public bool IsConnected { get; set; }

	public WalletExtensionState? ConnectedWallet { get; set; }
}