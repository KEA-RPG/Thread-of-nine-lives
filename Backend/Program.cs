using Backend.Controllers;
using Backend.Repositories;
using Backend.Services;
using Infrastructure.Persistance.Relational;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "threadgame",
            ValidAudience = "threadgame",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UngnjU6otFg8IumrmGgl-MbWUUc9wMk0HR37M-VYs6s=")),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero  // Set clock skew to zero for precise validation
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

                // Access the claims directly from context.Principal
                var jti = context.Principal?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                Debug.WriteLine($"Checking cache for jti during validation: {jti}");

                if (jti != null && memoryCache.TryGetValue(jti, out _))
                {
                    context.Fail("This token has been revoked.");
                }
               

                return Task.CompletedTask;
            }
        };


    });

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
