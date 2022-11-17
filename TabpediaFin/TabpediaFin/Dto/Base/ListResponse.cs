namespace TabpediaFin.Dto.Base;

public class ListResponse<T> : BaseResponse
{
    public List<T>? List { get; set; }

}
