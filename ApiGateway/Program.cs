using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// pull the config for the current environment.
builder.Configuration.AddJsonFile($"ocelot.json", true, true);

// TODO: pull from configuration 
var authenticationProviderKey = "IdentityApiKey";

builder.Services.AddAuthentication().AddJwtBearer(authenticationProviderKey, x =>
{
    var authorityUrl = builder.Configuration.GetSection("OpenIdConnect")["AuthorityUrl"];

    // add to configuration
    x.Authority = authorityUrl; // IdentityServerUrl
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };
});

builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// finally use the Ocelot gateway
await app.UseOcelot();
//app.MapGet("/", () => "Ocelot Gatway");

app.Run();
