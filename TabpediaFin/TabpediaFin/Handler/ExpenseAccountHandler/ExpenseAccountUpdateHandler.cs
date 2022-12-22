using TabpediaFin.Domain;
using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Handler.ExpenseAccountHandler;

public class ExpenseAccountUpdateHandler : IRequestHandler<ExpenseAccountUpdateDto, RowResponse<ExpenseAccountFetchDto>>
{
    private readonly FinContext _context;

    public ExpenseAccountUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ExpenseAccountFetchDto>> Handle(ExpenseAccountUpdateDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseAccountFetchDto>();

        try
        {
            var expenseAccount = await _context.ExpenseAccount.FirstAsync(x => x.Id == req.Id, cancellationToken);
            expenseAccount.Name = req.Name;
            expenseAccount.ExpenseAccountNumber = req.ExpenseAccountNumber;
            expenseAccount.ExpenseCategoryId = req.ExpenseCategoryId;
            expenseAccount.TaxId = req.TaxId;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ExpenseAccountFetchDto()
            {
                Id = expenseAccount.Id,
                Name = expenseAccount.Name,
                ExpenseAccountNumber = expenseAccount.ExpenseAccountNumber,
                ExpenseCategoryId = expenseAccount.ExpenseCategoryId,
                TaxId = expenseAccount.TaxId
            };

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }



}

[Table("ExpenseAccount")]
public class ExpenseAccountUpdateDto : IRequest<RowResponse<ExpenseAccountFetchDto>>
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string ExpenseAccountNumber { get; set; } = string.Empty;
    public int ExpenseCategoryId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
}