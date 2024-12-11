using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Helpers
{
    public static class CorsHelper
    {
        public static void AddCorsPolicy(IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    if (corsSettings?.AllowedOrigins != null && corsSettings.AllowedOrigins.Any())
                    {
                        builder.WithOrigins(corsSettings.AllowedOrigins.ToArray());
                    }
                    else
                    {
                        builder.AllowAnyOrigin();
                    }

                    
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
        }
    }

    public class CorsSettings
    {
        public List<string> AllowedOrigins { get; set; }
    }
}
