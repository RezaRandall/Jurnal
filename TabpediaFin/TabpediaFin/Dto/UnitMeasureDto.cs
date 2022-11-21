using System.Runtime.Serialization;
using TabpediaFin.Handler.UnitMeasure;
//using UnitMeasure = TabpediaFin.Repository.UnitMeasure;
//using static TabpediaFin.Repository.UnitMeasureRepository;

namespace TabpediaFin.Dto;

[Table("UnitMeasure")]

public class UnitMeasureDto
{
    protected UnitMeasureDto()
    {
    }

    public UnitMeasureDto(
        int id,
        int tenantId,
        string name,
        string description,
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
        CreatedUid = createdUid;
        CreatedUtc = createdUtc;
        UpdatedUid = updatedUid;
        UpdatedUtc = updatedUtc;
    }

    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatedUid { get; set; }
    public DateTime CreatedUtc { get; set; }
    public int UpdatedUid { get; set; }
    public DateTime UpdatedUtc { get; set; }
}


//public class UnitMeasureDto : BaseDto
//{
//    public string Name { get; set; } = string.Empty;
//    public string Description { get; set; } = string.Empty;
//}

public class GetUnitMeasureListQuery : IRequest<List<UnitMeasure>>
{
    public string? searchby { get; set; } = string.Empty;
    [JsonIgnore]
    public int? TenantId { get; set; } = 0;
}

//public class GetUnitMeasureListQuery : IRequest<List<UnitMeasure>>
//{
//    public string? sortby { get; set; } = string.Empty;
//    public string? valsort { get; set; } = string.Empty;
//    public string? searchby { get; set; } = string.Empty;
//    public string? valsearch { get; set; } = string.Empty;
//    public int? jumlah_data { get; set; } = 5;
//    public int? offset { get; set; } = 0;
//    [JsonIgnore]
//    public int? TenantId { get; set; } = 0;
//}

public class GetUnitMeasureQuery : IRequest<UnitMeasure>
{
    public int? Id { get; set; } = 0;
    public int? TenantId { get; set; } = 0;
}



public class DeleteUnitMeasure : IRequest<bool>
{
    public int? Id { get; set; } = 0;
    public int? TenantId { get; set; } = 0;
}

public class AddUnitMeasure : IRequest<UnitMeasure>
{
    public string Name { get; set; }
    public string Description { get; set; }
    [JsonIgnore]
    [IgnoreDataMember]
    public int? CreatedUid { get; set; } = 0;
    [JsonIgnore]
    [IgnoreDataMember]
    public int? TenantId { get; set; } = 0;
}

public class UpdateUnitMeasure : IRequest<UnitMeasure>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [JsonIgnore]
    [IgnoreDataMember]
    public int? UpdatedUid { get; set; } = 0;
    [JsonIgnore]
    [IgnoreDataMember]
    public int? TenantId { get; set; } = 0;
}
