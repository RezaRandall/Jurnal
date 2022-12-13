namespace TabpediaFin.Domain
{
    public class PurchaseBillingTag : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int TagId { get; set; }
        public int TransId { get; set; }
    }
}
