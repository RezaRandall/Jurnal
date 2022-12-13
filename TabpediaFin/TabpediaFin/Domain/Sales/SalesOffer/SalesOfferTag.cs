namespace TabpediaFin.Domain
{
    public class SalesOfferTag : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int TagId { get; set; }
        public int TransId { get; set; }
    }
}
