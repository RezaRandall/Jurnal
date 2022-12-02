using TabpediaFin.Domain;
using TabpediaFin.Handler.ExpenseCategoryHandler;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseUpdateHandler : IRequestHandler<ExpenseUpdateDto, RowResponse<ExpenseFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(ExpenseUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseFetchDto>();

        try
        {
            var expense = await _context.Expense.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            expense.TransNum = request.TransNum;
            expense.TransDate = request.TransDate;
            expense.ContactId = request.ContactId;
            expense.PaymentMethodId = request.PaymentMethodId;
            expense.PaymentTermId = request.PaymentTermId;
            expense.Amount = request.Amount;
            expense.DiscountTypeId = request.DiscountTypeId;
            expense.DiscountPercent = request.DiscountPercent;
            expense.DiscountAmount = request.DiscountAmount;
            expense.Notes = request.Notes;
            expense.Description = request.Description;
            expense.Description = request.Description;
            expense.TaxId = request.TaxId;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ExpenseFetchDto()
            {
                Id = request.Id,
                TransNum = request.TransNum,
                TransDate = request.TransDate,
                ContactId = request.ContactId,
                PaymentMethodId = request.PaymentMethodId,
                PaymentTermId = request.PaymentTermId,
                Amount = request.Amount,
                DiscountTypeId = request.DiscountTypeId,
                DiscountPercent = request.DiscountPercent,
                DiscountAmount = request.DiscountAmount,
                Notes = request.Notes,
                Description = request.Description,
                TaxId = request.TaxId,
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

public class ExpenseUpdateDto : IRequest<RowResponse<ExpenseFetchDto>>
{
    public int Id { get; set; }
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