namespace TabpediaFin.Handler.ExpenseCategoryHandler;

public class ExpenseCategoryInsertHandler : IRequestHandler<ExpenseCategoryInsertDto, RowResponse<ExpenseCategoryFetchDto>>
{
    private readonly FinContext _context;
    private readonly IExpenseCategoryCacheRemover _cacheRemover;

    public ExpenseCategoryInsertHandler(FinContext db, IExpenseCategoryCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
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

            _cacheRemover.RemoveCache();

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
public class ExpenseCategoryInsertValidator : AbstractValidator<ExpenseCategoryInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ExpenseCategoryInsertValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250)
            .MustAsync(
                async (model, name, cancellation) =>
                {
                    return await IsUniqueName(model, name, cancellation);
                }
            ).WithMessage("Name must be unique");

        RuleFor(x => x.Description).MaximumLength(250);
    }

    public async Task<bool> IsUniqueName(ExpenseCategoryInsertDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsExpenseCategoryNameExist(name, 0);
        return !isExist;
    }
}

