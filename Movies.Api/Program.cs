using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Data;
using Movies.Api.Utility;

var builder = WebApplication.CreateBuilder(args);

const string BearerSchema = "Bearer";
const string IDPAuthorityUrl = "https://localhost:5005";

builder.Services.AddDbContext<MoviesApiContext>(options =>
    options.UseInMemoryDatabase("Movies"));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(BearerSchema).AddJwtBearer(BearerSchema, options =>
{
    options.Authority = IDPAuthorityUrl;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthenticationPolicy.ClientIdPolicy, policy => policy.RequireClaim(ClaimNames.ClientId, ClaimNames.MovieClient));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var moviesContext = services.GetRequiredService<MoviesApiContext>();
    MoviesContextSeed.SeedAsync(moviesContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// perform Authentication on every request.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();