namespace TabpediaFin.Dto.Common.Response;

public class ListResponse<T> : BaseResponse
{
    public List<T>? List { get; set; }

}
