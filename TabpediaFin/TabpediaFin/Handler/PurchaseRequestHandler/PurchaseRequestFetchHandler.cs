namespace TabpediaFin.Handler.PurchaseRequestHandler
{
    public class PurchaseRequestFetchHandler
    {
    }

    public class PurchaseRequestFetchDto : BaseDto
    {
        public int StaffId { get; set; }
        public int VendorId { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public string BudgetYear { get; set; } = string.Empty;
        public int UrgentLevel { get; set; }
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string AttachmentLink { get; set; } = string.Empty;
        public List<PurchaseRequestFetchTag> TagList { get; set; }
        public List<PurchaseRequestFetchItem> ItemList { get; set; }
        public List<PurchaseRequestFetchAttachment> AttachmentList { get; set; }
    }

    public class PurchaseRequestFetchTag : BaseDto
    {
        public int TagId { get; set; }
        public int TransId { get; set; }
    }

    public class PurchaseRequestFetchItem : BaseDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitMeasureId { get; set; }
        public int TransId { get; set; }
    }
    public class PurchaseRequestFetchAttachment : BaseDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public double FileSize { get; set; }
        public int TransId { get; set; }
    }
}
