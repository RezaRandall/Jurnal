namespace TabpediaFin.Dto
{
    [Table("Tag")]
    public class TagDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
