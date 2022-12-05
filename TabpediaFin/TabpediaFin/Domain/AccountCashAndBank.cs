namespace TabpediaFin.Domain;

public class AccountCashAndBank : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CashAndBankCategoryId { get; set; } = 0;
    public int DetailAccountId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
}
