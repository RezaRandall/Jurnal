namespace TabpediaFin.Dto.Common.Request;

public class FetchByIdRequestDto<TId, TResponse> : IRequest<TResponse>
{
    public TId? Id { get; set; } = default;
}


public class FetchByIdRequestDto<T> : FetchByIdRequestDto<int, RowResponse<T>>
{
    public FetchByIdRequestDto(int id)
    {
        Id = id;
    }
}
