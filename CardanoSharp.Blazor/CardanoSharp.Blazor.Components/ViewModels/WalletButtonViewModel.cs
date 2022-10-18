using CardanoSharp.Blazor.Components.Models;

namespace CardanoSharp.Blazor.Components.ViewModels;

public class WalletButtonViewModel
{
	public WalletButtonViewModel(bool isConnected, WalletExtensionState? connectedWallet)
	{
		IsConnected = isConnected;
		ConnectedWallet = connectedWallet;
	}

	public bool IsConnected { get; set; }

	public WalletExtensionState? ConnectedWallet { get; set; }
}