using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Handler.PaymentTerm;

public class PaymentTermByIdHandler : IFetchByIdHandler<PaymentTermDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentTermByIdHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<PaymentTermDto>> Handle(FetchByIdRequestDto<PaymentTermDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<PaymentTermDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<PaymentTermDto>(request.Id, _currentUser);
                if (row == null)
                {
                    response.IsOk = false;
                    response.Row = row;
                    response.ErrorMessage = "Data not found";
                }
                else
                {
                    response.IsOk = true;
                    response.Row = row;
                    response.ErrorMessage = string.Empty;
                }
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

[Table("PaymentTerm")]
public class PaymentTermDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TermDays { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}