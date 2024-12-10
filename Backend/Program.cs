using Backend;
using Backend.Controllers;
using Backend.Repositories.Document;
using Backend.Repositories.Graph;
using Backend.Repositories.Interfaces;
using Backend.Repositories.Relational;
using Backend.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Graph;
using Infrastructure.Persistance.Relational;
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
            builder.Services.AddScoped<ICardRepository, GraphCardRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, GraphUserRepository>();
            builder.Services.AddScoped<IDeckRepository, GraphDeckRepository>();
            builder.Services.AddScoped<IDeckService, DeckService>();
            builder.Services.AddScoped<ICombatRepository, GraphCombatRepository>();
            builder.Services.AddScoped<ICombatService, CombatService>();
            builder.Services.AddScoped<IEnemyService, EnemyService>();
            builder.Services.AddScoped<IEnemyRepository, GraphEnemyRepository>();
            builder.Services.AddMemoryCache(); // Bruger vi til in-memory caching for blacklisting tokens


            builder.Services.AddCors(p => p.AddPolicy("*", b =>
            
            b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));


            PersistanceConfiguration.ConfigureServices(builder.Services, dbtype.DefaultConnection);


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
            app.UseCors("*");
            app.Run();
        }
    }
}