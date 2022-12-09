namespace TabpediaFin.Domain
{
    public class PurchaseOffer : BaseEntity
    {
        public int StaffId { get; set; }
        public int VendorId { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public string BudgetYear { get; set; } = string.Empty;
        public int UrgentLevel { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
