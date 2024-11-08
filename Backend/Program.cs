using Backend;
using Backend.Controllers;
using Backend.Repositories;
using Backend.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // Add security definition for JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeckRepository, DeckRepository>();
builder.Services.AddScoped<IDeckService, DeckService>();




builder.Services.AddScoped<IEnemyService, EnemyService>();
builder.Services.AddScoped<IEnemyRepository, EnemyRepository>();
builder.Services.AddMemoryCache(); // Bruger vi til in-memory caching for blacklisting tokens


builder.Services.AddCors(p => p.AddPolicy("*", b =>
b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));


PersistanceConfiguration.ConfigureServices(builder.Services, dbtype.DefaultConnection);


var app = builder.Build();

// Map controllers
CardController.MapCardEndpoint(app);
AuthController.MapAuthEndpoints(app);
EnemyController.MapEnemyEndpoint(app);
DeckController.MapDeckEndpoint(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("*");
app.Run();
