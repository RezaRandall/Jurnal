using System.Security.Claims;

namespace TabpediaFin.Infrastructure.Security;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _user;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        if (_httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("User Principal not valid");
        }

        _user = _httpContextAccessor.HttpContext.User;
    }


    public int UserId
    {
        get
        {
            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.OrdinalIgnoreCase));
            if (claim == null) return 0;

            return Convert.ToInt32(claim.Value);
        }
    }


    public string Username
    {
        get
        {
            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals("Username", StringComparison.OrdinalIgnoreCase));
            if (claim == null) return string.Empty;

            return claim.Value;
        }
    }

    public string FullName
    {
        get
        {
            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals("FullName", StringComparison.OrdinalIgnoreCase));
            if (claim == null) return string.Empty;

            return claim.Value;
        }
    }


    public int RoleId
    {
        get
        {
            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals("RoleId", StringComparison.OrdinalIgnoreCase));
            if (claim == null) return 0;

            return Convert.ToInt32(claim.Value);
        }
    }

    public string RoleName
    {
        get
        {
            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals("RoleName", StringComparison.OrdinalIgnoreCase));
            if (claim == null) return string.Empty;

            return claim.Value;
        }
    }

    public int TenantId
    {
        get
        {
            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals("TenantId", StringComparison.OrdinalIgnoreCase));
            if (claim == null) return 0;

            return Convert.ToInt32(claim.Value);
        }
    }

    public string TenantName
    {
        get
        {
            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals("TenantName", StringComparison.OrdinalIgnoreCase));
            if (claim == null) return string.Empty;

            return claim.Value;
        }
    }
}


public interface ICurrentUser
{
    int UserId { get; }

    string Username { get; }

    string FullName { get; }

    int RoleId { get; }

    string RoleName { get; }

    int TenantId { get; }

    string TenantName { get; }
}
