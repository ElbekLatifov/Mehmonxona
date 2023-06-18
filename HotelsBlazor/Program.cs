using Blazored.LocalStorage;
using HotelsBlazor;
using HotelsBlazor.Managers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<RequestManager>();
builder.Services.AddScoped<HotelsManager>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7124") });

await builder.Build().RunAsync();
