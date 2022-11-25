namespace TabpediaFin.Handler.ExpenseCategoryHandler;

public class ExpenseCategoryInsertHandler : IRequestHandler<ExpenseCategoryInsertDto, RowResponse<ExpenseCategoryFetchDto>>
{
    private readonly FinContext _context;

    public ExpenseCategoryInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ExpenseCategoryFetchDto>> Handle(ExpenseCategoryInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseCategoryFetchDto>();

        var ExpenseCategory = new ExpenseCategory()
        {
            Name = request.Name,
            Description = request.Description,
            AccountId = request.AccountId,
        };

        try
        {
            await _context.ExpenseCategory.AddAsync(ExpenseCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ExpenseCategoryFetchDto()
            {
                Id = ExpenseCategory.Id,
                Name = ExpenseCategory.Name,
                Description = ExpenseCategory.Description,
                AccountId = ExpenseCategory.AccountId
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



public class ExpenseCategoryInsertDto : IRequest<RowResponse<ExpenseCategoryFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public int AccountId { get; set; }

}
