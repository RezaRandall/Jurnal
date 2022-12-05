namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodFetchHandler : IFetchByIdHandler<PaymentMethodDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentMethodFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }


    public async Task<RowResponse<PaymentMethodDto>> Handle(FetchByIdRequestDto<PaymentMethodDto> request, CancellationToken cancellationToken)
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


[Table("PaymentMethod")]
public class PaymentMethodDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;

    [Searchable]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
