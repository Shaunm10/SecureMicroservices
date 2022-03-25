using IdentityServer;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// adds Identity Server's DI services.
builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

// add it to the middleware
app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
