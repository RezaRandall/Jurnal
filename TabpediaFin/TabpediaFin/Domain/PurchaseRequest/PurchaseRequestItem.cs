namespace TabpediaFin.Domain
{
    public class PurchaseRequestItem : BaseEntity
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitMeasureId { get; set; }
        public int TransId { get; set; }
    }
}
