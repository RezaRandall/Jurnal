namespace TabpediaFin.Domain.SendMoney;

public class SendMoneyTag : BaseEntity
{
    public int Id { get; set; }
    public int TagId { get; set; }
    public int TransId { get; set; }
}
