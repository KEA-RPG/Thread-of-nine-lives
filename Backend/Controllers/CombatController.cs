using Backend.Models;
using Backend.Services;
using Domain.DTOs;
using Backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;

namespace Backend.Controllers
{
    public static class CombatController
    {

        public static void MapCombatEndpoints(this WebApplication app)
        {
            app.MapPost("/combat/{id}/action", (ICombatService combatService, GameActionDTO gameAction, State state) =>
            {
                if (gameAction == null || string.IsNullOrEmpty(gameAction.Type))
                {
                    return Results.BadRequest("Invalid action.");
                }
                var updatedState = combatService.ProcessAction(gameAction, state);

                return Results.Ok(updatedState);
            });

            app.MapPost("/combat/initialize", (ICombatService combatService, StateGameInit stateGameInit) =>
            {
                var state = combatService.GetInitState(stateGameInit);

                return Results.Ok(state);
            });

        }
    }
}
