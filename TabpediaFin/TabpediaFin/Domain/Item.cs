namespace TabpediaFin.Domain
{
    public class Item : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public int UnitMeasureId { get; set; } = 0;
        public int AverageCost { get; set; } = 0;
        public int Cost { get; set; } = 0;
        public int Price { get; set; } = 0;
        public bool IsSales { get; set; } = true;
        public bool IsPurchase { get; set; } = true;
        public bool IsStock { get; set; } = true;
        public int StockMin { get; set; } = 0;
        public bool IsArchived { get; set; } = true;
        public string ImageFileName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int PurchaseAccount { get; set; } = 0;
        public int SalesAccount { get; set; } = 0;
    }
}
