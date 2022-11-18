namespace TabpediaFin.Dto.Common.Response;

public class PagedListResponse<T> : BaseResponse
{
    public List<T>? List { get; set; }

    public int RecordCount { get; set; }
}
