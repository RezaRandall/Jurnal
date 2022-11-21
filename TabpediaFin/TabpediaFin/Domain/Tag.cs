namespace TabpediaFin.Domain
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
