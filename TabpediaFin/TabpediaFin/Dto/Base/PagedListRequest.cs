namespace TabpediaFin.Dto.Base;

public class PagedListRequest
{
    public int PageSize { get; set; }

    public int PageNum { get; set; }

    public string Search { get; set; } = string.Empty;

    public string Sort { get; set; } = string.Empty;
}
