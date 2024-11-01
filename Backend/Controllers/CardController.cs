using Backend.Services;
using Domain.DTOs;
using Domain.Entities; //TODO: Change to correct namespace(DTO)

namespace Backend.Controllers
{

    public static class CardController
    {
        public static void MapCardEndpoint(this WebApplication app)
        {
            //Get all cards
            //Tænker det skal være muligt for alle at se eksisterende kort, regardless af roller
            app.MapGet("/cards", (ICardService cardService) =>
            {
                var cards = cardService.GetAllCards();
                return cards;
            });

            //Get card by id
            //Samme årsag som ovenstående
            app.MapGet("/cards/{id}", (ICardService cardService, int id) =>
            {
                var cardDTO = cardService.GetCardById(id);
                return cardDTO;
            });

            //Delete card
            app.MapDelete("/cards/{id}", (ICardService cardService, int id) =>
            {
                var cardDTO = cardService.GetCardById(id);
                if (cardDTO == null)
                {
                    return Results.NotFound();
                }
                
                return cardService.DeleteCard(id);


            }).RequireAuthorization(policy => policy.RequireRole("Admin"));

            //Create card
            app.MapPost("/cards", (ICardService cardService, CardDTO cardDTO) =>
            {
                var createdCardDTO = cardService.CreateCard(cardDTO);
                return Results.Created($"/cards/{cardDTO.Id}", createdCardDTO);
            }).RequireAuthorization(policy => policy.RequireRole("Admin"));

            //Update card
            app.MapPut("/cards", (ICardService cardService, CardDTO cardDTO) => {

                var updateCardDTO = cardService.UpdateCard(cardDTO);

                if (updateCardDTO == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(updateCardDTO);
            }).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
