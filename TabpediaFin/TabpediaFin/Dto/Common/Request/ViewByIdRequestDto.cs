namespace TabpediaFin.Dto.Common.Request;

public class ViewByIdRequestDto<TId, TResponse> : IRequest<TResponse>
{
    public TId? Id { get; set; } = default;
}


public class ViewByIdRequestDto<T> : ViewByIdRequestDto<int, RowResponse<T>>
{
    public ViewByIdRequestDto(int id)
    {
        Id = id;
    }
}
