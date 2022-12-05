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
    public DbSet<Item> Item { get; set; }
    public DbSet<UnitMeasure> UnitMeasure { get; set; }
    public DbSet<ItemUnitMeasure> ItemUnitMeasure { get; set; }
    public DbSet<ItemItemCategory> ItemItemCategory { get; set; }
    public DbSet<PaymentTerms> PaymentTerm { get; set; }
    public DbSet<Expense> Expense { get; set; }
    public DbSet<AccountCashAndBank> AccountCashAndBank { get; set; }
    public DbSet<ItemCategory> ItemCategory { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Tax> Tax { get; set; }
    public DbSet<ContactAddressType> ContactAddressType { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<ContactGroup> ContactGroup { get; set; }
    public DbSet<ContactAddress> ContactAddress { get; set; }
    public DbSet<ContactPerson> ContactPerson { get; set; }
    public DbSet<Warehouse> Warehouse { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategory { get; set; }
    public DbSet<PurchaseRequest> PurchaseRequest { get; set; }
    public DbSet<PurchaseRequestTag> PurchaseRequestTag { get; set; }
    public DbSet<PurchaseRequestAttachment> PurchaseRequestAttachment { get; set; }
    public DbSet<PurchaseRequestItem> PurchaseRequestItem { get; set; }
    


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AppendGlobalQueryFilter<IHasTenant>(s => s.TenantId == _currentUser.TenantId);
        base.OnModelCreating(modelBuilder);
    }

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


    public int SaveChangesAsync()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }


    private void UpdateAuditFields()
    {
        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entry in modifiedEntries)
        {
            var entity = entry.Entity as BaseEntity;
            if (entity != null) 
            {
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
}
