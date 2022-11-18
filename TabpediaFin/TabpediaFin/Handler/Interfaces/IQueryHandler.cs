namespace TabpediaFin.Handler.Interfaces;

public interface IQueryByIdHandler<TId, TResponse> : IRequestHandler<QueryByIdDto<TId, TResponse>, TResponse>
{
}

public interface IQueryByIdHandler<T> : IRequestHandler<QueryByIdDto<T>, RowResponse<T>>
{
}


//public interface IQueryByIdHandler<T> : IQueryByIdHandler<int, RowResponse<T>>
//{
//}


