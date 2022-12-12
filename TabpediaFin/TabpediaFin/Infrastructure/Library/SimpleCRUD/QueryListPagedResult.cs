namespace TabpediaFin.Infrastructure.Library.SimpleCRUD;

public class QueryPagedResult<T>
{
    public List<T> List { get; set; } = new List<T>();

    public int TotalRecord { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}
