using IdentityServer;
using IdentityServer.Data.Migrations;
using IdentityServerHost.Quickstart.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var identityConnection = builder.Configuration.GetConnectionString("IdentityConnectionString");
var migrationsAssembly = "IdentityServer";//typeof(Config).GetType().Assembly.GetName().Name;

// adds Identity Server's DI services.
builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddTestUsers(Config.TestUsers)
    .AddTestUsers(TestUsers.Users)
    .AddDeveloperSigningCredential();
    //.AddConfigurationStore(options => {
    //    options.ConfigureDbContext = d => d.UseSqlServer(identityConnection, 
    //    sql => sql.MigrationsAssembly(migrationsAssembly));
    //}).AddOperationalStore(options => {
    //    options.ConfigureDbContext = b => b.UseSqlServer(identityConnection, sql => sql.MigrationsAssembly(migrationsAssembly));
    //});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

//SeedData.InitializeDatabase(app);

// add it to the middleware
app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();



