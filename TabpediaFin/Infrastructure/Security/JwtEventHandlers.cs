using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TabpediaFin.Infrastructure.Security;

public class JwtEventHandlers : JwtBearerEvents
{
    public JwtEventHandlers()
    {
    }

    //public override async Task TokenValidated(TokenValidatedContext context)
    //{
    //    var accessToken = context.SecurityToken as JwtSecurityToken;
    //    if (accessToken != null)
    //    {
    //        ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;
    //        if (identity != null)
    //        {
    //            var usernameClaim = identity.Claims.FirstOrDefault(x => x.Type.Equals("Username"));
    //            var appIdClaim = identity.Claims.FirstOrDefault(x => x.Type.Equals("AppId"));

    //            if (usernameClaim != null && appIdClaim != null)
    //            {
    //                var username = usernameClaim.Value;
    //                var appIdStr = appIdClaim.Value;

    //                if (int.TryParse(appIdStr, out int appId))
    //                {
    //                    if (!string.IsNullOrWhiteSpace(username) && appId != 0)
    //                    {
    //                        var cacheKey = $"{username}_{appId}";
    //                        var savedToken = await _redisTokenService.GetTokenAsync(username, (AppTypeId)appId, TokenTypeKey.ACCESS_TOKEN);
    //                        if (string.IsNullOrWhiteSpace(savedToken))
    //                        {
    //                            throw new SecurityTokenExpiredException();
    //                        }
    //                        else
    //                        {
    //                            var rawToken = accessToken.RawData;
    //                            if (rawToken.Equals(savedToken))
    //                            {
    //                                //context.Success();
    //                                return;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    // throw new SecurityTokenValidationException("TokenValidatedHandler: Token validation failed.");
    //    context.Fail("TokenValidatedHandler: Token validation failed.");
    //}


    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
        {
            context.Response.Headers.Add("Token-Expired", "true");
        }

        return Task.CompletedTask;
    }
}
