using Blazored.LocalStorage;
using ClientWA;
using ClientWA.Services.Class;
using ClientWA.Services.Interfaces;
using Domain.Service.Class;
using Domain.Service.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CookieHandler>();
builder.Services.AddHttpClient("Server", x =>  x.BaseAddress = new Uri("https://localhost:7248/api/"))
.AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(typeof(IJwtService<>), typeof(JwtService<>));
builder.Services.AddScoped<IEncreption, Encreption>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IApiService,ApiService>();
builder.Services.AddScoped<Auth>();

await builder.Build().RunAsync();
