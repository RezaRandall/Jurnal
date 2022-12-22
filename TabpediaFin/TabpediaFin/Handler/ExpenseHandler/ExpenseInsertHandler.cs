using TabpediaFin.Domain;
using TabpediaFin.Domain.Expense;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseInsertHandler : IRequestHandler<ExpenseInsertDto, RowResponse<ExpenseFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(ExpenseInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseFetchDto>();
        int transIdResult;
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransDate);

        var expense = new Expense()
        {
            TransNum = request.TransNum,
            TransDate = TransDate,
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
            AccountCashAndBankId = request.AccountCashAndBankId,
            PayLater = request.PayLater,
            PriceIncludesTax = request.PriceIncludesTax
        };

        try
        {
            await _context.Expense.AddAsync(expense, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            transIdResult = expense.Id;

            List<ExpenseFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transIdResult, cancellationToken);
            List<ExpenseFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);


            var row = new ExpenseFetchDto()
            {
                Id = expense.Id,
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
                AccountCashAndBankId = expense.AccountCashAndBankId,
                PayLater = expense.PayLater,
                PriceIncludesTax = expense.PriceIncludesTax
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

    public async Task<List<ExpenseFetchAttachment>> PostAttachmentAsync(List<ExpenseAttachmentFiles> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ExpenseAttachment> ExpenseAttachmentList = new List<ExpenseAttachment>();
        List<ExpenseFetchAttachment> ExpenseFetchAttachmentList = new List<ExpenseFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (ExpenseAttachmentFiles item in filedata)
            {
                ExpenseAttachmentList.Add(new ExpenseAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
                ExpenseFetchAttachmentList.Add(new ExpenseFetchAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
            }

            await _context.ExpenseAttachment.AddRangeAsync(ExpenseAttachmentList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ExpenseFetchAttachmentList;
    }

    public async Task<List<ExpenseFetchTag>> PostTagAsync(List<ExpenseInsertTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ExpenseTag> ExpenseTag = new List<ExpenseTag>();
        List<ExpenseFetchTag> ExpenseFetchTag = new List<ExpenseFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (ExpenseInsertTag item in filedata)
            {
                ExpenseTag.Add(new ExpenseTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
                ExpenseFetchTag.Add(new ExpenseFetchTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
            }

            await _context.ExpenseTag.AddRangeAsync(ExpenseTag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ExpenseFetchTag;
    }


}


public class ExpenseInsertDto : IRequest<RowResponse<ExpenseFetchDto>>
{
    public string TransNum { get; set; } = string.Empty;
    public DateTime TransDate { get; set; }
    public int ContactId { get; set; } = 0;
    public int PaymentMethodId { get; set; } = 0;
    public int PaymentTermId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public int DiscountTypeId { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int AccountCashAndBankId { get; set; } = 0;
    public Boolean PayLater { get; set; } = false;
    public Boolean PriceIncludesTax { get; set; } = false;
    public List<ExpenseAttachmentFiles> AttachmentFile { get; set; }
    public List<ExpenseInsertTag> TagList { get; set; }
}

public class ExpenseAttachmentFiles
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int TransId { get; set; }
}

public class ExpenseInsertTag
{
    public int TagId { get; set; } = 0;
}

