namespace TabpediaFin.Domain
{
    public class SalesOrderItem : BaseEntity
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
        public int TransId { get; set; }
    }
}
