using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseFetchHandler : IFetchByIdHandler<ExpenseFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ExpenseFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(FetchByIdRequestDto<ExpenseFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<ExpenseFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<ExpenseFetchDto>(request.Id, _currentUser);
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

[Table("Expense")]
public class ExpenseFetchDto : BaseDto
{
    public string TransNum { get; set; } = string.Empty;
    public DateTime? TransDate { get; set; }
    public int ContactId { get; set; } = 0;
    public int PaymentMethodId { get; set; } = 0;
    public int PaymentTermId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int DiscountTypeId { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
}