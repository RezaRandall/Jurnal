namespace TabpediaFin.Dto.Base;

public class PagedListResponse<T> : BaseResponse
{
    public List<T>? Data { get; set; }

    public int Total { get; set; }
}
