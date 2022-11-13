namespace TabpediaFin.Dto;

public class RefreshTokenRequestDto : BaseDto, IRequest<AuthenticateResponseDto>
{
    public string Token { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public string AppCode { get; set; } = string.Empty;
}


public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
        RuleFor(x => x.AppCode).NotNull().NotEmpty();
    }
}
