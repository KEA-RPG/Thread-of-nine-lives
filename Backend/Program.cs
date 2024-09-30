using Backend.Repositories;
using Backend.Services;
using Infrastructure.Persistance.Relational;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// JWT Authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        ValidIssuer = "threadgame",
        ValidAudience = "threadgame",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A7c$7DFG9!fQ2@Vbn#4gTxlpT67^n8#QhE"))
    };
});

// Add services to the container
builder.Services.AddControllers(); // Add controller support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Logging.ClearProviders(); // Clears default logging providers
builder.Logging.AddConsole(); // Enables console logging
builder.Logging.AddDebug(); // Enables debug logging (visible in Visual Studio's output window)
builder.Logging.SetMinimumLevel(LogLevel.Information); // Sets the minimum level to log


builder.Services.AddDbContext<RelationalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")));

// Build the app
var app = builder.Build();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Use Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Map controllers with attribute-based routing
app.MapControllers(); // Automatically map controllers

// Default route
app.MapGet("/", () => "Hello World!");

app.Run();
