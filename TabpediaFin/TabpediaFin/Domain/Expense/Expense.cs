namespace TabpediaFin.Domain.Expense;

public class Expense : BaseEntity
{
    public int PayFromAccountId { get; set; } = 0;
    public Boolean PayLater { get; set; } = false;
    public int RecipientContactId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public int PaymentMethodId { get; set; } = 0;
    public string TransactionNo { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public int PaymentTermId { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int Status { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public int DiscountForAccountId { get; set; } = 0;
    public Int64 TotalAmount { get; set; } = 0;
}
