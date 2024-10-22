using Backend;
using Backend.Controllers;
using Backend.Repositories;
using Backend.Services;
using Infrastructure.Persistance.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Backend; 


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(
    issuer: "threadgame",
    audience: "threadgame",
    signingKey: "UngnjU6otFg8IumrmGgl-MbWUUc9wMk0HR37M-VYs6s="
);

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

builder.Services.AddDbContext<RelationalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")));

builder.Services.AddScoped<IEnemyService, EnemyService>();
builder.Services.AddScoped<IEnemyRepository, EnemyRepository>();
builder.Services.AddMemoryCache(); // Bruger vi til in-memory caching for blacklisting tokens


builder.Services.AddCors(p => p.AddPolicy("*", b =>
b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
//PersistanceConfiguration.ConfigureServices(builder.Services, builder.Configuration, "relational");
var app = builder.Build();

// Map controllers
CardController.MapCardEndpoint(app);
AuthController.MapAuthEndpoints(app);
EnemyController.MapEnemyEndpoint(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("*");
app.Run();
