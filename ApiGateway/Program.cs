var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var environmentName = builder.Environment.EnvironmentName;

// pull the config for the current environment.
builder.Configuration.AddJsonFile($"ocelot.{environmentName}.json", true, true);


// add the ocelot.json to the configuration
app.Configuration.

app.MapGet("/", () => "Hello World!");

app.Run();
