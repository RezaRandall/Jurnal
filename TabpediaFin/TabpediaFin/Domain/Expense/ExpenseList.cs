namespace TabpediaFin.Domain.Expense;

public class ExpenseList : BaseEntity
{
    public int Id { get; set; } = 0;
    public bool PriceIncludesTax { get; set; } = false;
    public int ExpenseAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int TransId { get; set; } = 0;
}
