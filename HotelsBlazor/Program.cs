using Blazored.LocalStorage;
using HotelsBlazor;
using HotelsBlazor.Managers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<RequestManager>();
builder.Services.AddScoped<HotelsManager>();
builder.Services.AddBlazoredLocalStorage();

//builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7025") });

await builder.Build().RunAsync();
