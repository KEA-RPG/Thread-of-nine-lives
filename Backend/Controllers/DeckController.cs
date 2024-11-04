using Backend.Extensions;
using Backend.Services;
using Domain.DTOs;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace Backend.Controllers
{
    public static class DeckController
    {

        public static void MapDeckEndpoint(this WebApplication app)
        {

            //Get all public decks
            app.MapGet("/decks/public", (IDeckService deckService) =>
            {
                return Results.Ok(deckService.GetPublicDecks());
            });

            //Get all user decks
            app.MapGet("/decks", (IDeckService deckService, HttpContext context) =>
            {
                string userName = context.GetUserName();
                if (userName == null)
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(deckService.GetUserDecks(userName));

            });

            //Delete deck
            app.MapDelete("/decks/{id}", (IDeckService deckService, int id) =>
            {
                var dbDeck = deckService.GetDeckById(id);
                if (dbDeck == null)
                {
                    return Results.NotFound();
                }

                try
                {
                    deckService.DeleteDeck(id);
                    return Results.NoContent();
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

            }).RequireAuthorization(policy => policy.RequireRole("Player", "Admin")); //I tilfælde af at der skal kunnes slette offentlige decks

            //Create deck
            app.MapPost("/decks", (IDeckService deckService, IUserService userService, DeckDTO deck, HttpContext context) =>
            {

                var userName = context.GetUserName();
                if (userName == null)
                {
                    return Results.Unauthorized();
                }

                int userID = userService.GetUserIdByUserName(userName);
                deck.UserId = userID;
                deckService.CreateDeck(deck);
                return Results.Created($"/decks/{deck.Id}", deck);

            }).RequireAuthorization(policy => policy.RequireRole("Player", "Admin"));//I tilfælde der skal laves offentlige free decks

            //Update deck
            app.MapPut("/decks", (IDeckService deckService, HttpContext context, DeckDTO deck) =>
            {
                var dbDeck = deckService.GetDeckById(deck.Id);
                var userName = context.GetUserName();
                var role = context.GetUserRole();
                if (dbDeck == null)
                {
                    return Results.NotFound();
                }

                if (role == "Admin")
                {   //Check if the user is an admin

                    deckService.UpdateDeck(deck);
                    return Results.Ok(deck);


                }
                else if (role == "Player" && dbDeck.UserId == deck.UserId)
                {   //Check if the user is the owner of the deck

                    deckService.UpdateDeck(deck);
                    return Results.Ok(deck);
                }
                return Results.Unauthorized();
            }).RequireAuthorization(policy => policy.RequireRole("Player", "Admin"));//I tilfælde der skal opdateres offentlige free decks
        }
    }
}
