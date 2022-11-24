namespace TabpediaFin.Handler.Interfaces;

public interface IQueryReadFileExcelHandler<T> : IRequestHandler<QueryReadFileExcelDto<T>, PagedListResponse<T>>
{
}

