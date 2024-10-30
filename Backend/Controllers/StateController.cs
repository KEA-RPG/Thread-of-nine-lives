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
            app.MapPost("/combat", (GameAction gameAction) =>
            {
                if (gameAction == null || string.IsNullOrEmpty(gameAction.Type))
                {
                    return Results.BadRequest("Invalid action.");
                }

                gameState.ProcessAction(gameAction);

                return Results.Ok(gameState);
            });

            app.MapGet("/game-state", (IEnemyService enemyService, IPlayerService playerService) =>
            {
                if (gameState == null || gameState.EnemyDTO == null || gameState.PlayerDTO == null)
                {
                    var enemyDTO = enemyService.GetEnemyById(2);
                    if (enemyDTO == null)
                    {
                        return Results.NotFound("Enemy not found.");
                    }

                    var playerDTO = playerService.GetPlayerById(1);
                    if (playerDTO == null)
                    {
                        return Results.NotFound("Player not found.");
                    }

                    gameState = new State(playerDTO, enemyDTO);
                }
                return Results.Ok(gameState);
            });
        }
    }
}
