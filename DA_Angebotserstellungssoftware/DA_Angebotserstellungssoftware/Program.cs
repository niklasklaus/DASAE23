using Blazored.Modal;
using DA_Angebotserstellungssoftware;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using DA_Angebotserstellungssoftware.Data;
using DA_Angebotserstellungssoftware.InsertCustomerData;
using DA_Angebotserstellungssoftware;
using DA_Angebotserstellungssoftware.Proposals;
using Radzen;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddBlazoredModal();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<MySqlConnectionManager>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new MySqlConnectionManager(configuration);
});

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<InsertCustomerDataService>();
builder.Services.AddScoped<InsertLVsService>();
builder.Services.AddScoped<InsertEffortAndDiscountService>();
builder.Services.AddScoped<SharedService>();
builder.Services.AddScoped<InsertPaymentTermService>();
builder.Services.AddScoped<SearchProposalService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();