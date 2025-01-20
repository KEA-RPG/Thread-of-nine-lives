using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Services;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using Backend.Models;
using Microsoft.AspNetCore.Antiforgery;

namespace Backend.Controllers
{
    public static class AuthController
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            // Login Endpoint
            app.MapPost("/auth/login", (IUserService userService, HttpContext context, IAntiforgery antiforgery, Credentials credentials) =>
            {
                // Check if user exists and password is correct
                if (userService.ValidateUserCredentials(credentials.Username, credentials.Password))
                {
                    var loggedInUser = userService.GetUserByUsername(credentials.Username);

                    // Prepare JWT claims
                    var claims = new[]
                    {
                        new Claim(JwtClaimNames.Sub, loggedInUser.Username),
                        new Claim("role", loggedInUser.Role),
                        new Claim(ClaimTypes.Role, loggedInUser.Role),
                        new Claim(JwtClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    // Create JWT token
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UngnjU6otFg8IumrmGgl-MbWUUc9wMk0HR37M-VYs6s="));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(1),
                        Issuer = "threadgame",
                        Audience = "threadgame",
                        SigningCredentials = creds
                    };

                    var jwtHandler = new JsonWebTokenHandler();
                    var token = jwtHandler.CreateToken(tokenDescriptor);

                    // Generate & store CSRF tokens (this automatically sets a HttpOnly antiforgery cookie)
                    antiforgery.GetAndStoreTokens(context);

                    // Return only the JWT in JSON
                    return Results.Ok(new
                    {
                        Token = token
                    });
                }
                // If user is not valid
                return Results.BadRequest("Invalid username or password.");
            });

            // Signup (Create User) Endpoint
            app.MapPost("/auth/signup", (IUserService userService, Credentials credentials) =>
            {

                var existingUser = userService.GetUserByUsername(credentials.Username);
                if (existingUser != null)
                {
                    return Results.BadRequest("User already exists.");
                }

                try
                {
                    userService.CreateUser(credentials);
                    return Results.Ok("User created successfully.");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // Logout Endpoint
            app.MapPost("/auth/logout", (IMemoryCache memoryCache, HttpContext context) =>
            {


                var authorizationHeader = context.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Results.BadRequest("No valid token provided.");
                }

                var token = authorizationHeader.Replace("Bearer ", "");

                var jwtHandler = new JsonWebTokenHandler();
                JsonWebToken jwtToken = null;

                try
                {
                    jwtToken = jwtHandler.ReadJsonWebToken(token) as JsonWebToken;
                }
                catch (ArgumentException)
                {
                    return Results.BadRequest("Invalid token.");
                }

                var jti = jwtToken?.GetPayloadValue<string>(JwtClaimNames.Jti);
                var expiration = jwtToken?.ValidTo;

                if (jti != null && expiration != null)
                {
                    // Check if the token is already in the cache
                    if (memoryCache.TryGetValue(jti, out _))
                    {
                        return Results.BadRequest("Token is already logged out.");
                    }

                    // Blacklist the JTI until it expires
                    memoryCache.Set(jti, true, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = expiration
                    });

                    return Results.Ok("Logged out successfully.");
                }

                return Results.BadRequest("Token is invalid or has no jti.");
            });
        }
    }
}
