namespace TabpediaFin.Handler.Interfaces;

public interface IViewByIdHandler<T> : IRequestHandler<ViewByIdRequestDto<T>, RowResponse<T>>
{
}
