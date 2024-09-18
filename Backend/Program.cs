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

//PersistanceConfiguration.ConfigureServices(builder.Services, builder.Configuration, "relational");
var app = builder.Build();

CardController.MapCardEndpoint(app);


app.MapGet("/", () => "Hello World!");
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
