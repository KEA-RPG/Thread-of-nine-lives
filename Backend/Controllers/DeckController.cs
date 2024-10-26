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
                int userId = 1;
                return deckService.GetUserDecks(/*userId*/);
            });

            //Get deck by id
            app.MapGet("/decks/{id}", (IDeckService deckService, int id) =>
            {
                return deckService.GetDeckById(id);
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
            });

            //Create deck
            app.MapPost("/decks", (IDeckService deckService, DeckDTO deck) =>
            {
                deckService.CreateDeck(deck);
                return Results.Created($"/decks/{deck.Id}", deck);
            });

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
            });
        }
    }
}
