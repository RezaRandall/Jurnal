using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace TabpediaFin.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtIssuerOptions _jwtOptions;

    public JwtTokenGenerator(IOptions<JwtIssuerOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<string> CreateToken(AuthenticateResponseDto user)
    {
        var claims = new[]
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim("Username", user.Username),
            new Claim("FullName", user.FullName),
            new Claim("RoleId", user.RoleId.ToString()),
            new Claim("RoleName", user.RoleName),
            new Claim("TenantId", user.TenantId.ToString()),
            new Claim("TenantName", user.TenantName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            new Claim(JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(_jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var jwt = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            _jwtOptions.NotBefore,
            _jwtOptions.Expiration,
            _jwtOptions.SigningCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string GenerateRefreshToken(int size = 32)
    {
        var randomNumber = new byte[size];
        using var numberGenerator = RandomNumberGenerator.Create();
        numberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}



public interface IJwtTokenGenerator
{
    Task<string> CreateToken(AuthenticateResponseDto user);

    string GenerateRefreshToken(int size = 32);
}
