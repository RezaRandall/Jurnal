using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TabpediaFin.Controllers;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public LoginController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }


    [HttpPost("login")]
    public async Task<AuthenticateResponseDto> Login([FromBody] AuthenticateRequestDto param)
        => await _mediator.Send(param);


    [HttpPost("refresh-token")]
    public async Task<AuthenticateResponseDto> RefreshToken([FromBody] RefreshTokenRequestDto param)
        => await _mediator.Send(param);


    [HttpGet("current-user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public LoginUserDto CurrentUser()
    {
        if (_currentUser == null) return new LoginUserDto();

        return new LoginUserDto()
        {
            Username = _currentUser.Username ?? string.Empty,
            FullName = _currentUser.FullName ?? string.Empty,
            RoleName = _currentUser.RoleName ?? string.Empty,
            TenantName = _currentUser.TenantName ?? string.Empty
        };
    }


}
