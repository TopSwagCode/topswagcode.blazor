using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using TopSwagCode.Blazor.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAntDesign();

builder.Services.AddHttpClient<LocalHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient<ServerlessHttpClient>(
        client => client.BaseAddress = new Uri("https://serverless.topswagcode.dev/api/"))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
    .ConfigureHandler(
        authorizedUrls: new[] { "https://serverless.topswagcode.dev/api/" }));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.PostLogoutRedirectUri = "https://topswagcode.dev";
}).AddAccountClaimsPrincipalFactory<
ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

builder.Services.AddSingleton<IVersionService, WasmVersionService>(b => new WasmVersionService
{
    Version = Assembly.GetExecutingAssembly().
    GetCustomAttribute<AssemblyInformationalVersionAttribute>().
    InformationalVersion
});

await builder.Build().RunAsync();
