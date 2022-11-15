using TabpediaFin.Dto.Base;

namespace TabpediaFin.Dto;

public class AuthenticateResponseDto : BaseDto
{
    [JsonIgnore]
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    [JsonIgnore]
    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    [JsonIgnore]
    public int TenantId { get; set; }

    public string TenantName { get; set; } = string.Empty;

    [JsonIgnore]
    public string Hashed { get; set; } = string.Empty;

    [JsonIgnore]
    public string Salt { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public bool IsAuthorized => !string.IsNullOrWhiteSpace(AccessToken);
}
