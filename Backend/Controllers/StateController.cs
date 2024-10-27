using Backend.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;

namespace Backend.Controllers
{
    public static class StateController
    {
        private static State gameState;

        public static void MapStateEndpoints(this WebApplication app)
        {
            app.MapPost("/combat", (Services.Action action) =>
            {
                if (action == null || string.IsNullOrEmpty(action.Type))
                {
                    return Results.BadRequest("Invalid action.");
                }

                gameState.ProcessAction(action);

                return Results.Ok(gameState);
            });

            app.MapGet("/game-state", (IEnemyService enemyService) =>
            {
                if (gameState == null || gameState.EnemyDTO == null)
                {
                    var enemyDTO = enemyService.GetEnemyById(2);
                    if (enemyDTO == null)
                    {
                        return Results.NotFound("Enemy not found.");
                    }

                    gameState = new State(new Player { Health = 30 }, enemyDTO);
                }
                return Results.Ok(gameState);
            });

            

        }
    }
}
