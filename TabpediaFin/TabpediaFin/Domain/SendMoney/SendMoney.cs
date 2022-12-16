namespace TabpediaFin.Domain.SendMoney;

public class SendMoney : BaseEntity
{
    public int PayFromAccountId { get; set; } = 0;
    public int ReceiverVendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountCashAndBankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
}
