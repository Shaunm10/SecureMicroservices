using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Movies.Client.ApiServices;
using Movies.Client.Configuration;
using NuGet.Packaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomAppConfiguration(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMovieApiService, MovieApiService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    var provider = builder.Services.BuildServiceProvider();
    var openIdConnectConfiguration = provider.GetService<IOptions<OpenIdConnect>>().Value;
   
    // the address of the IPD server
    options.Authority = openIdConnectConfiguration.Authority; 

    // who this client is
    options.ClientId = openIdConnectConfiguration.ClientId;

    // the secret we are using
    options.ClientSecret = openIdConnectConfiguration.ClientSecret; 

    // which flow this application is using.
    options.ResponseType =
        openIdConnectConfiguration.ResponseType; 

    // the scopes we are requesting
    options.Scope.AddRange(openIdConnectConfiguration.Scopes);

    options.SaveTokens = openIdConnectConfiguration.SaveTokens;

    // if we should get claims from the endpoint after authenticating.
    options.GetClaimsFromUserInfoEndpoint = openIdConnectConfiguration.GetClaimsFromUserInputEndpoint;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
