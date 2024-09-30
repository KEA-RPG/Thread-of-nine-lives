using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [ApiController]
    [Route("cards")]
    [Authorize] // Require authentication for all actions in this controller
    public class CardController
    {
        private readonly ICardService _cardService;
        private readonly ILogger<CardController> _logger;

        public CardController(ICardService cardService, ILogger<CardController> logger)
        {
            _cardService = cardService;
            _logger = logger;
        }

        // Get all cards
        [HttpGet]
        public object GetAllCards()
        {
            _logger.LogInformation("Fetching all cards");
            var cards = _cardService.GetAllCards();
            return Results.Json(cards, statusCode: 200); // Return 200 OK
        }

        // Get card by id
        [HttpGet("{id}")]
        public object GetCardById(int id)
        {
            var card = _cardService.GetCardById(id);
            if (card == null)
            {
                return Results.Json(new { message = "Card not found" }, statusCode: 404); // Return 404 Not Found
            }
            return Results.Json(card, statusCode: 200); // Return 200 OK
        }

        // Delete card
        [HttpDelete("{id}")]
        public object DeleteCard(int id)
        {
            _cardService.DeleteCard(id);
            return Results.Json(new { message = "Card deleted successfully" }, statusCode: 204); // Return 204 No Content
        }

        // Create card
        [HttpPost]
        public object CreateCard([FromBody] Card card)
        {
            _cardService.CreateCard(card);
            return Results.Json(card, statusCode: 201); // Return 201 Created
        }

        // Update card
        [HttpPut]
        public object UpdateCard([FromBody] Card card)
        {
            var existingCard = _cardService.GetCardById(card.Id);
            if (existingCard == null)
            {
                return Results.Json(new { message = "Card not found" }, statusCode: 404); // Return 404 Not Found
            }

            _cardService.UpdateCard(card);
            return Results.Json(card, statusCode: 200); // Return 200 OK
        }
    }
}
