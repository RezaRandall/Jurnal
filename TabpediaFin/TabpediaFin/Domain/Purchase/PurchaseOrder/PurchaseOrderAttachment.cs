namespace TabpediaFin.Domain
{
    public class PurchaseOrderAttachment : BaseEntity
    {
        public int Id { get; set; } = 0;
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public int TransId { get; set; }
    }
}
