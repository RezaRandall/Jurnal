namespace TabpediaFin.Dto.Common.Request;

public class QueryPagedListDto<T> : IRequest<PagedListResponse<T>>
{
    //private QueryPagedListDto<contactqueryDto> request;

    //public QueryPagedListDto()
    //{
    //}

    //public QueryPagedListDto(QueryPagedListDto<contactqueryDto> request)
    //{
    //    PageSize = request.PageSize;
    //    PageNum = request.PageNum;
    //    Search = request.Search;
    //    SortBy = request.SortBy;
    //    SortDesc = request.SortDesc;
    //}

    public int PageSize { get; set; } = 10;

    public int PageNum { get; set; } = 1;

    public string Search { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public bool SortDesc { get; set; }
}
