namespace TabpediaFin.Handler.Interfaces;

public interface IFetchByIdHandler<TId, TResponse> : IRequestHandler<FetchByIdRequestDto<TId, TResponse>, TResponse>
{
}

public interface IFetchByIdHandler<T> : IRequestHandler<FetchByIdRequestDto<T>, RowResponse<T>>
{
}
