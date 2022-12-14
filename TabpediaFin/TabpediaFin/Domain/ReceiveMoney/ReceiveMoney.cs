namespace TabpediaFin.Domain.ReceiveMoney;

public class ReceiveMoney : BaseEntity
{
    public int DepositToAccountId { get; set; } = 0;
    public int VendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; } 
    public string TransactionNo { get; set; } = string.Empty;
    public bool PriceIncludesTax { get; set; } = false;
    public int ReceiveFromAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    //public string AttachmentFileName { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
}
