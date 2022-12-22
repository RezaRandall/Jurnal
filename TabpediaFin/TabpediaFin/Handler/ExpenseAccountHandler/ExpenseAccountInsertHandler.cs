using TabpediaFin.Domain.Expense;
using TabpediaFin.Handler.ExpenseHandler;

namespace TabpediaFin.Handler.ExpenseAccountHandler;

public class ExpenseAccountInsertHandler : IRequestHandler<ExpenseAccountInsertDto, RowResponse<ExpenseAccountFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseAccountInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseAccountFetchDto>> Handle(ExpenseAccountInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseAccountFetchDto>();

        var expenseAccount = new ExpenseAccount()
        {
            Name = request.Name,
            ExpenseAccountNumber = request.ExpenseAccountNumber,
            ExpenseCategoryId = request.ExpenseCategoryId,
            TaxId = request.TaxId
        };

        try
        {
            await _context.ExpenseAccount.AddAsync(expenseAccount, cancellationToken);
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

public class ExpenseAccountInsertDto : IRequest<RowResponse<ExpenseAccountFetchDto>>
{
    public string Name { get; set; } = string.Empty;
    public string ExpenseAccountNumber { get; set; } = string.Empty;
    public int ExpenseCategoryId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
}