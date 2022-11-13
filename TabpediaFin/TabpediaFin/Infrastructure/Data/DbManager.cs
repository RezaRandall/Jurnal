using Npgsql;

namespace TabpediaFin.Infrastructure.Data;

public class DbManager
{
    private readonly string _connectionString;

    public DbManager(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public string ConnectionString => _connectionString;

    public IDbConnection CreateConnection()
    {
        IDbConnection cn = new NpgsqlConnection(_connectionString);

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        // SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);

        return cn;
    }
}
