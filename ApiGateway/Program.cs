var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var environmentName = builder.Environment.EnvironmentName;

// pull the config for the current environment.
builder.Configuration.AddJsonFile($"ocelot.json", true, true);



app.MapGet("/", () => "Hello World!");

app.Run();
