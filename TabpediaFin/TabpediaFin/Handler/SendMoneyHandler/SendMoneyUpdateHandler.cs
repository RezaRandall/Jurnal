using TabpediaFin.Domain.SendMoney;

namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyUpdateHandler : IRequestHandler<SendMoneyUpdateDto, RowResponse<SendMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public SendMoneyUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<SendMoneyFetchDto>> Handle(SendMoneyUpdateDto request, CancellationToken cancellationToken)
    {
        int sendMoneyId;
        var result = new RowResponse<SendMoneyFetchDto>();

        List<SendMoneyTag> sendMoneyTag = new List<SendMoneyTag>();
        List<SendMoneyAttachment> sendMoneyAttachment = new List<SendMoneyAttachment>();
        List<SendMoneyList> sendMoneyUpdateList = new List<SendMoneyList>();


        List<SendMoneyFetchTag> sendMoneyFetchTag = new List<SendMoneyFetchTag>();
        List<SendMoneyFetchAttachment> sendMoneyFetchAttachment = new List<SendMoneyFetchAttachment>();
        List<SendMoneyFetchList> sendMoneyFetchList = new List<SendMoneyFetchList>();

        try
        {

            var dataPengirimUang = await _context.SendMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            
            var dataAkun = await _context.Account.FirstAsync(x => x.Id == request.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var saldoPengirim = dataAkun.Balance;

            sendMoneyId = request.Id;
            double hasilHitung = 0;
            foreach (SendMoneyUpdateList i in request.SendMoneyList)
            {
                // ambil nilai balance dari akun berdasarkan send money list
                var getBalance = await _context.Account.FirstAsync(x => x.Id == i.AccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var nilaiBalanceAccount = getBalance.Balance;
                var balanceNol = getBalance.Balance;
                Int64 balance = 0;
                var accountId = 0;
                if (i.Id == 0 || i.Id == null)
                {
                    DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);
                    var sentMoney = new SendMoneyList()
                    {
                        Id = i.Id,
                        PriceIncludesTax = i.PriceIncludesTax,
                        AccountId = i.AccountId,
                        Description = i.Description,
                        TaxId = i.TaxId,
                        Amount = i.Amount,
                        CreatedUid = _currentUser.UserId,
                        TransId = sendMoneyId
                    };
                    await _context.SendMoneyList.AddAsync(sentMoney, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    balance = sentMoney.Amount;
                }
                else 
                {
                    //var sendMoneyListData = await _context.SendMoneyList.FirstAsync(x => x.TransId == sendMoneyId && x.TenantId == _currentUser.TenantId, cancellationToken);
                    //var sendMoneyListDatas = await _context.SendMoneyList.AsNoTracking().lis
                    //List<SendMoneyUpdateList> SendMoneyList = _context.SendMoneyList.Where<SendMoneyList>(x => x.TransId == sendMoneyId && x.TenantId == _currentUser.TenantId).ToList();
                    var sendMoneyListData = await _context.SendMoneyList.AsNoTracking().Where(x => x.TransId == sendMoneyId && x.TenantId == _currentUser.TenantId).ToListAsync();
                    //balance = sendMoneyListData.Amount;
                    //accountId = sendMoneyListData.AccountId;

                    foreach (var itm in sendMoneyListData)
                    {
                        if (balanceNol == 0)
                        {
                            getBalance.Balance = i.Amount;

                            //var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                            //hasilHitung = hasilRefund - request.TotalAmount;
                            dataAkun.Balance = hasilHitung;
                        }
                        else
                        {
                            var accounts = await _context.Account.AsNoTracking().FirstOrDefaultAsync(x => x.Id == itm.AccountId && x.TenantId == _currentUser.TenantId, cancellationToken);

                            //var pengembalian = nilaiBalanceAccount + itm.Amount;
                            hasilHitung = accounts.Balance - itm.Amount;
                            //getBalance.Balance = hasilHitung;
                            accounts.Balance = hasilHitung;
                        }
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                    hasilHitung = hasilRefund - request.TotalAmount;
                    dataAkun.Balance = hasilHitung;
                }

                // perhitungan balance dari setiap account
                //if (request.TotalAmount > dataPengirimUang.TotalAmount)
                //{
                //    if (balanceNol == 0)
                //    {
                //        getBalance.Balance = i.Amount;
                //        var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                //        hasilHitung = hasilRefund - request.TotalAmount;
                //        dataAkun.Balance = hasilHitung;
                //    }
                //    else
                //    {
                //        var pengembalian = nilaiBalanceAccount + dataPengirimUang.TotalAmount;
                //        hasilHitung = pengembalian - i.Amount;
                //        getBalance.Balance = hasilHitung;

                //        var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                //        hasilHitung = hasilRefund - request.TotalAmount;
                //        dataAkun.Balance = hasilHitung;
                //    }

                //}
                //if (request.TotalAmount < dataPengirimUang.TotalAmount)
                //{
                //    if (balanceNol == 0)
                //    {
                //        getBalance.Balance = i.Amount;

                //        var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                //        hasilHitung = hasilRefund - request.TotalAmount;
                //        dataAkun.Balance = hasilHitung;
                //    }
                //    else 
                //    {
                //        var pengembalian = nilaiBalanceAccount + dataPengirimUang.TotalAmount;
                //        hasilHitung = pengembalian - i.Amount;
                //        getBalance.Balance = hasilHitung;

                //        var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                //        hasilHitung = hasilRefund - request.TotalAmount;
                //        dataAkun.Balance = hasilHitung;
                //    }
                //}
                //if (request.TotalAmount == dataPengirimUang.TotalAmount)
                //{
                //    if (balanceNol == 0)
                //    {
                //        getBalance.Balance = i.Amount;

                //        var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                //        hasilHitung = hasilRefund - request.TotalAmount;
                //        dataAkun.Balance = hasilHitung;
                //    }
                //    else 
                //    {


                //        var pengembalian = nilaiBalanceAccount + balance;
                //        hasilHitung = pengembalian - i.Amount;
                //        getBalance.Balance = hasilHitung;

                //        var hasilRefund = saldoPengirim + dataPengirimUang.TotalAmount;
                //        hasilHitung = hasilRefund - request.TotalAmount;
                //        dataAkun.Balance = hasilHitung;
                //    }

                //}

            }
            await _context.SaveChangesAsync(cancellationToken);

            dataPengirimUang.PayFromAccountId = request.PayFromAccountId;
            dataPengirimUang.RecipientContactId = request.RecipientContactId;
            dataPengirimUang.TransactionDate = request.TransactionDate;
            dataPengirimUang.TransactionNo = request.TransactionNo;
            dataPengirimUang.Memo = request.Memo;
            dataPengirimUang.TotalAmount = request.TotalAmount;
            dataPengirimUang.WitholdingAmount = request.WitholdingAmount;
            dataPengirimUang.DiscountAmount = request.DiscountAmount;
            dataPengirimUang.DiscountPercent = request.DiscountPercent;
            dataPengirimUang.DiscountForAccountId = request.DiscountForAccountId;

            await _context.SaveChangesAsync(cancellationToken);

            
            List<int> idUpdateSendMoneyTag = new List<int>();
            List<int> idUpdateSendMoneyAttachment = new List<int>();
            List<int> idUpdateSendMoneyList = new List<int>();

            if (request.SendMoneyTagList.Count > 0)
            {
                foreach (SendMoneyUpdateTag i in request.SendMoneyTagList)
                {
                    idUpdateSendMoneyTag.Add(i.Id);
                    sendMoneyTag.Add(new SendMoneyTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = sendMoneyId,
                        CreatedUid = _currentUser.UserId
                    });
                    sendMoneyFetchTag.Add(new SendMoneyFetchTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = sendMoneyId
                    });
                }
                _context.SendMoneyTag.UpdateRange(sendMoneyTag);
            }

            if (request.SendMoneyAttachmentFile.Count > 0)
            {
                foreach (SendMoneyAttachmentUpdate i in request.SendMoneyAttachmentFile)
                {
                    idUpdateSendMoneyAttachment.Add(i.Id);
                    sendMoneyAttachment.Add(new SendMoneyAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        TransId = sendMoneyId
                    });
                    sendMoneyFetchAttachment.Add(new SendMoneyFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        TransId = sendMoneyId
                    });
                }
                _context.SendMoneyAttachment.UpdateRange(sendMoneyAttachment);
            }

            if (request.SendMoneyList.Count > 0)
            {
                foreach (SendMoneyUpdateList i in request.SendMoneyList)
                {
                    idUpdateSendMoneyList.Add(i.Id);
                    sendMoneyUpdateList.Add(new SendMoneyList
                    {
                        Id = i.Id,
                        PriceIncludesTax = i.PriceIncludesTax,
                        AccountId = i.AccountId,
                        Description = i.Description,
                        TaxId = i.TaxId,
                        Amount = i.Amount,
                        CreatedUid = _currentUser.UserId,
                        TransId = sendMoneyId
                    });
                    sendMoneyFetchList.Add(new SendMoneyFetchList
                    {
                        Id = i.Id,
                        PriceIncludesTax = i.PriceIncludesTax,
                        AccountId = i.AccountId,
                        Description = i.Description,
                        TaxId = i.TaxId,
                        Amount = i.Amount,
                        TransId = sendMoneyId
                    });
                }
                _context.SendMoneyList.UpdateRange(sendMoneyUpdateList);
            }

            List<SendMoneyTag> sendMoneyTagList = _context.SendMoneyTag.Where<SendMoneyTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateSendMoneyTag.Contains(x.Id)).ToList();
            List<SendMoneyAttachment> sendMoneyAttachmentList = _context.SendMoneyAttachment.Where<SendMoneyAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateSendMoneyAttachment.Contains(x.Id)).ToList();
            List<SendMoneyList> sendMoneyList = _context.SendMoneyList.Where<SendMoneyList>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateSendMoneyList.Contains(x.Id)).ToList();
            _context.SendMoneyTag.RemoveRange(sendMoneyTagList);
            _context.SendMoneyAttachment.RemoveRange(sendMoneyAttachmentList);
            _context.SendMoneyList.RemoveRange(sendMoneyList);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new SendMoneyFetchDto()
            {
                Id = request.Id,
                PayFromAccountId = request.PayFromAccountId,
                RecipientContactId = request.RecipientContactId,
                TransactionDate = request.TransactionDate,
                TransactionNo = request.TransactionNo,
                Memo = request.Memo,
                TotalAmount = request.TotalAmount,
                WitholdingAmount = request.WitholdingAmount,
                DiscountAmount = request.DiscountAmount,
                DiscountPercent = request.DiscountPercent,
                DiscountForAccountId = request.DiscountForAccountId,
                SendMoneyTagList = sendMoneyFetchTag,
                SendMoneyAttachmentList = sendMoneyFetchAttachment,
                SendMoneyList = sendMoneyFetchList
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

public class SendMoneyUpdateDto : IRequest<RowResponse<SendMoneyFetchDto>>
{
    public int Id { get; set; }
    public int PayFromAccountId { get; set; } = 0;
    public int RecipientContactId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public Int64 TotalAmount { get; set; } = 0;
    public Int64 WitholdingAmount { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public int DiscountForAccountId { get; set; } = 0;
    public List<SendMoneyAttachmentUpdate> SendMoneyAttachmentFile { get; set; }
    public List<SendMoneyUpdateTag> SendMoneyTagList { get; set; }
    public List<SendMoneyUpdateList> SendMoneyList { get; set; }
}

public class SendMoneyAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class SendMoneyUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}

public class SendMoneyUpdateList
{
    public int Id { get; set; }
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
}