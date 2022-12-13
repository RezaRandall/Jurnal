namespace TabpediaFin.Domain
{
    public class SalesOrderTag : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int TagId { get; set; }
        public int TransId { get; set; }
    }
}
