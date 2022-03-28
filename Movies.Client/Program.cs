using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Movies.Client.ApiServices;
using Movies.Client.Configuration;
using Movies.Client.HttpHandlers;
using NuGet.Packaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomAppConfiguration(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMovieApiService, MovieApiService>();

var provider = builder.Services.BuildServiceProvider();

// pull the configuration for the openIdConnect because we will need it later one.
var openIdConnectConfiguration = provider.GetService<IOptions<OpenIdConnect>>().Value;
var servicesUrlConfiguration = provider.GetService<IOptions<ServiceUrls>>().Value;


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{

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

// 1 create an HttpClient used for accessing the Movies.Api
builder.Services.AddTransient<AuthenticationDelegateHandler>();

// configuration for the MovieClient Api
builder.Services.AddHttpClient(ApiConfigurations.MovieClient, client =>
{
    client.BaseAddress = new Uri(servicesUrlConfiguration.MovieApi);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddHttpMessageHandler<AuthenticationDelegateHandler>();

// configuration for the IDP CLient
builder.Services.AddHttpClient(ApiConfigurations.IDPClient, client =>
{
    client.BaseAddress = new Uri(openIdConnectConfiguration.Authority);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});


builder.Services.AddSingleton(new ClientCredentialsTokenRequest
{
    Address = $"{openIdConnectConfiguration.Authority}/connect/token",
    ClientId = "movieClient",
    ClientSecret = openIdConnectConfiguration.ClientSecret,
    Scope = openIdConnectConfiguration.MovieApiScope
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
