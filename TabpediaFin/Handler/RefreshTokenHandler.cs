using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TabpediaFin.Handler;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequestDto, AuthenticateResponseDto>
{
    private IOptions<JwtSettings> _jwtSettings;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IAuthenticateRepository _repository;

    public RefreshTokenHandler(IOptions<JwtSettings> jwtSettings
        , IJwtTokenGenerator jwtTokenGenerator
        , IAuthenticateRepository repository)
    {
        _jwtSettings = jwtSettings;
        _jwtTokenGenerator = jwtTokenGenerator;
        _repository = repository;
    }

    public async Task<AuthenticateResponseDto> Handle(RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        var username = GetUsernameFromExpiredToken(request.Token).Value;
        if (string.IsNullOrEmpty(username))
        {
            throw new HttpException(HttpStatusCode.Unauthorized, new { Error = "Invalid credentials." });
        }

        var user = await _repository.FetchUserAsync(username, request.AppCode);
        if (user == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, new { Error = "Invalid credentials." });
        }

        var isValidRefreshToken = await _repository.IsValidRefreshToken(user.UserId, request.RefreshToken);
        if (!isValidRefreshToken)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, new { Error = "Invalid credentials." });
        }

        var token = await _jwtTokenGenerator.CreateToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        if (await _repository.UpsertRefreshTokenAsync(user.UserId, refreshToken))
        {
            user.AccessToken = token;
            user.RefreshToken = refreshToken;

            return user;
        }

        throw new HttpException(HttpStatusCode.Unauthorized, new { Error = "Invalid credentials." });
    }


    private Claim GetUsernameFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _jwtSettings.Value.SigningCredentials.Key,
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Value.Audience,
            ValidateLifetime = false, // do not check for expiry date time
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out var securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Contains(
                SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
    }

}
