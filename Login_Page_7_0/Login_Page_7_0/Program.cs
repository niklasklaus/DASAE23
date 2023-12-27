/*using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Login_Page_7_0;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();*/

using Login_Page_7_0;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var host = builder.Build();
var navigationManager = host.Services.GetRequiredService<NavigationManager>();

// Hier "/login" durch den gewünschten Pfad ersetzen
var targetPage = "/login";

// Überprüfen, ob die aktuelle Seite bereits die Zielseite ist
if (!navigationManager.Uri.EndsWith(targetPage, StringComparison.OrdinalIgnoreCase))
{
    navigationManager.NavigateTo(targetPage, forceLoad: true);
}

await host.RunAsync();
