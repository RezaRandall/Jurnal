using System.Runtime.Serialization;
using static TabpediaFin.Repository.UnitMeasureRepository;

namespace TabpediaFin.Dto;

public class UnitMeasureDto
{
    public class GetUnitMeasureListQuery : IRequest<List<UnitMeasure>>
    {
        public string? searchby { get; set; } = string.Empty;
        [JsonIgnore]
        public int? TenantId { get; set; } = 0;
    }

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






}
