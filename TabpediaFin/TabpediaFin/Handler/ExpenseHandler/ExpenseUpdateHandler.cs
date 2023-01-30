using TabpediaFin.Domain.Expense;

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
            var dataPengirimUang = await _context.Expense.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            var reqSendMoney = dataPengirimUang.TotalAmount;
            double witholdingAmt = dataPengirimUang.WitholdingAmount;
            double discountAmt = witholdingAmt;
            double jumlah = 0;
            var witholdingId = dataPengirimUang.DiscountForAccountId;

            // BAYAR DARI AKUN / cek data akun
            var dataAkun = await _context.Account.FirstAsync(x => x.Id == request.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var saldoPengirim = dataAkun.Balance;
            expenseId = request.Id;


            var expenseListData = await _context.ExpenseList.AsNoTracking().Where(x => x.TransId == expenseId && x.TenantId == _currentUser.TenantId).ToListAsync();
            foreach (var itm in expenseListData)
            {
                //PENGEMBALIAN
                if (discountAmt != 0)
                {
                    var getSaldo = await _context.Account.FirstAsync(x => x.Id == witholdingId && x.TenantId == _currentUser.TenantId, cancellationToken);
                    var saldo = getSaldo.Balance;
                    var value = saldo - witholdingAmt;

                    discountAmt = value;
                    jumlah = reqSendMoney + witholdingAmt;
                    getSaldo.Balance = value;

                }
                var getBalaces = await _context.Account.FirstAsync(x => x.Id == itm.ExpenseAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var nilaiBalanceAccounts = getBalaces.Balance;
                var hasil = nilaiBalanceAccounts - itm.Amount;
                getBalaces.Balance = hasil;
            }
            if (witholdingAmt != 0)
            {
                var getValue = await _context.Account.FirstAsync(x => x.Id == request.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var nilai = getValue.Balance + jumlah;
                getValue.Balance = nilai;
            }
            else
            {
                var getValue = await _context.Account.FirstAsync(x => x.Id == request.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var nilai = getValue.Balance + reqSendMoney;
                getValue.Balance = nilai;
            }

            // COUNT IN EVERY ACCOUNTS
            foreach (ExpenseUpdateList i in request.ExpenseList)
            {
                if (i.Id == 0)
                {
                    DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);
                    var expense = new ExpenseList()
                    {
                        Id = i.Id,
                        PriceIncludesTax = i.PriceIncludesTax,
                        ExpenseAccountId = i.ExpenseAccountId,
                        Description = i.Description,
                        TaxId = i.TaxId,
                        Amount = i.Amount,
                        CreatedUid = _currentUser.UserId,
                        TransId = expenseId
                    };
                    await _context.ExpenseList.AddAsync(expense, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // ambil nilai balance dari akun berdasarkan send money list
                var getBalace = await _context.Account.FirstAsync(x => x.Id == i.ExpenseAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var nilaiBalanceAccount = getBalace.Balance;

                double nilaiKembali = 0;

                var hasil = nilaiBalanceAccount + i.Amount;
                getBalace.Balance = hasil;

                nilaiKembali = dataAkun.Balance - i.Amount;
                dataAkun.Balance = nilaiKembali;

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
            dataPengirimUang.TotalAmount = reqSendMoney - backBalanceTransaction;

            if (request.WitholdingAmount != 0)
            {
                var discount = await _context.Account.FirstAsync(x => x.Id == request.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                discount.Balance += backBalanceTransaction;
            }
        }
        else
        {
            var backBalanceTransaction = request.TotalAmount - dataPengirimUang.TotalAmount;
            dataPengirimUang.TotalAmount = reqSendMoney + backBalanceTransaction;

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
            dataPengirimUang.PayLater = request.PayLater;
            dataPengirimUang.RecipientContactId = request.RecipientContactId;
            dataPengirimUang.TransactionDate = request.TransactionDate;
            dataPengirimUang.PaymentMethodId = request.PaymentMethodId;
            dataPengirimUang.TransactionNo = request.TransactionNo;
            dataPengirimUang.BillingAddress = request.BillingAddress;
            dataPengirimUang.DueDate = request.DueDate;
            dataPengirimUang.PaymentTermId = 0;
            dataPengirimUang.Memo = request.Memo;
            dataPengirimUang.Status = request.Status;
            dataPengirimUang.DiscountPercent = request.DiscountPercent;
            dataPengirimUang.DiscountAmount = request.DiscountAmount;
            dataPengirimUang.DiscountForAccountId = request.DiscountForAccountId;
            dataPengirimUang.WitholdingAmount = request.WitholdingAmount;

            expenseId = request.Id;
            List<int> idUpdateExpenseTag = new List<int>();
            List<int> idUpdateExpenseAttachment = new List<int>();
            List<int> idUpdateExpenseList = new List<int>();

            // ATTACHMENT
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

            // TAG
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

            // EXPENSE LIST
            if (request.ExpenseList.Count > 0)
            {
                foreach (ExpenseUpdateList expList in request.ExpenseList)
                {
                    idUpdateExpenseList.Add(expList.Id);
                    expenseUpdateList.Add(new ExpenseList
                    {
                        Id = expList.Id,
                        PriceIncludesTax = expList.PriceIncludesTax,
                        ExpenseAccountId = expList.ExpenseAccountId,
                        Description = expList.Description,
                        TaxId = expList.TaxId,
                        Amount = expList.Amount,
                        TransId = expenseId,
                        CreatedUid = _currentUser.UserId
                    });
                    expenseFetchList.Add(new ExpenseFetchList
                    {
                        Id = expList.Id,
                        PriceIncludesTax = expList.PriceIncludesTax,
                        ExpenseAccountId = expList.ExpenseAccountId,
                        Description = expList.Description,
                        TaxId = expList.TaxId,
                        Amount = expList.Amount,
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
                PayLater = request.PayLater,
                RecipientContactId = request.RecipientContactId,
                TransactionDate = request.TransactionDate,
                PaymentMethodId = request.PaymentMethodId,
                TransactionNo = request.TransactionNo,
                BillingAddress = request.BillingAddress,
                DueDate = request.DueDate,
                PaymentTermId = 0,
                Memo = request.Memo,
                Status = request.Status,
                DiscountPercent = request.DiscountPercent,
                DiscountAmount = request.DiscountAmount,
                DiscountForAccountId = request.DiscountForAccountId,
                TotalAmount = request.TotalAmount,
                WitholdingAmount = request.WitholdingAmount,
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
    public DateTime? DueDate { get; set; }
    public int PaymentTermId { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int Status { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public int DiscountForAccountId { get; set; } = 0;
    public Int64 TotalAmount { get; set; } = 0;
    public Int64 WitholdingAmount { get; set; } = 0;
    public List<ExpenseAttachmentUpdate> AttachmentFile { get; set; }
    public List<ExpenseUpdateTag> ExpenseTagList { get; set; }
    public List<ExpenseUpdateList> ExpenseList { get; set; }
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