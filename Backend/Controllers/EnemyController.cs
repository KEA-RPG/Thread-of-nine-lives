using Backend.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Backend.Controllers
{
    public static class EnemyController
    {
        public static void MapEnemyEndpoint(this WebApplication app)
        {
            //Get all enemies
            app.MapGet("/enemies", (IEnemyService enemyService) =>
            {
                return enemyService.GetAllEnemies();
            });

            //Get enemy by id
            app.MapGet("/enemies/{id}", (IEnemyService enemyService, int id) =>
            {
                return enemyService.GetEnemyById(id);
            });

            //Delete enemy
            app.MapDelete("/enemies/{id}", (IEnemyService enemyService, int id) =>
            {
                var enemy = new Enemy();
                enemy.Id = id;
                var dbEnemy = enemyService.GetEnemyById(enemy.Id);
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

