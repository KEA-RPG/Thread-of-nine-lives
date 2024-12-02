using Backend;
using Backend.Controllers;
using Backend.Repositories.Document;
using Backend.Repositories.Interfaces;
using Backend.Repositories.Relational;
using Backend.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace Backend
{
    public class Program
    {
        private static void Main(string[] args)
        {
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
            builder.Services.AddScoped<ICombatRepository, CombatRepository>();
            builder.Services.AddScoped<ICombatService, CombatService>();
            builder.Services.AddScoped<IEnemyService, EnemyService>();
            builder.Services.AddScoped<IEnemyRepository, EnemyRepository>();
            builder.Services.AddMemoryCache(); // Bruger vi til in-memory caching for blacklisting tokens


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:5173")  // Specify the allowed origin (frontend)
                           .AllowAnyHeader()                      // Allow all headers (e.g., Authorization, Content-Type, etc.)
                           .AllowAnyMethod()                      // Allow all HTTP methods (e.g., GET, POST, PUT, DELETE)
                           .AllowCredentials();                   // Allow cookies and Authorization headers to be sent with the request
                });
            });


            PersistanceConfiguration.ConfigureServices(builder.Services, dbtype.DefaultConnection);

            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; // Custom header for CSRF token
                options.Cookie.Name = ".AspNetCore.Antiforgery"; // Default antiforgery cookie name
                options.Cookie.SameSite = SameSiteMode.Strict; // Use Lax for local development to ease cross-origin issues
                options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Allow cookies over HTTP. Skal v�re HTTPS optimalt set
            });


            var app = builder.Build();


            // Map controllers
            app.MapCardEndpoint();
            app.MapAuthEndpoints();
            app.MapEnemyEndpoint();
            app.MapDeckEndpoint();

            app.MapCombatEndpoints();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/", () => "Hello World!");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("CorsPolicy");
            app.Run();
        }
    }
}