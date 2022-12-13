namespace TabpediaFin.Domain
{
    public class SalesBillingTag : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int TagId { get; set; }
        public int TransId { get; set; }
    }
}
