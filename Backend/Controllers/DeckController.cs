using Backend.Extensions;
using Backend.SecurityLogic;
using Backend.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Antiforgery;
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
            // Add a comment to a deck
            app.MapPost("/decks/{deckId}/comments", async (IDeckService deckService, int deckId, CommentDTO commentDto, HttpContext context) =>
            {
                System.Diagnostics.Debug.WriteLine("Attempting to validate anti-forgery token for comment.");
                System.Diagnostics.Debug.WriteLine("Incoming request cookies:");

                foreach (var cookie in context.Request.Cookies)
                {
                    System.Diagnostics.Debug.WriteLine($"Cookie Name: {cookie.Key}, Cookie Value: {cookie.Value}");
                }

                try
                {
                    await AntiForgeryHelper.ValidateAntiForgeryToken(context);
                    System.Diagnostics.Debug.WriteLine("Anti-forgery token validated successfully.");
                }
                catch (AntiforgeryValidationException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Anti-forgery token validation failed: {ex.Message}");
                    throw;
                }

                commentDto.DeckId = deckId;
                deckService.AddComment(commentDto);
                System.Diagnostics.Debug.WriteLine($"Comment added successfully to deck {deckId}.");
                return Results.Created($"/decks/{deckId}/comments/{commentDto.Id}", commentDto);
            }).RequireAuthorization(policy => policy.RequireRole("Player", "Admin"));



        }
    }
}
