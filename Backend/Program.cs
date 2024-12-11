using Backend.Controllers;
using Backend.Helpers;
using Backend.Repositories.Document;
using Backend.Repositories.Interfaces;
using Backend.Repositories.Relational;
using Backend.Services;
using Infrastructure.Persistance;
using Microsoft.OpenApi.Models;

namespace Backend
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddHealthChecks();

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


            CorsHelper.AddCorsPolicy(builder.Services, builder.Configuration);


            var hostingEnvironment = builder.Environment.EnvironmentName;
            PersistanceConfiguration.ConfigureServices(builder.Services, dbtype.DefaultConnection, hostingEnvironment);

            //builder.Services.AddAntiforgery(options =>
            //{
            //    // Customize the settings if needed (e.g., SameSite policy, cookie options)
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Ensures cookie is sent over HTTPS
            //    options.Cookie.HttpOnly = true;  // Prevents JavaScript access to the cookie
            //    options.Cookie.SameSite = SameSiteMode.Unspecified;  // Protects against CSRF attacks
            //    options.Cookie.Expiration = TimeSpan.FromMinutes(60);
            //    options.Cookie.Path = "/";
            //});


            var app = builder.Build();


            // Map controllers
            app.MapCardEndpoint();
            app.MapAuthEndpoints();
            app.MapEnemyEndpoint();
            app.MapDeckEndpoint();
            app.MapHealthChecks("/health");

            app.MapCombatEndpoints();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/", () => "Hello World!");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowAll");
            app.Run();
        }
    }
}