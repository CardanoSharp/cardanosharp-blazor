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
@using CardanoSharp.Blazor.Components;<br/>
@using CardanoSharp.Blazor.Components.Models;<br/>
@using CardanoSharp.Blazor.Components.Enums;<br/>
<br/>
&lt;WalletConnector<br/>
&nbsp;&nbsp;@ref="myWebWallets" <br/>
&nbsp;&nbsp;Header="Please choose a wallet"<br/>
&nbsp;&nbsp;AutoCloseOnConnect="false"<br/>
&nbsp;&nbsp;AutoCloseOnDisconnect="true"<br/>
&nbsp;&nbsp;Layout="WalletConnectorLayout.List" <br/>
&nbsp;&nbsp;SupportedExtensions="<br/>
&nbsp;&nbsp;&nbsp;&nbsp;@(new List<WalletExtension>()<br/>
&nbsp;&nbsp;&nbsp;&nbsp;{<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;new WalletExtension() { <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Key = "eternl", <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name = "Eternl", <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Icon = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAYAAABccqhmAAAAG........",<br/> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Url = "https://eternl.io"<br/> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;new WalletExtension() { <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Key = "gerowallet", <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name = "Gero", <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Icon = "https://gerowallet.io/assets/img/logo2.ico", <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Url = "https://gerowallet.io" <br/>
&nbsp;&nbsp;&nbsp;&nbsp;},<br/>
&nbsp;&nbsp;&nbsp;&nbsp;})<br/>
&nbsp;&nbsp;"&gt;<br/>
&lt;/WalletConnector&gt;

@code { <br/>
&nbsp;&nbsp;if (myWebWallets != null && myWebWallets.Initialized)<br/>
&nbsp;&nbsp;{<br/>
&nbsp;&nbsp;&nbsp;&nbsp;if (myWebWallets.Connected)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;{<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;var result = await myWebWallets.GetBalance();<br/>
&nbsp;&nbsp;&nbsp;&nbsp;}<br/>
&nbsp;&nbsp;}<br/>
}<br/>
```
