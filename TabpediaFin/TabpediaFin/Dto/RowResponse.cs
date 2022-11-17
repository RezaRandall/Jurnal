namespace TabpediaFin.Dto;

public class RowResponse<T> : BaseResponse
{
    public T? Row { get; set; }

}
