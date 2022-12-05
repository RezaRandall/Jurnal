namespace TabpediaFin.Handler.Interfaces;

public interface ICreateHandler<T> : IRequestHandler<CreateRequestDto<T>, RowResponse<T>>
{
}
