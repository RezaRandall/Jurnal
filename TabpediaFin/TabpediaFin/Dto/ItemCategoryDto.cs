namespace TabpediaFin.Dto
{
    [Table("ItemCategory")]
    public class ItemCategoryDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class GetItemCategoryListQuery : IRequest<List<ItemCategory>>
    {
        public string? sortby { get; set; } = string.Empty;
        public string? valsort { get; set; } = string.Empty;
        public string? searchby { get; set; } = string.Empty;
        public string? valsearch { get; set; } = string.Empty;
        public int? jumlah_data { get; set; } = 5;
        public int? offset { get; set; } = 0;
        [JsonIgnore]
        public int? TenantId { get; set; } = 0;
    }
    public class GetItemCategoryQuery : IRequest<ItemCategory>
    {
        public int? Id { get; set; } = 0;
        public int? TenantId { get; set; } = 0;
    }
    public class DeleteItemCategory : IRequest<bool>
    {
        public int? Id { get; set; } = 0;
        public int? TenantId { get; set; } = 0;
    }

    public class AddItemCategory : IRequest<ItemCategory>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatedUid { get; set; } = 0;
        public int? TenantId { get; set; } = 0;
    }

    public class UpdateItemCategory : IRequest<ItemCategory>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? UpdatedUid { get; set; } = 0;
        public int? TenantId { get; set; } = 0;
    }
}
