using Backend.Services;
using Domain.Entities;

namespace Backend.Controllers
{
    public static class FightController
    {
        public static void MapCardEndpoint(this WebApplication app)
        {
            //Get all fights
            app.MapGet("/fights", (IFightService fightService) =>
            {
                return fightService.GetAllFights();
            });

            //Get fight by id
            app.MapGet("/fights/{id}", (IFightService fightService, int id) =>
            {
                return fightService.GetFightById(id);
            });

            //Delete fight
            app.MapDelete("/fights/{id}", (IFightService fightService, int id) =>
            {
                var dbFight = fightService.GetFightById(id);
                if (dbFight == null)
                {
                    return Results.NotFound();
                }
                fightService.DeleteFight(id);
                return Results.Ok();
            });

            //Create fight
            app.MapPost("/fights", (IFightService fightService, Fight fight) =>
            {
                fightService.CreateFight(fight);
                return Results.Created($"/fights/{fight.Id}", fight);
            });

            //Update fight
            app.MapPut("/fights", (IFightService fightService, Fight fight) => {

                var dbFight = fightService.GetFightById(fight.Id);

                if (dbFight == null)
                {
                    return Results.NotFound();
                }

                fightService.UpdateFight(fight);
                return Results.Ok(fight);
            });

        }
    }
}
