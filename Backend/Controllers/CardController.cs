using Backend.Services;

namespace Backend.Controllers
{
    public static class CardController
    {
        public static void MapCardEndpoint(this WebApplication app)
        {
            app.MapGet("/cards", (ICardService cardService) =>
            {
                return cardService.GetAllCards();
            });




        }
    }
}
