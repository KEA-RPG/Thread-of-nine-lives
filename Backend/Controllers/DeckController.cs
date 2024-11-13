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

            app.MapGet("/decks/{id}", (IDeckService deckService, IUserService userService, int id, HttpContext context) =>
            {
                var dbDeck = deckService.GetDeckById(id);
                var userName = context.GetUserName();
                var role = context.GetUserRole();

                if (dbDeck == null)
                {
                    return Results.NotFound();
                }

                else if (role == "Admin")//Check if the user is an admin
                {
                    return Results.Ok(dbDeck);
                }

                else if (role == "Player" && dbDeck.UserId == userService.GetUserIdByUserName(userName))//Check if the user is the owner of the deck
                {
                    return Results.Ok(dbDeck);
                }

                return Results.Unauthorized();
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
                var createdDeck = deckService.CreateDeck(deck);
                return Results.Created($"/decks/{createdDeck.Id}", createdDeck);

            }).RequireAuthorization(policy => policy.RequireRole("Player", "Admin"));//I tilfælde der skal laves offentlige free decks

            //Update deck
            app.MapPut("/decks/{id}", (IDeckService deckService, IUserService userService, HttpContext context, DeckDTO deck, int id) =>
            {
                var dbDeck = deckService.GetDeckById(id);
                var userName = context.GetUserName();
                var role = context.GetUserRole();
                if (dbDeck == null)
                {
                    return Results.NotFound();
                }
                deck.Id = id;
                if (role == "Admin")
                {   //Check if the user is an admin
                    
                    deckService.UpdateDeck(deck);
                    return Results.Ok(deck);
                }
                else if (role == "Player" && dbDeck.UserId == userService.GetUserIdByUserName(userName))
                {   //Check if the user is the owner of the deck

                    deckService.UpdateDeck(deck);
                    return Results.Ok(deck);
                }
                return Results.Unauthorized();
            }).RequireAuthorization(policy => policy.RequireRole("Player", "Admin"));//I tilfælde der skal opdateres offentlige free decks


            // Add a comment to a deck
            app.MapPost("/decks/{deckId}/comments", (IDeckService deckService, int deckId, CommentDTO commentDto) =>
            {
                commentDto.DeckId = deckId;
                deckService.AddComment(commentDto);
                return Results.Created($"/decks/{deckId}/comments/{commentDto.Id}", commentDto);
            }).RequireAuthorization(policy => policy.RequireRole("Player", "Admin"));


            // Get all comments for a specific deck
            app.MapGet("/decks/{deckId}/comments", (IDeckService deckService, int deckId) =>
            {
                return Results.Ok(deckService.GetCommentsByDeckId(deckId));
            });

        }
    }
}
