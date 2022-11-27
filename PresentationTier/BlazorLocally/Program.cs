using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorLocally;
using HttpClient.ClientImplementations;
using HttpClient.ClientInterfaces;
using SoloX.BlazorJsBlob;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new System.Net.Http.HttpClient { BaseAddress = new Uri("https://localhost:7086") });
builder.Services.AddScoped<IOfferService, OfferHttpClient>();
builder.Services.AddScoped<IFarmService, FarmHttpClient>();
builder.Services.AddScoped<IAuthService, AuthHttpClient>();


await builder.Build().RunAsync();


// TODO: https://www.youtube.com/watch?v=iedIu9H982Q