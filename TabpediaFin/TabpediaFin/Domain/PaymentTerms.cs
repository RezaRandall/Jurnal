namespace TabpediaFin.Domain;

public class PaymentTerms : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TermDays { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
