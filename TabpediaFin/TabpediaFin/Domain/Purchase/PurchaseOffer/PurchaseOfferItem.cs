namespace TabpediaFin.Domain
{
    public class PurchaseOfferItem : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
        public int TransId { get; set; }
    }
}
