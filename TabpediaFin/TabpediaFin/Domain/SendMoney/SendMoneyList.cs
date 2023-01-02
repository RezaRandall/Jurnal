namespace TabpediaFin.Domain.SendMoney;

public class SendMoneyList : BaseEntity
{
    public int Id { get; set; } = 0;
    public bool PriceIncludesTax { get; set; } = false;
    public int PaymentForAccountCashAndBanktId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int TransId { get; set; }
}
