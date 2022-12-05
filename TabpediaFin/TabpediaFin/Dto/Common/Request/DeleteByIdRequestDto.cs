namespace TabpediaFin.Dto.Common.Request;

public class DeleteByIdRequestDto<TId, TResponse> : IRequest<TResponse>
{
    public TId? Id { get; set; } = default;
}


public class DeleteByIdRequestDto<T> : DeleteByIdRequestDto<int, RowResponse<T>>
{
    public DeleteByIdRequestDto(int id)
    {
        Id = id;
    }
}
