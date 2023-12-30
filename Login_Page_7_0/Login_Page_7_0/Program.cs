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
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
/*builder.Services.AddScoped<MySqlConnectionManager>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new MySqlConnectionManager(configuration);
});

builder.Services.AddScoped<LoginService>();*/

var connectionString = "Server=localhost;Database=da_dbschema;User Id=root;Password=root;"; // Hier deine Verbindungszeichenfolge einfügen
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Weitere Dienstregistrierungen...
builder.Services.AddScoped<LoginService>();

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
