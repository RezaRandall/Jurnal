namespace TabpediaFin.Handler.ExpenseCategoryHandler;

public class ExpenseCategoryUpdateHandler : IRequestHandler<ExpenseCategoryUpdateDto, RowResponse<ExpenseCategoryFetchDto>>
{
    private readonly FinContext _context;
    private readonly IExpenseCategoryCacheRemover _cacheRemover;

    public ExpenseCategoryUpdateHandler(FinContext db, IExpenseCategoryCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ExpenseCategoryFetchDto>> Handle(ExpenseCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseCategoryFetchDto>();

        try
        {
            var ExpenseCategory = await _context.ExpenseCategory.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (ExpenseCategory == null || ExpenseCategory.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            ExpenseCategory.Name = request.Name;
            ExpenseCategory.Description = request.Description;
            ExpenseCategory.AccountId = request.AccountId;

            await _context.SaveChangesAsync(cancellationToken);
            _cacheRemover.RemoveCache();

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

public class ExpenseCategoryUpdateValidator : AbstractValidator<ExpenseCategoryUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ExpenseCategoryUpdateValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty();

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

    public async Task<bool> IsUniqueName(ExpenseCategoryUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsExpenseCategoryNameExist(name, model.Id);
        return !isExist;
    }
}
