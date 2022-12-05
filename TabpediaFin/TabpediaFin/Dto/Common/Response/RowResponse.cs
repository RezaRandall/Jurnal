namespace TabpediaFin.Dto.Common.Response;

public class RowResponse<T> : BaseResponse
{
    public T? Row { get; set; }

}
