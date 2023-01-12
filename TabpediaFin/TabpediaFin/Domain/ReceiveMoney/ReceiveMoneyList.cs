namespace TabpediaFin.Domain.ReceiveMoney;

public class ReceiveMoneyList : BaseEntity
{
    public int Id { get; set; } = 0;
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int TransId { get; set; } = 0;
}
