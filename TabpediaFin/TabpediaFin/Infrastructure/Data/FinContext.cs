using Microsoft.EntityFrameworkCore;
using TabpediaFin.Domain;

namespace TabpediaFin.Infrastructure.Data;

public class FinContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ICurrentUser _currentUser;

    public FinContext(DbContextOptions options
        , IConfiguration configuration
        , ILoggerFactory loggerFactory
        , ICurrentUser currentUser        )
        : base(options)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
        _currentUser = currentUser;
    }

    public DbSet<PaymentMethod> PaymentMethod { get; set; }
    public DbSet<ItemCategory> ItemCategory { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }


    public async Task<int> SaveChangesAsync()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }


    private void UpdateAuditFields()
    {
        // get entries that are being Added or Updated
        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entry in modifiedEntries)
        {
            var entity = entry.Entity as BaseEntity;
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedUid = _currentUser.UserId;
                entity.CreatedUtc = now;
            }

            entity.TenantId = _currentUser.TenantId;
            entity.UpdatedUid = _currentUser.UserId;
            entity.UpdatedUtc = now;
        }
    }

}
