namespace TabpediaFin.Handler.Product;
public class PaymentTermFetchHandler : IQueryByIdHandler<PaymentTermDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentTermFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }


    public async Task<RowResponse<PaymentTermDto>> Handle(QueryByIdDto<PaymentTermDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<PaymentTermDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<PaymentTermDto>(request.Id, _currentUser);

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
