using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Web;
using TopSwagCode.Blazor.Server.Hubs;
using TopSwagCode.Blazor.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //options.Authority = builder.Configuration["Auth0:Authority"];
    //options.Audience = builder.Configuration["Auth0:ApiIdentifier"];
    options.Authority = "https://dev-oloabuzf.us.auth0.com/";
    options.Audience = "https://topswagcode.com";
});

builder.Services.Configure<ComputerVisionOptions>(
    builder.Configuration.GetSection(ComputerVisionOptions.Position));

builder.Services.AddScoped<IComputerVisionService, ComputerVisionService>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToFile("index.html");

app.Run();
