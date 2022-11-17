namespace TabpediaFin.Dto
{
    [Table("ContactGroup")]
    public class ContactGroupDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
