using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Backend.SecurityLogic
{
    public static class AntiForgeryHelper
    {
        public static async Task ValidateAntiForgeryToken(HttpContext context)
        {
            var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

            try
            {
                await antiforgery.ValidateRequestAsync(context);
                System.Diagnostics.Debug.WriteLine("CSRF token validated successfully.");
            }
            catch (AntiforgeryValidationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"CSRF validation failed: {ex.Message}");
                throw; // Re-throw to allow higher-level error handling
            }
        }
    }
}
