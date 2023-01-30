using TabpediaFin.Domain.Expense;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseUpdatePayBillHandler : IRequestHandler<ExpenseUpdatePayBillDto, RowResponse<ExpenseFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseUpdatePayBillHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(ExpenseUpdatePayBillDto request, CancellationToken cancellationToken)
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
            var dataPengirimUang = await _context.Expense.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId && x.PayLater == true, cancellationToken);
            var reqBalanceDue = dataPengirimUang.BalanceDue;
            expenseId = request.Id;
            var payLater = dataPengirimUang.PayLater;
            var paymentMethodId = dataPengirimUang.PaymentMethodId;
            var billingAddress = dataPengirimUang.BillingAddress;
            var dueDate = dataPengirimUang.DueDate;
            var paymentTermId = dataPengirimUang.PaymentTermId;
            Int64 payBill = 0;
            Int64 totalAmount = dataPengirimUang.TotalAmount;
            //var expenseListData = await _context.ExpenseList.AsNoTracking().Where(x => x.TransId == expenseId && x.TenantId == _currentUser.TenantId).ToListAsync();
            //foreach (var itm in expenseListData)
            //{

            //}
            //var expenseListData = await _context.Expense.AsNoTracking().Where(x => x.Id == expenseId && x.TenantId == _currentUser.TenantId && x.PayLater == true).ToListAsync();
            foreach (var itm in request.ExpenseList)
            {
                payBill = reqBalanceDue - itm.Amount;
                dataPengirimUang.BalanceDue = payBill;
            }

            // JURNAL COUNT
            if (request.TotalAmount == dataPengirimUang.TotalAmount)
            {
                if (request.WitholdingAmount != 0)
                {
                    var discount = await _context.Account.FirstAsync(x => x.Id == request.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                    discount.Balance += request.WitholdingAmount;
                }
            }
            else if (request.TotalAmount < dataPengirimUang.TotalAmount)
            {
                var backBalanceTransaction = dataPengirimUang.TotalAmount - request.TotalAmount;
                dataPengirimUang.TotalAmount = reqBalanceDue - backBalanceTransaction;

                if (request.WitholdingAmount != 0)
                {
                    var discount = await _context.Account.FirstAsync(x => x.Id == request.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                    discount.Balance += backBalanceTransaction;
                }
            }
            else
            {
                var backBalanceTransaction = request.TotalAmount - dataPengirimUang.TotalAmount;
                dataPengirimUang.TotalAmount = reqBalanceDue + backBalanceTransaction;

                if (request.WitholdingAmount != 0)
                {
                    var discount = await _context.Account.FirstAsync(x => x.Id == request.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                    discount.Balance += backBalanceTransaction;
                }
            }
            if (request.TotalAmount == 0) // || pengirim.Balance == 0
            {
                result.IsOk = false;
                result.ErrorMessage = "Transaction account lines must not be blank / is invalid, Failed!";
                return result;
            }

            dataPengirimUang.PayFromAccountId = request.PayFromAccountId;
            //dataPengirimUang.PayLater = request.PayLater;
            dataPengirimUang.TransactionDate = request.TransactionDate;
            //dataPengirimUang.PaymentMethodId = request.PaymentMethodId;
            dataPengirimUang.TransactionNo = request.TransactionNo;
            //dataPengirimUang.BillingAddress = request.BillingAddress;
            //dataPengirimUang.DueDate = request.DueDate;
            //dataPengirimUang.PaymentTermId = request.PaymentTermId;
            dataPengirimUang.Memo = request.Memo;
            dataPengirimUang.Status = request.Status;
            dataPengirimUang.DiscountPercent = request.DiscountPercent;
            dataPengirimUang.DiscountAmount = request.DiscountAmount;
            dataPengirimUang.DiscountForAccountId = request.DiscountForAccountId;
            dataPengirimUang.TotalAmount = request.TotalAmount;
            //dataPengirimUang.BalanceDue = request.BalanceDue;
            dataPengirimUang.WitholdingAmount = request.WitholdingAmount;

            List<int> idUpdateExpenseTag = new List<int>();
            List<int> idUpdateExpenseAttachment = new List<int>();
            List<int> idUpdateExpenseList = new List<int>();

            // ATTACHMENT
            if (request.AttachmentFile.Count > 0)
            {
                foreach (ExpenseAttachmentUpdateBill i in request.AttachmentFile)
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

            // TAG
            if (request.ExpenseTagList.Count > 0)
            {
                foreach (ExpenseUpdateBillTag expTag in request.ExpenseTagList)
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

            // EXPENSE LIST
            if (request.ExpenseList.Count > 0)
            {
                foreach (ExpenseUpdateBillList expList in request.ExpenseList)
                {
                    idUpdateExpenseList.Add(expList.Id);
                    expenseUpdateList.Add(new ExpenseList
                    {
                        Id = expList.Id,
                        //PriceIncludesTax = expList.PriceIncludesTax,
                        ExpenseAccountId = expList.ExpenseAccountId,
                        Description = expList.Description,
                        //TaxId = expList.TaxId,
                        Amount = totalAmount,
                        TransId = expenseId,
                        CreatedUid = _currentUser.UserId
                    });
                    expenseFetchList.Add(new ExpenseFetchList
                    {
                        Id = expList.Id,
                        //PriceIncludesTax = expList.PriceIncludesTax,
                        ExpenseAccountId = expList.ExpenseAccountId,
                        Description = expList.Description,
                        //TaxId = expList.TaxId,
                        Amount = totalAmount,
                        TransId = expenseId
                    });
                }
                _context.ExpenseList.UpdateRange(expenseUpdateList);
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
                PayLater = payLater,
                RecipientContactId = request.RecipientContactId,
                TransactionDate = request.TransactionDate,
                PaymentMethodId = paymentMethodId,
                TransactionNo = request.TransactionNo,
                BillingAddress = billingAddress,
                DueDate = dueDate,
                PaymentTermId = paymentTermId,
                Memo = request.Memo,
                Status = request.Status,
                DiscountPercent = request.DiscountPercent,
                DiscountAmount = request.DiscountAmount,
                DiscountForAccountId = request.DiscountForAccountId,
                TotalAmount = request.TotalAmount,
                BalanceDue = payBill,
                WitholdingAmount = request.WitholdingAmount,
                ExpenseTagList = expenseFetchTag,
                ExpenseAttachmentList = expenseFetchAttachment,
                ExpenseFetchList = expenseFetchList
            };
            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch(Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }
}

public class ExpenseUpdatePayBillDto : IRequest<RowResponse<ExpenseFetchDto>>
{
    public int Id { get; set; }
    public int PayFromAccountId { get; set; } = 0;
    //public Boolean PayLater { get; set; } = false;
    public int RecipientContactId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    //public int PaymentMethodId { get; set; } = 0;
    public string TransactionNo { get; set; } = string.Empty;
    //public string BillingAddress { get; set; } = string.Empty;
    //public DateTime? DueDate { get; set; }
    //public int PaymentTermId { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int Status { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public int DiscountForAccountId { get; set; } = 0;
    public Int64 TotalAmount { get; set; } = 0;
    public Int64 BalanceDue { get; set; } = 0;
    public Int64 WitholdingAmount { get; set; } = 0;
    public List<ExpenseAttachmentUpdateBill> AttachmentFile { get; set; }
    public List<ExpenseUpdateBillTag> ExpenseTagList { get; set; }
    public List<ExpenseUpdateBillList> ExpenseList { get; set; }
}
public class ExpenseAttachmentUpdateBill
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}
public class ExpenseUpdateBillTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}

public class ExpenseUpdateBillList
{
    public int Id { get; set; }
    //public bool PriceIncludesTax { get; set; } = false;
    public int ExpenseAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    //public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
}