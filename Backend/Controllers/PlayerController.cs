using Backend.Services;
using Domain.DTOs;

namespace Backend.Controllers
{
    public static class PlayerController
    {
        public static void MapPlayerEndpoints(this WebApplication app)
        {
            // Get all players
            app.MapGet("/players", (IPlayerService playerService) =>
            {
                var playerDTOs = playerService.GetAllPlayers();
                return playerDTOs;
            });//.RequireAuthorization(policy => policy.RequireRole("Admin"));

            // Get player by id
            app.MapGet("/players/{id}", (IPlayerService playerService, int id) =>
            {
                var playerDTO = playerService.GetPlayerById(id);
                if (playerDTO == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(playerDTO);
            });

            // Create player
            app.MapPost("/players", (IPlayerService playerService, PlayerDTO playerDTO) =>
            {
                var createdPlayerDTO = playerService.CreatePlayer(playerDTO);
                return Results.Created($"/players/{createdPlayerDTO.Id}", createdPlayerDTO);
            });

            // Update player
            app.MapPut("/players", (IPlayerService playerService, PlayerDTO playerDTO) =>
            {
                var updatedPlayerDTO = playerService.UpdatePlayer(playerDTO);
                if (updatedPlayerDTO == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(updatedPlayerDTO);
            });

            // Delete player
            app.MapDelete("/players/{id}", (IPlayerService playerService, int id) =>
            {
                var playerDTO = playerService.GetPlayerById(id);
                if (playerDTO == null)
                {
                    return Results.NotFound();
                }
                playerService.DeletePlayer(id);
                return Results.Ok();
            });
        }
    }
}
