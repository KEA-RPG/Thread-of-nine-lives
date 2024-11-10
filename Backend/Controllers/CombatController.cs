using Backend.Models;
using Backend.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;

namespace Backend.Controllers
{
    public static class CombatController
    {
        private static CombatService gameState;

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

            app.MapPost("/init-game-state", (IEnemyService enemyService, IPlayerService playerService, StateGameInit stateGameInit) =>
            {

                // Fetch enemy and player using the IDs from the query parameters
                var enemyDTO = enemyService.GetEnemyById(stateGameInit.EnemyId);
                if (enemyDTO == null)
                {
                    return Results.NotFound("Enemy not found.");
                }

                var playerDTO = playerService.GetPlayerById(stateGameInit.PlayerId);
                if (playerDTO == null)
                {
                    return Results.NotFound("Player not found.");
                }

                // Initialize the game state with the player and enemy data
                gameState = new StateService(playerDTO, enemyDTO);

                return Results.Ok(gameState);
            });

            app.MapGet("/game-state", (IEnemyService enemyService, IPlayerService playerService) =>
            {
                if (gameState == null)
                {
                    return Results.NotFound("Game state has not been initialized.");
                }

                return Results.Ok(gameState);
            });
        }
    }
}
