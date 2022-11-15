using Microsoft.EntityFrameworkCore;
using TabpediaFin.Domain;

namespace TabpediaFin.Infrastructure.Data;

public class FinContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public FinContext(DbContextOptions<FinContext> options, IConfiguration configuration, ILoggerFactory loggerFactory) : base(options)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }
    public DbSet<UnitMeasures> UnitMeasure { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Default"));
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

}
