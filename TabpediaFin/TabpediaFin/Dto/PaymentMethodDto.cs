namespace TabpediaFin.Dto;

[Table("PaymentMethod")]
public class PaymentMethodDto : BaseDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
