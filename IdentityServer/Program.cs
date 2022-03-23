using IdentityServer4.Models;
using IdentityServer4.Test;

var builder = WebApplication.CreateBuilder(args);

// adds Identity Server's DI services.
builder.Services.AddIdentityServer()
    .AddInMemoryClients(new List<Client>())
    .AddInMemoryIdentityResources(new List<IdentityResource>())
    .AddInMemoryApiResources(new List<ApiResource>())
    .AddInMemoryApiScopes(new List<ApiScope>())
    .AddTestUsers(new List<TestUser>())
    .AddDeveloperSigningCredential();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// add it to the middleware
app.UseIdentityServer();
app.Run();
