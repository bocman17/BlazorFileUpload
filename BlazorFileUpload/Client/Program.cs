global using BlazorFileUpload.Shared;
global using System.Net.Http.Json;
global using BlazorFileUpload.Client.Services.UploadService;
using BlazorFileUpload.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IUploadService, UploadService>();

await builder.Build().RunAsync();
