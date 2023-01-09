namespace TabpediaFin.Domain;

public class Account : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CategoryId { get; set; } = 0;
    public int AccountParentId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public double Balance { get; set; } = 0;
    public bool IsLocked { get; set; } = false;
    public int BankId { get; set; } = 0; 
    public Account(string name, string accountnumber, int categoryid, int accountparentid, int taxid, string description, double balance, bool islocked, int tenantid, int userid)
    {
        Name = name;
        AccountNumber = accountnumber;
        CategoryId = categoryid;
        AccountParentId = accountparentid;
        TaxId = taxid;
        Description = description;
        Balance = balance;
        IsLocked = islocked;
        TenantId = tenantid;
        CreatedUid = userid;
        UpdatedUid = userid;
        UpdatedUtc = DateTime.UtcNow;

    }

    public Account() { }
}
