namespace TabpediaFin.Handler.Interfaces;

public interface IQueryPagedListHandler<T> : IRequestHandler<QueryPagedListDto<T>, PagedListResponse<T>>
{
}
