namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodViewHandler : IViewByIdHandler<PaymentMethodDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentMethodViewHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }


    public async Task<RowResponse<PaymentMethodDto>> Handle(ViewByIdRequestDto<PaymentMethodDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<PaymentMethodDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<PaymentMethodDto>(request.Id, _currentUser);

                response.IsOk = true;
                response.Row = row;
                response.ErrorMessage = string.Empty;
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.Row = null;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
