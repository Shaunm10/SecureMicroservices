using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// pull the config for the current environment.
builder.Configuration.AddJsonFile($"ocelot.json", true, true);

builder.Services.AddOcelot();

var app = builder.Build();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// finally use the Ocelot gateway
await app.UseOcelot();
//app.MapGet("/", () => "Ocelot Gatway");

app.Run();
