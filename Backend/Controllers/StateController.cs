using Backend.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public static class StateController
    {
        private static State gameState = new State(new Player { Health = 30 }, new Enemy { Name = "Sir Meowsalot", Health = 15 });

        public static void MapStateEndpoints(this WebApplication app)
        {
            app.MapPost("/combat", (Services.Action action) =>
            {
                if (action == null || string.IsNullOrEmpty(action.Type))
                {
                    return Results.BadRequest("Invalid action.");
                }

                gameState.ProcessAction(action);

                return Results.Ok(new { enemy = new { health = gameState.Enemy.Health } });
            });

            app.MapGet("/game-state", () =>
            {
                return Results.Ok(new { enemy = new { name = gameState.Enemy.Name, health = gameState.Enemy.Health } });
            });
        }
    }
}
