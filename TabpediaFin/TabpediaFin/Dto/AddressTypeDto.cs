namespace TabpediaFin.Dto
{
    [Table("AddressType")]
    public class AddressTypeDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
