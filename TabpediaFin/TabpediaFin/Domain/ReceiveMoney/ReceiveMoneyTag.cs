namespace TabpediaFin.Domain.ReceiveMoney;

public class ReceiveMoneyTag : BaseEntity
{
    public int Id { get; set; } = 0;
    public int TagId { get; set; }
    public int TransId { get; set; }
}
