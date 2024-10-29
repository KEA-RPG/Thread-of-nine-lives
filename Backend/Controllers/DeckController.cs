using Backend.Services;
using Domain.DTOs;

namespace Backend.Controllers
{
    public static class DeckController
    {
        public static void MapDeckEndpoint(this WebApplication app)
        {
            //Get all decks
            app.MapGet("/decks", (IDeckService deckService) => //TODO need to take user id from JWT token
            {
                return deckService.GetUserDecks(/*userId*/);
            });

            //Get deck by id
            //Samme årsag som ovenover
            app.MapGet("/decks/{id}", (IDeckService deckService, int id) =>
            {
                var dbDeck = deckService.GetDeckById(id);
                if (dbDeck?.IsPublic == true)
                {
                    return Results.Ok(dbDeck);
                }

                return Results.BadRequest("Deck is private");
            });


            //Delete deck
            app.MapDelete("/decks/{id}", (IDeckService deckService, int id) =>
            {
                var dbDeck = deckService.GetDeckById(id);
                if (dbDeck == null)
                {
                    return Results.NotFound();
                }
                deckService.DeleteDeck(id);
                return Results.Ok();
            }).RequireAuthorization(policy => policy.RequireRole("User", "Admin")); //I tilfælde af at der skal kunnes slette offentlige decks

            //Create deck
            app.MapPost("/decks", (IDeckService deckService, DeckDTO deck) =>
            {
                deckService.CreateDeck(deck);
                return Results.Created($"/decks/{deck.Id}", deck);
            }).RequireAuthorization(policy => policy.RequireRole("User", "Admin"));//I tilfælde der skal laves offentlige free decks

            //Update deck
            app.MapPut("/decks", (IDeckService deckService, DeckDTO deck) =>
            {

                var dbDeck = deckService.GetDeckById(deck.Id);

                if (dbDeck == null)
                {
                    return Results.NotFound();
                }

                deckService.UpdateDeck(deck);
                return Results.Ok(deck);
            }).RequireAuthorization(policy => policy.RequireRole("User", "Admin"));//I tilfælde der skal opdateres offentlige free decks
        }
    }
}
