namespace TabpediaFin.Handler;

public class AuthenticateHandler : IRequestHandler<AuthenticateRequestDto, AuthenticateResponseDto>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IAuthenticateRepository _repository;

    public AuthenticateHandler(IPasswordHasher passwordHasher
        , IJwtTokenGenerator jwtTokenGenerator
        , IAuthenticateRepository repository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _repository = repository;
    }


    public async Task<AuthenticateResponseDto> Handle(AuthenticateRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _repository.FetchUserAsync(request.Username, request.AppCode);
        if (user == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, new { Error = "User is not found." });
        }

        if (!user.Hashed.SequenceEqual(_passwordHasher.Hash(request.Password, user.Salt)))
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

        throw new HttpException(HttpStatusCode.Unauthorized, new { Error = "Failed process token." });
    }
}
