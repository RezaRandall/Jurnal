namespace TabpediaFin.Dto.Common.Request;

public class QueryPagedListDto<T> : IRequest<PagedListResponse<T>>
{
    private QueryPagedListDto<contactqueryDto> request;

    public QueryPagedListDto()
    {
    }

    public QueryPagedListDto(QueryPagedListDto<contactqueryDto> request)
    {
        this.PageSize = request.PageSize;
        this.PageNum = request.PageNum;
        this.Search = request.Search;
        this.SortBy = request.SortBy;
        this.SortDesc = request.SortDesc;
    }

    public int PageSize { get; set; } = 10;

    public int PageNum { get; set; } = 1;

    public string Search { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public bool SortDesc { get; set; }
}
