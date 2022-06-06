# cardanosharp-blazor

CardanoSharp.Blazor is a component library for C# devs to easily get started with developing dApps for Cardano.

## WalletConnector

The wallet connector will give users the option of connecting to any of a preset list of known wallets.

### Parameters
WalletConnector has the follow parameters that can be set as required:

1. Header [*"Choose Your Wallet"*] - Text to display above the wallet options
2. AutoCloseOnConnect [*false*] - Whether to close the wallet choice popup after connecting to a wallet
3. AutoCloseOnDisconnect [*false*] - Whether to close the wallet choice popup after disconnecting from a wallet
4. Layout [*Card*] - Either List or Card 
5. SupportedExtensions [Eternl,Gero,Nami] - If you want to override the current known list of wallet browser extensions, and their applicable attributes (name, icon etc)

### Usage
```
@using CardanoSharp.Blazor.Components;
@using CardanoSharp.Blazor.Components.Models;
@using CardanoSharp.Blazor.Components.Enums;

<WalletConnector
  @ref="myWebWallets" 
  Header="Please choose a wallet"
  AutoCloseOnConnect="false"
  AutoCloseOnDisconnect="true"
  Layout="WalletConnectorLayout.List" 
  SupportedExtensions="
    @(new List<WalletExtension>()
    {
      new WalletExtension() { 
        Key = "eternl", 
        Name = "Eternl", 
        Icon = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAYAAABccqhmAAAAG........", 
        Url = "https://eternl.io" 
      },
      new WalletExtension() { 
        Key = "gerowallet", 
        Name = "Gero", 
        Icon = "https://gerowallet.io/assets/img/logo2.ico", 
        Url = "https://gerowallet.io" 
    },
    })
  ">
</WalletConnector>

@code { 
  if (myWebWallets != null && myWebWallets.Initialized)
  {
    if (myWebWallets.Connected)
    {
      var result = await myWebWallets.GetBalance();
    }
  }
}
```
