namespace TabpediaFin.Domain.TransferMoney;

public class TransferMoney : BaseEntity
{
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; }
    public int Amount { get; set; } = 0;
    public int Memo { get; set; } = 0;
    //public int Tag { get; set; } = 0;
    public string TransactionNumber { get; set; } = string.Empty;
    //public int FileName { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
}
