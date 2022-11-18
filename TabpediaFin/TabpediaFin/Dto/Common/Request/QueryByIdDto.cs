namespace TabpediaFin.Dto.Common.Request;

public class QueryByIdDto<TId, TResponse> : IRequest<TResponse>
{
    public TId Id { get; set; }
}


public class QueryByIdDto<T> : QueryByIdDto<int, RowResponse<T>>
{
    public QueryByIdDto(int id)
    {
        Id = id;
    }
}
