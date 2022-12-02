
using TabpediaFin.Handler.ExpenseCategoryHandler;
using TabpediaFin.Handler.ExpenseHandler;
using TabpediaFin.Handler.Item;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseInsertHandler : IRequestHandler<ExpenseInsertDto, RowResponse<ExpenseFetchDto>>
{
    private readonly FinContext _context;

    public ExpenseInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(ExpenseInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseFetchDto>();

        var expense = new Expense()
        {
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

        try
        {
            await _context.Expense.AddAsync(expense, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ExpenseFetchDto()
            {
                TransNum = expense.TransNum,
                TransDate = expense.TransDate,
                ContactId = expense.ContactId,
                PaymentMethodId = expense.PaymentMethodId,
                PaymentTermId = expense.PaymentTermId,
                Amount = expense.Amount,
                DiscountTypeId = expense.DiscountTypeId,
                DiscountPercent = expense.DiscountPercent,
                DiscountAmount = expense.DiscountAmount,
                Notes = expense.Notes,
                Description = expense.Description,
                TaxId = expense.TaxId,
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

public class ExpenseInsertDto : IRequest<RowResponse<ExpenseFetchDto>>
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
