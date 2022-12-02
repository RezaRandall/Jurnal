﻿namespace TabpediaFin.Domain;

public class Expense : BaseEntity
{
    public string TransNum { get; set; } = string.Empty;
    public DateTime? TransDate { get; set; }
    public int ContactId { get; set; } = 0;
    public int PaymentMethodId { get; set; } = 0;
    public int PaymentTermId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int DiscountTypeId { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
}
