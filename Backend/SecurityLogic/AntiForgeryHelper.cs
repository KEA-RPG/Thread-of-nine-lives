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
            }
            catch (AntiforgeryValidationException ex)
            {
                throw;
            }
        }
    }
}
