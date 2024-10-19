using Backend.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Diagnostics;

namespace Backend.Controllers
{
    public static class EnemyController
    {
        public static void MapEnemyEndpoint(this WebApplication app)
        {
            //Get all enemies
            app.MapGet("/enemies", (IEnemyService enemyService, HttpContext context) =>
            {
                // Log all claims available in the current context
                Debug.WriteLine("Claims available for the current user:");
                var claims = context.User.Claims;
                foreach (var claim in claims)
                {
                    Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }

                if (!context.User.IsInRole("Admin"))
                {
                    Debug.WriteLine("User does not have Admin role.");
                    return Results.Forbid();
                }

                return Results.Ok(enemyService.GetAllEnemies());
            }).RequireAuthorization(policy => policy.RequireRole("Admin"));

            //Get enemy by id
            app.MapGet("/enemies/{id}", (IEnemyService enemyService, int id) =>
            {
                return enemyService.GetEnemyById(id);
            });

            //Delete enemy
            app.MapDelete("/enemies/{id}", (IEnemyService enemyService, int id) =>
            {
                var dbEnemy = enemyService.GetEnemyById(id);
                if (dbEnemy == null)
                {
                    return Results.NotFound();
                }
                enemyService.DeleteEnemy(id);
                return Results.Ok();
            });

            //Create enemy
            app.MapPost("/enemies", (IEnemyService enemyService, Enemy enemy) =>
            {
                enemyService.CreateEnemy(enemy);
                return Results.Created($"/enemies/{enemy.Id}", enemy);
            });

            //Update enemy
            app.MapPut("/enemies", (IEnemyService enemyService, Enemy enemy) => {

                var dbEnemy = enemyService.GetEnemyById(enemy.Id);

                if (dbEnemy == null)
                {
                    return Results.NotFound();
                }

                enemyService.UpdateEnemy(enemy);
                return Results.Ok(enemy);
            });
        }
    }
}

