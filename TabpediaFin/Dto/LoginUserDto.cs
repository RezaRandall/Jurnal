namespace TabpediaFin.Dto;

public class LoginUserDto : BaseDto
{
    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public string TenantName { get; set; } = string.Empty;
}
