using TabpediaFin.Domain;
using TabpediaFin.Domain.Expense;
using TabpediaFin.Handler.Item;
using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

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
        int expenseId;
        List<ExpenseTag> expenseTag = new List<ExpenseTag>();
        List<ExpenseAttachment> expenseAttachment = new List<ExpenseAttachment>();
        List<ExpenseList> expenseUpdateList = new List<ExpenseList>();

        List<ExpenseFetchTag> expenseFetchTag = new List<ExpenseFetchTag>();
        List<ExpenseFetchAttachment> expenseFetchAttachment = new List<ExpenseFetchAttachment>();
        List<ExpenseFetchList> expenseFetchList = new List<ExpenseFetchList>();

        try
        {
            var expenses = await _context.Expense.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            expenses.PayFromAccountId = request.PayFromAccountId;
            expenses.PayLater = request.PayLater;
            expenses.RecipientContactId = request.RecipientContactId;
            expenses.TransactionDate = request.TransactionDate;
            expenses.PaymentMethodId = request.PaymentMethodId;
            expenses.TransactionNo = request.TransactionNo;
            expenses.BillingAddress = request.BillingAddress;
            expenses.DueDate = request.DueDate;
            expenses.PaymentTermId = request.PaymentTermId;
            expenses.Memo = request.Memo;
            expenses.Status = request.Status;
            expenses.DiscountPercent = request.DiscountPercent;
            expenses.DiscountAmount = request.DiscountAmount;
            expenses.TotalAmount = request.TotalAmount;

            expenseId = request.Id;
            List<int> idUpdateExpenseTag = new List<int>();
            List<int> idUpdateExpenseAttachment = new List<int>();
            List<int> idUpdateExpenseList = new List<int>();


            if (request.ExpenseTagList.Count > 0)
            {
                foreach (ExpenseUpdateTag expTag in request.ExpenseTagList)
                {
                    idUpdateExpenseTag.Add(expTag.Id);
                    expenseTag.Add(new ExpenseTag
                    {
                        Id = expTag.Id,
                        TagId = expTag.TagId,
                        TransId = expenseId,
                        CreatedUid = _currentUser.UserId
                    });
                    expenseFetchTag.Add(new ExpenseFetchTag
                    {
                        Id = expTag.Id,
                        TagId = expTag.TagId,
                        TransId = expenseId
                    });
                }
                _context.ExpenseTag.UpdateRange(expenseTag);
            }

            if (request.AttachmentFile.Count > 0)
            {
                foreach (ExpenseAttachmentUpdate i in request.AttachmentFile)
                {
                    idUpdateExpenseAttachment.Add(i.Id);
                    expenseAttachment.Add(new ExpenseAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        TransId = expenseId
                    });
                    expenseFetchAttachment.Add(new ExpenseFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        TransId = expenseId
                    });
                }
                _context.ExpenseAttachment.UpdateRange(expenseAttachment);
            }

            List<ExpenseTag> expenseTagList = _context.ExpenseTag.Where<ExpenseTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateExpenseTag.Contains(x.Id)).ToList();
            List<ExpenseAttachment> expenseAttachmentList = _context.ExpenseAttachment.Where<ExpenseAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateExpenseAttachment.Contains(x.Id)).ToList();
            List<ExpenseList> expenseList = _context.ExpenseList.Where<ExpenseList>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateExpenseList.Contains(x.Id)).ToList();
            _context.ExpenseTag.RemoveRange(expenseTagList);
            _context.ExpenseAttachment.RemoveRange(expenseAttachmentList);
            _context.ExpenseList.RemoveRange(expenseList);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ExpenseFetchDto()
            {
                Id = request.Id,
                PayFromAccountId = request.PayFromAccountId,
                PayLater = request.PayLater,
                RecipientContactId = request.RecipientContactId,
                TransactionDate = request.TransactionDate,
                PaymentMethodId = request.PaymentMethodId,
                TransactionNo = request.TransactionNo,
                BillingAddress = request.BillingAddress,
                DueDate = request.DueDate,
                PaymentTermId = request.PaymentTermId,
                Memo = request.Memo,
                Status = request.Status,
                DiscountPercent = request.DiscountPercent,
                DiscountAmount = request.DiscountAmount,
                TotalAmount = request.TotalAmount,
                ExpenseTagList = expenseFetchTag,
                ExpenseAttachmentList = expenseFetchAttachment,
                ExpenseFetchList = expenseFetchList
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
    public Int64 TotalAmount { get; set; } = 0;
    public List<ExpenseAttachmentUpdate> AttachmentFile { get; set; }
    public List<ExpenseUpdateTag> ExpenseTagList { get; set; }
    public List<ExpenseUpdateList> ExpenseUpdateList { get; set; }
}

public class ExpenseAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}
public class ExpenseUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}

public class ExpenseUpdateList
{
    public int Id { get; set; }
    public bool PriceIncludesTax { get; set; } = false;
    public int ExpenseAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
}