namespace TabpediaFin.Handler.ExpenseCategoryHandler;

public class ExpenseCategoryUpdateHandler : IRequestHandler<ExpenseCategoryUpdateDto, RowResponse<ExpenseCategoryFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseCategoryUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseCategoryFetchDto>> Handle(ExpenseCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseCategoryFetchDto>();

        try
        {
            var ExpenseCategory = await _context.ExpenseCategory.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            ExpenseCategory.Name = request.Name;
            ExpenseCategory.Description = request.Description;
            ExpenseCategory.AccountId = request.AccountId;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ExpenseCategoryFetchDto()
            {
                Id = request.Id,
                Name = ExpenseCategory.Name,
                Description = ExpenseCategory.Description,
                AccountId = ExpenseCategory.AccountId,
            };

            result.IsOk= true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk= false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}


public class ExpenseCategoryUpdateDto : IRequest<RowResponse<ExpenseCategoryFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public int AccountId { get; set; }
}
