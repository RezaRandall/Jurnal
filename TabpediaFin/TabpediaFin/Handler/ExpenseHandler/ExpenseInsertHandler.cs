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
        Int64 RequestAmountTotal;
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);

        var expense = new Expense()
        {
            PayFromAccountId = request.PayFromAccountId,
            PayLater = request.PayLater,
            RecipientContactId = request.RecipientContactId,
            TransactionDate = TransDate,
            PaymentMethodId = request.PaymentMethodId,
            TransactionNo = request.TransactionNo,
            BillingAddress = request.BillingAddress,
            DueDate = request.DueDate,
            PaymentTermId = request.PaymentTermId,
            Memo = request.Memo,
            Status = request.Status,
            DiscountPercent = request.DiscountPercent,
            DiscountAmount = request.DiscountAmount,
            DiscountForAccountId = request.DiscountForAccountId,
            TotalAmount = request.TotalAmount,
            WitholdingAmount = request.WitholdingAmount
        };

        try
        {
            await _context.Expense.AddAsync(expense, cancellationToken);
            RequestAmountTotal = expense.TotalAmount;

            double jumlah = expense.TotalAmount + expense.WitholdingAmount;

            // BALANCE REDUCTION FROM PAYEER ACCOUNT
            if (request.WitholdingAmount == 0)
            {
                var accountPayeer = await _context.Account.FirstAsync(x => x.Id == request.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var reductionAmt = accountPayeer.Balance - RequestAmountTotal;
                accountPayeer.Balance = reductionAmt;
            }
            else
            {
                var accountPayeer = await _context.Account.FirstAsync(x => x.Id == request.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var reduction = accountPayeer.Balance - jumlah;
                accountPayeer.Balance = reduction;
            }

            // WHEN INCLUDE WITHOLDING
            if (request.DiscountAmount != 0 || request.DiscountPercent != 0 || request.DiscountForAccountId != 0)
            {
                var discountInput = await _context.Account.FirstAsync(x => x.Id == request.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var countWitholding = discountInput.Balance + request.WitholdingAmount;
                discountInput.Balance = countWitholding;
            }

            // BALANCE SUM FROM RECIPIENT ACCOUNT
            foreach (ExpenseInsertList i in request.ExpenseList)
            {
                var paymentForAccountId = await _context.Account.FirstAsync(x => x.Id == i.ExpenseAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var sumBalance = paymentForAccountId.Balance + i.Amount;
                paymentForAccountId.Balance = sumBalance;
            }

            await _context.SaveChangesAsync(cancellationToken);
            transIdResult = expense.Id;

            List<ExpenseFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transIdResult, cancellationToken);
            List<ExpenseFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);
            List<ExpenseFetchList> ExpenseListResult = await PostExpenseListAsync(request.ExpenseList, transIdResult, cancellationToken);

            var row = new ExpenseFetchDto()
            {
                Id = expense.Id,
                PayFromAccountId = expense.PayFromAccountId,
                PayLater = expense.PayLater,
                RecipientContactId = expense.RecipientContactId,
                TransactionDate = expense.TransactionDate,
                PaymentMethodId = expense.PaymentMethodId,
                TransactionNo = expense.TransactionNo,
                BillingAddress = expense.BillingAddress,
                DueDate = expense.DueDate,
                PaymentTermId = expense.PaymentTermId,
                Memo = expense.Memo,
                Status = expense.Status,
                DiscountPercent = expense.DiscountPercent,
                DiscountAmount = expense.DiscountAmount,
                DiscountForAccountId = expense.DiscountForAccountId,
                TotalAmount = expense.TotalAmount,
                WitholdingAmount = expense.WitholdingAmount
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

    public async Task<List<ExpenseFetchList>> PostExpenseListAsync(List<ExpenseInsertList> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ExpenseList> ExpenseList = new List<ExpenseList>();
        List<ExpenseFetchList> ExpenseFetchList = new List<ExpenseFetchList>();

        if (filedata.Count > 0)
        {
            foreach (ExpenseInsertList item in filedata)
            {
                ExpenseList.Add(new ExpenseList
                {
                    PriceIncludesTax = item.PriceIncludesTax,
                    ExpenseAccountId = item.ExpenseAccountId,
                    Description = item.Description,
                    TaxId = item.TaxId,
                    Amount = item.Amount,
                    TransId = TransId
                });
                ExpenseFetchList.Add(new ExpenseFetchList
                {
                    PriceIncludesTax = item.PriceIncludesTax,
                    ExpenseAccountId = item.ExpenseAccountId,
                    Description = item.Description,
                    TaxId = item.TaxId,
                    Amount = item.Amount,
                    TransId = TransId
                });
            }
            await _context.ExpenseList.AddRangeAsync(ExpenseList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return ExpenseFetchList;
    }

}


public class ExpenseInsertDto : IRequest<RowResponse<ExpenseFetchDto>>
{
    public int PayFromAccountId { get; set; } = 0;
    public Boolean PayLater { get; set; } = false;
    public int RecipientContactId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public int PaymentMethodId { get; set; } = 0;
    public string TransactionNo { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int PaymentTermId { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int Status { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public int DiscountForAccountId { get; set; } = 0;
    public Int64 TotalAmount { get; set; } = 0;
    public Int64 WitholdingAmount { get; set; } = 0;
    public List<ExpenseAttachmentFiles> AttachmentFile { get; set; }
    public List<ExpenseInsertTag> TagList { get; set; }
    public List<ExpenseInsertList> ExpenseList { get; set; }
}

public class ExpenseAttachmentFiles
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class ExpenseInsertTag
{
    public int TagId { get; set; } = 0;
}

public class ExpenseInsertList
{
    public bool PriceIncludesTax { get; set; } = false;
    public int ExpenseAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
}
