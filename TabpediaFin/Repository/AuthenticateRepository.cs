namespace TabpediaFin.Repository;

public class AuthenticateRepository : BaseRepository, IAuthenticateRepository
{
    public AuthenticateRepository(DbManager dbContext)
        : base(dbContext)
    {

    }

    public async Task<AuthenticateResponseDto> FetchUserAsync(string username, string appCode)
    {
        var sql = @"SELECT u.""Id"" AS UserId
            , u.""Username""
            , u.""FullName""
            , x.""RoleId""
            , r.""RoleName""
            , u.""TenantId""
            , t.""Name"" AS ""TenantName""
            , u.""Hashed"" 
            , u.""Salt"" 
            FROM ""AppUserRole"" x
            INNER JOIN ""AppRole"" r ON x.""RoleId"" = r.""Id"" 
            INNER JOIN ""App"" a ON r.""AppId"" = a.""Id""
            INNER JOIN ""AppUser"" u ON x.""UserId"" = u.""Id""
            INNER JOIN ""AppTenant"" t ON u.""TenantId"" = t.""Id""
            WHERE LOWER(u.""Username"") = @Username
            AND UPPER(a.""AppCode"") = @AppCode
            AND x.""IsActive"" = TRUE 
            AND u.""IsActive"" = TRUE 
            AND u.""IsLocked"" = FALSE 
            AND t.""IsActive"" = TRUE 
            AND t.""IsLocked"" = FALSE";

        var parameters = new DynamicParameters();
        parameters.Add("Username", username.Trim().ToLowerInvariant());
        parameters.Add("AppCode", appCode.Trim().ToUpperInvariant());

        using (var cn = _dbManager.CreateConnection())
        {
            return await cn.QueryFirstOrDefaultAsync<AuthenticateResponseDto>(sql, parameters);
        }
    }


    public async Task<bool> UpsertRefreshTokenAsync(int userid, string refreshToken)
    {
        var sql = @"INSERT INTO ""AppRefreshToken"" (""Token"", ""ExpiredUtc"", ""UserId"", ""CreatedUtc"")
            VALUES(@RefreshToken, @ExpiredUtc, @UserId, @CreatedUtc) 
            ON CONFLICT (""UserId"") 
            DO 
               UPDATE SET ""Token"" = EXCLUDED.""Token"", 
   	            ""ExpiredUtc"" = EXCLUDED.""ExpiredUtc"", 
   	            ""UserId"" = EXCLUDED.""UserId"",
   	            ""CreatedUtc"" = EXCLUDED.""CreatedUtc""";

        var expiredUtc = DateTime.UtcNow.AddDays(1);

        var parameters = new DynamicParameters();
        parameters.Add("RefreshToken", refreshToken);
        parameters.Add("ExpiredUtc", DateTime.UtcNow.AddDays(1));
        parameters.Add("UserId", userid);
        parameters.Add("CreatedUtc", DateTime.UtcNow);

        using (var cn = _dbManager.CreateConnection())
        {
            var affected = await cn.ExecuteAsync(sql, parameters);
            return affected > 0;
        }
    }


    public async Task<bool> IsValidRefreshToken(int userId, string refreshToken)
    {
        var sql = @"SELECT COUNT(1) AS ""TokenCount""
            FROM ""AppRefreshToken"" x
            WHERE x.""UserId"" = @UserId AND x.""RefreshToken"" = @RefreshToken";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("RefreshToken", refreshToken);

        using (var cn = _dbManager.CreateConnection())
        {
            var tokencount = await cn.ExecuteScalarAsync<int>(sql, parameters);
            return tokencount > 0;
        }
    }
}


public interface IAuthenticateRepository
{
    Task<AuthenticateResponseDto> FetchUserAsync(string username, string appCode);

    Task<bool> UpsertRefreshTokenAsync(int userid, string refreshToken);

    Task<bool> IsValidRefreshToken(int userId, string refreshToken);
}
