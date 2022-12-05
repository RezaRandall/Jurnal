namespace TabpediaFin.Handler.Interfaces;

public interface IDeleteByIdHandler<TId, TResponse> : IRequestHandler<DeleteByIdRequestDto<TId, TResponse>, TResponse>
{
}

public interface IDeleteByIdHandler<T> : IRequestHandler<DeleteByIdRequestDto<T>, RowResponse<T>>
{
}
