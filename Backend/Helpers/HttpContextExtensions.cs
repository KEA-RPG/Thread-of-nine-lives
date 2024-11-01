using System.Security.Claims;

namespace Backend.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Retrieves the username from the JWT token in the HttpContext.
        /// </summary>
        /// <param name="context">The HttpContext containing the user principal.</param>
        /// <returns>The username if present; otherwise, null.</returns>
        public static string GetUserName(this HttpContext context)
        {
            return context.User?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
        }

        /// <summary>
        /// Retrieves the user role from the JWT token in the HttpContext.
        /// </summary>
        /// <param name="context">The HttpContext containing the user principal.</param>
        /// <returns>The user role if present; otherwise, null.</returns>
        public static string GetUserRole(this HttpContext context)
        {
            return context.User?.FindFirst(ClaimTypes.Role)?.Value;
        }

        // Add more methods to retrieve other claims as needed
    }
}