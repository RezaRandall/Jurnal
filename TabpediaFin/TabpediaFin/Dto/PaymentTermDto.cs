using System.Runtime.Serialization;

namespace TabpediaFin.Dto;

[Table("PaymentTerm")]
public class PaymentTermDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? TermDays { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
public class GetPaymentTermQuery : IRequest<UnitMeasure>
{
    public int? Id { get; set; } = 0;
    public int? TenantId { get; set; } = 0;
}
public class GetPaymentTermListQuery : IRequest<List<PaymentTerm>>
{
    public string? searchby { get; set; } = string.Empty;
    [JsonIgnore]
    public int? TenantId { get; set; } = 0;
}
public class DeletePaymentTerm : IRequest<bool>
{
    public int? Id { get; set; } = 0;
    public int? TenantId { get; set; } = 0;
}

public class AddPaymentTerm : IRequest<PaymentTerm>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? TermDays { get; set; }
    public bool IsActive { get; set; }
    [JsonIgnore]
    [IgnoreDataMember]
    public int? CreatedUid { get; set; } = 0;
    [JsonIgnore]
    [IgnoreDataMember]
    public int? TenantId { get; set; } = 0;
}

public class UpdatePaymentTerm : IRequest<PaymentTerm>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TermDays { get; set; }
    public bool IsActive { get; set; }
    [JsonIgnore]
    [IgnoreDataMember]
    public int? UpdatedUid { get; set; } = 0;
    [JsonIgnore]
    [IgnoreDataMember]
    public int? TenantId { get; set; } = 0;
}
