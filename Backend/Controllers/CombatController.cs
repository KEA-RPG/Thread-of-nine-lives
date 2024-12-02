﻿using Backend.Models;
using Backend.Services;
using Domain.DTOs;
using Backend.Extensions;

namespace Backend.Controllers
{
    public static class CombatController
    {

        public static void MapCombatEndpoints(this WebApplication app)
        {
            app.MapPost("/combat/{id}/action", (ICombatService combatService,int id, GameActionDTO action) =>
            {
                action.FightId = id;
                var state = combatService.GetProcessedState(id, action);

                return Results.Ok(state);
            });

            app.MapPost("/combat/initialize", (ICombatService combatService, StateGameInit stateGameInit, HttpContext context) =>
            {
                stateGameInit.UserName = context.GetUserName();
                var state = combatService.GetInitialState(stateGameInit);

                return Results.Ok(state);
            });

            app.MapGet("/combat/{id}", (ICombatService combatService, int id) =>
            {
                var state = combatService.GetStateByFightId(id);

                return Results.Ok(state);
            });
        }
    }
}
