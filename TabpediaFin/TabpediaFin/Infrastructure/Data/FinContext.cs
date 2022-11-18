using Microsoft.EntityFrameworkCore;

namespace TabpediaFin.Infrastructure.Data;

public class FinContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public FinContext(DbContextOptions options, IConfiguration configuration, ILoggerFactory loggerFactory) : base(options)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Default"));
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

}
