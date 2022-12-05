namespace TabpediaFin.Handler.Interfaces;

public interface IFetchPagedListHandler<T> : IRequestHandler<FetchPagedListRequestDto<T>, PagedListResponse<T>>
{
}
