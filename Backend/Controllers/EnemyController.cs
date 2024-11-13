using Backend.Services;
using Domain.DTOs;

namespace Backend.Controllers
{
    public static class EnemyController
    {
        public static void MapEnemyEndpoint(this WebApplication app)
        {
            // Get all enemies
            app.MapGet("/enemies", (IEnemyService enemyService) =>
            {
                var enemyDTOs = enemyService.GetAllEnemies();
                return enemyDTOs;
            }).RequireAuthorization(policy => policy.RequireRole("Player"));

            // Get enemy by id
            app.MapGet("/enemies/{id}", (IEnemyService enemyService, int id) =>
            {
                var enemyDTO = enemyService.GetEnemyById(id);
                if (enemyDTO == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(enemyDTO);
            });

            // Create enemy
            app.MapPost("/enemies", (IEnemyService enemyService, EnemyDTO enemyDTO) =>
            {
                var createdEnemyDTO = enemyService.CreateEnemy(enemyDTO);
                return Results.Created($"/enemies/{createdEnemyDTO.Id}", createdEnemyDTO);
            });

            // Put enemy
            app.MapPut("/enemies", (IEnemyService enemyService, EnemyDTO enemyDTO) =>
            {
                var updatedEnemyDTO = enemyService.UpdateEnemy(enemyDTO);
                if (updatedEnemyDTO == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(updatedEnemyDTO);
            });

            // Delete enemy
            app.MapDelete("/enemies/{id}", (IEnemyService enemyService, int id) =>
            {
                var enemyDTO = enemyService.GetEnemyById(id);
                if (enemyDTO == null)
                {
                    return Results.NotFound();
                }
                enemyService.DeleteEnemy(id);
                return Results.Ok();
            }).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
