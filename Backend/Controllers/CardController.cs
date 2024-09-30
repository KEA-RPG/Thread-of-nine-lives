using Backend.Services;
using Domain.Entities; //TODO: Change to correct namespace(DTO)

namespace Backend.Controllers
{
    public static class CardController
    {
        public static void MapCardEndpoint(this WebApplication app)
        {
            //Get all cards
            app.MapGet("/cards", (ICardService cardService) =>
            {
                return cardService.GetAllCards();
            });

            //Get card by id
            app.MapGet("/cards/{id}", (ICardService cardService, int id) =>
            {
                return cardService.GetCardById(id);
            });

            //Delete card
            app.MapDelete("/cards/{id}", (ICardService cardService, int id) =>
            {
                try
                {
                    cardService.DeleteCard(id);
                    return Results.NoContent();
                }
                catch(KeyNotFoundException)
                {
                    return Results.NotFound();
                }
                catch 
                {
                    return Results.StatusCode(500);
                }
            });

            //Create card
            app.MapPost("/cards", (ICardService cardService, Card card) =>
            {
                cardService.CreateCard(card);
                return Results.Created($"/cards/{card.Id}", card);
            });

            //Update card
            app.MapPut("/cards", (ICardService cardService, Card card) => {

                var dbCard = cardService.GetCardById(card.Id);

                if (dbCard == null)
                {
                    return Results.NotFound();
                }

                cardService.UpdateCard(card);
                return Results.Ok(card);
            });





        }
    }
}
