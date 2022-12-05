using TabpediaFin.Handler.ExpenseCategoryHandler;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseDeleteHandler : IRequestHandler<ExpenseDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<bool>> Handle(ExpenseDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var expense = await _context.Expense.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);

            _context.Expense.Attach(expense);
            _context.Expense.Remove(expense);

            await _context.SaveChangesAsync(cancellationToken);

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = true;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}

public class ExpenseDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; }
}