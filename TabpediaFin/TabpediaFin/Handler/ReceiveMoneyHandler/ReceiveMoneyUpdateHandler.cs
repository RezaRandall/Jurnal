using TabpediaFin.Domain;
using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.TransferMoney;
using TabpediaFin.Handler.ReceiveMoneyHandler;

namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyUpdateHandler : IRequestHandler<ReceiveMoneyUpdateDto, RowResponse<ReceiveMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ReceiveMoneyFetchDto>> Handle(ReceiveMoneyUpdateDto request, CancellationToken cancellationToken)
    {
        int receiveMoneyId;
        var result = new RowResponse<ReceiveMoneyFetchDto>();

        List<ReceiveMoneyTag> receiveMoneyTag = new List<ReceiveMoneyTag>();
        List<ReceiveMoneyAttachment> receiveMoneyAttachment = new List<ReceiveMoneyAttachment>();
        List<ReceiveMoneyList> receiveMoneyUpdateList = new List<ReceiveMoneyList>();

        List<ReceiveMoneyFetchTag> receiveMoneyFetchTag = new List<ReceiveMoneyFetchTag>();
        List<ReceiveMoneyFetchAttachment> receiveMoneyFetchAttachment = new List<ReceiveMoneyFetchAttachment>();
        List<ReceiveMoneyFetchList> receiveMoneyFetchList = new List<ReceiveMoneyFetchList>();

        try
        {
            var dataPenerimaUang = await _context.ReceiveMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);            

            var penerima = await _context.Account.FirstAsync(x => x.Id == request.DepositToAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var saldoPenerima = penerima.Balance;

            var totalDiterima = request.TotalAmount;
            var reqReceiveMoney = dataPenerimaUang.TotalAmount;
            double hasilHitung = 0;
            //double hasilHitungb = 0;

            receiveMoneyId = request.Id;
            var id = 0;

            // ACCOUNT CALCULATION
            foreach (ReceiveMoneyUpdateList i in request.ReceiveMoneyListUpdate)
            {
                var amtRequest = i.Amount;

                var pengirim = await _context.Account.FirstAsync(x => x.Id == i.AccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var balanceDepo = pengirim.Balance;
                Int64 balance = 0;
                if (i.Id == 0 || i.Id == null)
                {
                    DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);
                    var receiveMoney = new ReceiveMoneyList()
                    {
                        Id = i.Id,
                        PriceIncludesTax = i.PriceIncludesTax,
                        AccountId = i.AccountId,
                        Description = i.Description,
                        TaxId = i.TaxId,
                        Amount = i.Amount,
                        CreatedUid = _currentUser.UserId,
                        TransId = receiveMoneyId

                    };
                    await _context.ReceiveMoneyList.AddAsync(receiveMoney, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    balance = receiveMoney.Amount;
                }
                else
                {
                    var receiveMoneyListData = await _context.ReceiveMoneyList.AsNoTracking().FirstOrDefaultAsync(x => x.Id == i.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                    balance = receiveMoneyListData.Amount;
                    id = receiveMoneyListData.AccountId;
                }


                if (request.TotalAmount > dataPenerimaUang.TotalAmount)
                {
                    if (balanceDepo == 0)
                    {
                        var hasilBalanceAccount = reqReceiveMoney;
                        hasilHitung = hasilBalanceAccount - amtRequest;
                        pengirim.Balance = hasilHitung;
                        var hasilPengurangan = saldoPenerima - i.Amount;
                        penerima.Balance += hasilPengurangan;
                    }
                    else 
                    {
                        var hasilBalanceAccount = balanceDepo - balance;
                        hasilHitung = hasilBalanceAccount + amtRequest;
                        pengirim.Balance = hasilHitung;
                        var hasilPengurangan = i.Amount - saldoPenerima;
                        penerima.Balance += hasilPengurangan;
                    }
                }

                if (request.TotalAmount < dataPenerimaUang.TotalAmount && request.TotalAmount != 0)
                {
                    if (balanceDepo == 0)
                    {
                        var getBalanceAccount = await _context.Account.FirstAsync(x => x.Id == id && x.TenantId == _currentUser.TenantId, cancellationToken);
                        Double balances = getBalanceAccount.Balance;
                        var hasilBalanceAccount = balance - balances;

                        getBalanceAccount.Balance = hasilBalanceAccount;
                        pengirim.Balance = amtRequest;
                        penerima.Balance -= amtRequest;
                    }
                    else 
                    {
                        var hasilBalanceAccount = balanceDepo - balance;
                        hasilHitung = hasilBalanceAccount + amtRequest;
                        pengirim.Balance = hasilHitung;
                        var hasilPengurangan = saldoPenerima - i.Amount;
                        penerima.Balance -= amtRequest;
                    }
                }

                if (request.TotalAmount == dataPenerimaUang.TotalAmount)
                {
                    var hasilBalanceAccount = balanceDepo - balance;
                    hasilHitung = hasilBalanceAccount + amtRequest;
                    pengirim.Balance = hasilHitung;
                    var hasilPengurangan =  i.Amount - saldoPenerima;
                    penerima.Balance += hasilPengurangan;
                }

            }
            await _context.SaveChangesAsync(cancellationToken);

            // JURNAL CALCULATION
            if (request.TotalAmount > dataPenerimaUang.TotalAmount)
            {
                var backBalanceTrans = request.TotalAmount - dataPenerimaUang.TotalAmount;
                dataPenerimaUang.TotalAmount = reqReceiveMoney + backBalanceTrans;
                dataPenerimaUang.DepositToAccountId = request.DepositToAccountId;
                dataPenerimaUang.PayerId = request.PayerId;
                dataPenerimaUang.TransactionDate = request.TransactionDate;
                dataPenerimaUang.TransactionNo = request.TransactionNo;
                dataPenerimaUang.Memo = request.Memo;
            }
            if (request.TotalAmount < dataPenerimaUang.TotalAmount && request.TotalAmount != 0)
            {
                var backBalanceTrans = dataPenerimaUang.TotalAmount - request.TotalAmount;
                dataPenerimaUang.TotalAmount = reqReceiveMoney - backBalanceTrans;
                dataPenerimaUang.DepositToAccountId = request.DepositToAccountId;
                dataPenerimaUang.PayerId = request.PayerId;
                dataPenerimaUang.TransactionDate = request.TransactionDate;
                dataPenerimaUang.TransactionNo = request.TransactionNo;
                dataPenerimaUang.Memo = request.Memo;
            }
            //if (request.TotalAmount == 0)
            //{
            //    var valSum = saldoPenerima + balanceDepo;
            //    penerima.Balance = valSum;
            //    pengirim.Balance = balanceDepo - balanceDepo;
            //    dataPenerimaUang.DepositToAccountId = request.DepositToAccountId;
            //    dataPenerimaUang.PayerId = request.PayerId;
            //    dataPenerimaUang.TransactionDate = request.TransactionDate;
            //    dataPenerimaUang.TransactionNo = request.TransactionNo;
            //    dataPenerimaUang.Memo = request.Memo;
            //    dataPenerimaUang.TotalAmount = request.TotalAmount;
            //}
            //if (request.TotalAmount == dataPenerimaUang.TotalAmount)
            //{
            //    penerima.Balance = saldoPenerima;
            //    //pengirim.Balance = balanceDepo;
            //    dataPenerimaUang.DepositToAccountId = request.DepositToAccountId;
            //    dataPenerimaUang.PayerId = request.PayerId;
            //    dataPenerimaUang.TransactionDate = request.TransactionDate;
            //    dataPenerimaUang.TransactionNo = request.TransactionNo;
            //    dataPenerimaUang.Memo = request.Memo;
            //    dataPenerimaUang.TotalAmount = request.TotalAmount;
            //}
            if (request.TotalAmount == 0) // || pengirim.Balance == 0
            {
                result.IsOk = false;
                result.ErrorMessage = "Transaction account lines must not be blank / is invalid, Failed!";
                return result;
            }

            await _context.SaveChangesAsync(cancellationToken);


            // CHANGE LIST OF RECEIVE MONEY ITEMS
            List<int> idUpdateReceiveMoneyTag = new List<int>();
            List<int> idUpdateReceiveMoneyAttachment = new List<int>();
            List<int> idUpdateReceiveMoneyList = new List<int>();

            if (request.ReceiveMoneyTagList.Count > 0)
            {
                foreach (ReceiveMoneyUpdateTag i in request.ReceiveMoneyTagList)
                {
                    idUpdateReceiveMoneyTag.Add(i.Id);
                    receiveMoneyTag.Add(new ReceiveMoneyTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = receiveMoneyId,
                        CreatedUid = _currentUser.UserId
                    });
                    receiveMoneyFetchTag.Add(new ReceiveMoneyFetchTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = receiveMoneyId
                    });
                }
                _context.ReceiveMoneyTag.UpdateRange(receiveMoneyTag);
            }

            if (request.ReceiveMoneyAttachmentFile.Count > 0)
            {
                foreach (ReceiveMoneyAttachmentUpdate i in request.ReceiveMoneyAttachmentFile)
                {
                    idUpdateReceiveMoneyAttachment.Add(i.Id);
                    receiveMoneyAttachment.Add(new ReceiveMoneyAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        TransId = receiveMoneyId
                    });
                    receiveMoneyFetchAttachment.Add(new ReceiveMoneyFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        TransId = receiveMoneyId
                    });
                }
                _context.ReceiveMoneyAttachment.UpdateRange(receiveMoneyAttachment);
            }

            if (request.ReceiveMoneyListUpdate.Count > 0)
            {
                foreach (ReceiveMoneyUpdateList i in request.ReceiveMoneyListUpdate)
                {
                    idUpdateReceiveMoneyList.Add(i.Id);
                    receiveMoneyUpdateList.Add(new ReceiveMoneyList
                    {
                        Id = i.Id,
                        PriceIncludesTax = i.PriceIncludesTax,
                        AccountId = i.AccountId,
                        Description = i.Description,
                        TaxId = i.TaxId,
                        Amount = i.Amount,
                        CreatedUid = _currentUser.UserId,
                        TransId = receiveMoneyId
                    });
                    receiveMoneyFetchList.Add(new ReceiveMoneyFetchList
                    {
                        Id = i.Id,
                        AccountId = i.AccountId,
                        PriceIncludesTax = i.PriceIncludesTax,
                        Description = i.Description,
                        TaxId = i.TaxId,
                        Amount = i.Amount,
                        TransId = receiveMoneyId
                    });
                }
                _context.ReceiveMoneyList.UpdateRange(receiveMoneyUpdateList);
            }

            List<ReceiveMoneyTag> receiveMoneyTagList = _context.ReceiveMoneyTag.Where<ReceiveMoneyTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateReceiveMoneyTag.Contains(x.Id)).ToList();
            List<ReceiveMoneyAttachment> receiveMoneyAttachmentList = _context.ReceiveMoneyAttachment.Where<ReceiveMoneyAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateReceiveMoneyAttachment.Contains(x.Id)).ToList();
            List<ReceiveMoneyList> receiveMoneyList = _context.ReceiveMoneyList.Where<ReceiveMoneyList>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateReceiveMoneyList.Contains(x.Id)).ToList();
            _context.ReceiveMoneyTag.RemoveRange(receiveMoneyTagList);
            _context.ReceiveMoneyAttachment.RemoveRange(receiveMoneyAttachmentList);
            _context.ReceiveMoneyList.RemoveRange(receiveMoneyList);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ReceiveMoneyFetchDto()
            {
                Id = request.Id,
                DepositToAccountId = request.DepositToAccountId,
                PayerId = request.PayerId,
                TransactionDate = request.TransactionDate,
                TransactionNo = request.TransactionNo,
                Memo = request.Memo,
                TotalAmount = request.TotalAmount,
                ReceiveMoneyTagList = receiveMoneyFetchTag,
                ReceiveMoneyAttachmentList = receiveMoneyFetchAttachment,
                ReceiveMoneyList = receiveMoneyFetchList
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

public class ReceiveMoneyUpdateDto : IRequest<RowResponse<ReceiveMoneyFetchDto>>
{
    public int Id { get; set; }
    public int DepositToAccountId { get; set; } = 0;
    public int PayerId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public Int64 TotalAmount { get; set; } = 0;
    public List<ReceiveMoneyAttachmentUpdate> ReceiveMoneyAttachmentFile { get; set; }
    public List<ReceiveMoneyUpdateTag> ReceiveMoneyTagList { get; set; }
    public List<ReceiveMoneyUpdateList> ReceiveMoneyListUpdate { get; set; }
}

public class ReceiveMoneyAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class ReceiveMoneyUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}

public class ReceiveMoneyUpdateList
{
    public int Id { get; set; }
    public int AccountId { get; set; } = 0;
    public bool PriceIncludesTax { get; set; } = false;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
}