using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TabpediaFin.Infrastructure.Security;

public class JwtSettings
{
    public string SecurityKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    public SigningCredentials SigningCredentials =>
        new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey)), SecurityAlgorithms.HmacSha256Signature);
}
