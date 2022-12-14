namespace TabpediaFin.Domain.TransferMoney;

public class TransferMoneyTag : BaseEntity
{
    public int Id { get; set; }
    public int TagId { get; set; }
    public int TransId { get; set; }
}
