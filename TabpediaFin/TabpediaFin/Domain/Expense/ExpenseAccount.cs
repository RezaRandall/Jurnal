namespace TabpediaFin.Domain.Expense;

public class ExpenseAccount : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ExpenseAccountNumber { get; set; } = string.Empty;
    public int ExpenseCategoryId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
}
