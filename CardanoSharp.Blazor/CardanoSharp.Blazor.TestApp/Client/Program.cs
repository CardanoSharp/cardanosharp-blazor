using CardanoSharp.Blazor.Components.Services;
using CardanoSharp.Blazor.TestApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddCardanoWalletConnector();

// The list of wallets can be overrode like this:
// builder.Services.AddCardanoWalletConnector(new WalletExtension {Name = "Nami", Key = "nami"}, new WalletExtension {Name = "Flint", Key = "flint"});

await builder.Build().RunAsync();
