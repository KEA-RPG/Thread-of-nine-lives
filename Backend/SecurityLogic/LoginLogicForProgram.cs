using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend;

public static class LoginLogicForProgram
{
    public static void AddJwtAuthentication(this IServiceCollection services, string issuer, string audience, string signingKey)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero  // Set clock skew to zero for precise validation
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

                        // Access the claims directly from context.Principal
                        var jti = context.Principal?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                        Debug.WriteLine($"Checking cache for jti during validation: {jti}");

                        if (jti != null && memoryCache.TryGetValue(jti, out _))
                        {
                            context.Fail("This token has been revoked.");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }
}
