using TabpediaFin.Handler.ExpenseHandler;
using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Handler.ExpenseAccountHandler;

public class ExpenseAccountFetchHandler : IFetchByIdHandler<ExpenseAccountFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ExpenseAccountFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseAccountFetchDto>> Handle(FetchByIdRequestDto<ExpenseAccountFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<ExpenseAccountFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<ExpenseAccountFetchDto>(request.Id, _currentUser);
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


[Table("ExpenseAccount")]
public class ExpenseAccountFetchDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string ExpenseAccountNumber { get; set; } = string.Empty;
    public int ExpenseCategoryId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
}
