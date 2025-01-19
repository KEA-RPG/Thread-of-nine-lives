using Backend.Controllers;
using Backend.Repositories.Document;
using Backend.Repositories.Graph;
using Backend.Repositories.Interfaces;
using Backend.Repositories.Relational;
using Backend.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Graph;
using Infrastructure.Persistance.Relational;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Neo4j.Driver;
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
            builder.Services.AddHealthChecks();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
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
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDeckService, DeckService>();
            builder.Services.AddScoped<ICombatService, CombatService>();
            builder.Services.AddScoped<IEnemyService, EnemyService>();


            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<ICombatRepository, CombatRepository>();
            builder.Services.AddScoped<IDeckRepository, DeckRepository>();
            builder.Services.AddScoped<IEnemyRepository, EnemyRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            builder.Services.AddMemoryCache();

 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendClient", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173", "https://hoppscotch.io")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

   
            var hostingEnvironment = builder.Environment.EnvironmentName;
            PersistanceConfiguration.ConfigureServices(
                builder.Services,
                dbtype.DefaultConnection,
                hostingEnvironment
            );
            /*
            builder.Services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-COOKIE";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                // SameSite depends on your cross-site usage
                // options.Cookie.SameSite = SameSiteMode.Lax; 
                options.FormFieldName = "__RequestVerificationToken";
            });
            */
            var app = builder.Build();


            app.UseCors("FrontendClient");

            app.UseAuthentication();
            app.UseAuthorization();
            
            /*
            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value ?? string.Empty;
                var method = context.Request.Method;

                // Check if it's POST/PUT/DELETE
                var isStateChanging = (HttpMethods.IsPost(method)
                                       || HttpMethods.IsPut(method)
                                       || HttpMethods.IsGet(method)
                                       || HttpMethods.IsDelete(method));

                // Exclude /auth/login & /auth/signup
                var isExcluded = path.StartsWith("/auth/login", StringComparison.OrdinalIgnoreCase)
                                 || path.StartsWith("/auth/signup", StringComparison.OrdinalIgnoreCase);

                if (isStateChanging && !isExcluded)
                {
                    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
                    try
                    {
                        await antiforgery.ValidateRequestAsync(context);
                    }
                    catch (AntiforgeryValidationException)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("CSRF validation failed.");
                        return; // stop
                    }
                }

                // Otherwise continue
                await next();
            });
            */


            app.MapCardEndpoint();
            app.MapAuthEndpoints();
            app.MapEnemyEndpoint();
            app.MapDeckEndpoint();
            app.MapCombatEndpoints();
            app.MapHealthChecks("/health");
            app.MapGet("/", () => "Hello World!");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.Run();
        }
    }
}
