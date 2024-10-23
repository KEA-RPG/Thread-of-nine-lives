using Backend.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Backend.Controllers
{
    public static class StateController
    {
        private static State gameState = new State(new Player { Health = 30 }, new Enemy { Name = "Sir Meowsalot", Health = 35 });

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

            app.MapGet("/game-state", () =>
            {
                return Results.Ok(gameState);
            });

            
        }
    }
}
