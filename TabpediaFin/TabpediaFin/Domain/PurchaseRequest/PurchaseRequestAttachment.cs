namespace TabpediaFin.Domain
{
    public class PurchaseRequestAttachment : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public double FileSize { get; set; }
        public int TransId { get; set; }
    }
}
