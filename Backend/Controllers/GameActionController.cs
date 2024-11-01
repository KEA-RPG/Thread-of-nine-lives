using Backend.Services;
using Domain.DTOs;

namespace Backend.Controllers
{
    public static class GameActionController
    {
        public static void MapGameActionEndpoints(this WebApplication app)
        {
            // Get all game actions
            app.MapGet("/game-actions", (IGameActionService gameActionService) =>
            {
                var gameActionDTOs = gameActionService.GetAllGameActions();
                return gameActionDTOs;
            });

            // Get game action by id
            app.MapGet("/game-actions/{id}", (IGameActionService gameActionService, int id) =>
            {
                var gameActionDTO = gameActionService.GetGameActionById(id);
                if (gameActionDTO == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(gameActionDTO);
            });

            // Create game action
            app.MapPost("/game-actions", (IGameActionService gameActionService, GameActionDTO gameActionDTO) =>
            {
                var createdGameActionDTO = gameActionService.CreateGameAction(gameActionDTO);
                return Results.Created($"/game-actions/{createdGameActionDTO.Id}", createdGameActionDTO);
            });

            // Update game action
            app.MapPut("/game-actions", (IGameActionService gameActionService, GameActionDTO gameActionDTO) =>
            {
                var updatedGameActionDTO = gameActionService.UpdateGameAction(gameActionDTO);
                if (updatedGameActionDTO == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(updatedGameActionDTO);
            });

            // Delete game action
            app.MapDelete("/game-actions/{id}", (IGameActionService gameActionService, int id) =>
            {
                var gameActionDTO = gameActionService.GetGameActionById(id);
                if (gameActionDTO == null)
                {
                    return Results.NotFound();
                }
                gameActionService.DeleteGameAction(id);
                return Results.Ok();
            });
        }
    }
}
