namespace TabpediaFin.Domain.TransferMoney;

public class TransferMoney : BaseEntity
{
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; }
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
}
