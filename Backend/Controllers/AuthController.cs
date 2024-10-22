using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Services;
using Domain.Entities;

namespace Backend.Controllers
{

    public static class AuthController
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            // Login Endpoint
            app.MapPost("/auth/login", (IUserService userService, User user) =>
            {
                // Check if user exists and password is correct
                if (userService.ValidateUserCredentials(user.Username, user.PasswordHash))
                {
                    var loggedInUser = userService.GetUserByUsername(user.Username);
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                        new Claim(ClaimTypes.Role, loggedInUser.Role)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UngnjU6otFg8IumrmGgl-MbWUUc9wMk0HR37M-VYs6s="));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "threadgame",
                        audience: "threadgame",
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: creds);

                    return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
                }

                // If user is not valid
                return Results.Unauthorized();
            });

            // Signup (Create User) Endpoint
            app.MapPost("/auth/signup", (IUserService userService, User user) =>
            {
                // Check if user already exists
                var existingUser = userService.GetUserByUsername(user.Username);
                if (existingUser != null)
                {
                    return Results.BadRequest("User already exists");
                }

                // Create new user
                var newUser = new User
                {
                    Username = user.Username,
                    PasswordHash = user.PasswordHash // Password will be hashed in the service
                };

                userService.CreateUser(newUser);

                return Results.Ok("User created successfully");
            });
        }
    }
}
