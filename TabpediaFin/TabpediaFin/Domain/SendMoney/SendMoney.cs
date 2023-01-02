namespace TabpediaFin.Domain.SendMoney;

public class SendMoney : BaseEntity
{
    public int PayFromAccountId { get; set; } = 0;
    public int ReceiverId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
}
