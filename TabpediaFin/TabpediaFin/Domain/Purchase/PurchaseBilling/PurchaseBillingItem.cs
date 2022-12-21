namespace TabpediaFin.Domain
{
    public class PurchaseBillingItem : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
        public int TransId { get; set; }
        public int TaxId { get; set; }
    }
}
