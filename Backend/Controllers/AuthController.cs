using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // Login Endpoint
        [HttpPost("login")]
        [AllowAnonymous] // No need to be authenticated for login
        public object Login([FromBody] User user)
        {
            if (_userService.ValidateUserCredentials(user.Username, user.PasswordHash))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A7c$7DFG9!fQ2@Vbn#4gTxlpT67^n8#QhE"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "threadgame",
                    audience: "threadgame",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Json(new { token = tokenString }, statusCode: 200); // Manually return JSON with 200 OK
            }

            return Results.Json(new { message = "Unauthorized" }, statusCode: 401); // Manually return 401 Unauthorized
        }

        // Signup (Create User) Endpoint
        [HttpPost("signup")]
        [AllowAnonymous] // No need to be authenticated for signup
        public object Signup([FromBody] User user)
        {
            var existingUser = _userService.GetUserByUsername(user.Username);
            if (existingUser != null)
            {
                return Results.Json(new { message = "User already exists" }, statusCode: 400); // Return 400 Bad Request
            }

            var newUser = new User
            {
                Username = user.Username,
                PasswordHash = user.PasswordHash // Password will be hashed in the service
            };

            _userService.CreateUser(newUser);

            return Results.Json(new { message = "User created successfully" }, statusCode: 201); // Return 201 Created
        }
    }
}
