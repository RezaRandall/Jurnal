namespace TabpediaFin.Domain
{
    public class Tax : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        
        public double RatePercent { get; set; }
    }
}
