namespace TabpediaFin.Dto;

public class AuthenticateRequestDto : BaseDto, IRequest<AuthenticateResponseDto>
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string AppCode { get; set; } = string.Empty;
}


public class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequestDto>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.AppCode).NotNull().NotEmpty();
    }
}