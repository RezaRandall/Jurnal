using System.Runtime.Serialization;
using TabpediaFin.Handler.Product;

namespace TabpediaFin.Dto;

[Table("PaymentTerm")]
public class PaymentTermDto
{
    protected PaymentTermDto()
    {
    }

    public PaymentTermDto(
        int id,
        int tenantId,
        string name,
        string description,
        int termDays,
        bool isActive,
        int createdUid,
        DateTime createdUtc,
        int updatedUid,
        DateTime updatedUtc

        )
    {
        Id = id;
        TenantId = tenantId;
        Name = name;
        Description = description;
        TermDays = termDays;
        IsActive = isActive;
        CreatedUid = createdUid;
        CreatedUtc = createdUtc;
        UpdatedUid = updatedUid;
        UpdatedUtc = updatedUtc;
    }

    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TermDays { get; set; }
    public bool IsActive { get; set; }
    public int CreatedUid { get; set; }
    public DateTime CreatedUtc { get; set; }
    public int UpdatedUid { get; set; }
    public DateTime UpdatedUtc { get; set; }
}







//public class PaymentTermDto : BaseDto
//{
//    public string Name { get; set; } = string.Empty;
//    public string Description { get; set; } = string.Empty;
//    public int? TermDays { get; set; } = 0;
//    public bool IsActive { get; set; } = true;
//}

//public class GetPaymentTermListQuery : IRequest<List<PaymentTerm>>
//{
//    public string? searchby { get; set; } = string.Empty;
//    [JsonIgnore]
//    public int? TenantId { get; set; } = 0;
//}
public class GetPaymentTermQuery : IRequest<PaymentTerm>
{
    public int? Id { get; set; } = 0;
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
