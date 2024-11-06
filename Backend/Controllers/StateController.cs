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

            app.MapPost("/init-game-state", (IEnemyService enemyService, IPlayerService playerService, HttpContext httpContext) =>
            {
                // Retrieve enemyId and playerId from the query parameters
                var enemyIdStr = httpContext.Request.Query["enemyId"];
                var playerIdStr = httpContext.Request.Query["playerId"];

                if (!int.TryParse(enemyIdStr, out var enemyId) || !int.TryParse(playerIdStr, out var playerId))
                {
                    return Results.BadRequest("Invalid enemyId or playerId.");
                }

                // Fetch enemy and player using the IDs from the query parameters
                var enemyDTO = enemyService.GetEnemyById(enemyId);
                if (enemyDTO == null)
                {
                    return Results.NotFound("Enemy not found.");
                }

                var playerDTO = playerService.GetPlayerById(playerId);
                if (playerDTO == null)
                {
                    return Results.NotFound("Player not found.");
                }

                // Initialize the game state with the player and enemy data
                gameState = new State(playerDTO, enemyDTO);

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
