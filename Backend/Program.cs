using Backend.Controllers;
using Backend.Repositories;
using Backend.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICardRepository, CardRepository>();

builder.Services.AddDbContext<RelationalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")));

builder.Services.AddScoped<IEnemyService, EnemyService>();
builder.Services.AddScoped<IEnemyRepository, EnemyRepository>();

//PersistanceConfiguration.ConfigureServices(builder.Services, builder.Configuration, "relational");
var app = builder.Build();

CardController.MapCardEndpoint(app);


app.MapGet("/", () => "Hello World!");
EnemyController.MapEnemyEndpoint(app);
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
