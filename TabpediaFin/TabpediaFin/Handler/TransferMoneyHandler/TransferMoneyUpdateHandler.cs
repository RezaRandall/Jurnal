using NPOI.XWPF.UserModel;
using TabpediaFin.Domain.SendMoney;
using TabpediaFin.Domain.TransferMoney;
using TabpediaFin.Handler.SendMoneyHandler;

namespace TabpediaFin.Handler.TransferMoneyHandler;

public class TransferMoneyUpdateHandler : IRequestHandler<TransferMoneyUpdateDto, RowResponse<TransferMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public TransferMoneyUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<TransferMoneyFetchDto>> Handle(TransferMoneyUpdateDto request, CancellationToken cancellationToken)
    {
        int transferMoneyId;
        var result = new RowResponse<TransferMoneyFetchDto>();

        List<TransferMoneyTag> transferMoneyTag = new List<TransferMoneyTag>();
        List<TransferMoneyAttachment> transferMoneyAttachment = new List<TransferMoneyAttachment>();
        List<TransferMoneyFetchTag> transferMoneyFetchTag = new List<TransferMoneyFetchTag>();
        List<TransferMoneyFetchAttachment> transferMoneyFetchAttachment = new List<TransferMoneyFetchAttachment>();

        try
        {
            var transferMoney = await _context.TransferMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);


            var balanceAccountCashAndBankTrans = await _context.AccountCashAndBank.FirstAsync(x => x.Id == transferMoney.TransferFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var balanceAccountCashAndBankDepo = await _context.AccountCashAndBank.FirstAsync(x => x.Id == transferMoney.DepositToAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);

            var balanceTrans = balanceAccountCashAndBankTrans.Balance;
            var balanceDepo = balanceAccountCashAndBankDepo.Balance;

            if (request.Amount > transferMoney.Amount && request.Amount < balanceTrans)
            {
                var backBalanceTrans = request.Amount - transferMoney.Amount;
                balanceAccountCashAndBankTrans.Balance = balanceTrans - backBalanceTrans;
                balanceAccountCashAndBankDepo.Balance = balanceDepo + backBalanceTrans;
                transferMoney.TransferFromAccountId = request.TransferFromAccountId;
                transferMoney.DepositToAccountId = request.DepositToAccountId;
                transferMoney.Amount = request.Amount;
                transferMoney.Memo = request.Memo;
                transferMoney.TransactionNumber = request.TransactionNumber;
                transferMoney.TransactionDate = request.TransactionDate;
            }
            if (request.Amount < transferMoney.Amount)
            {
                var backBalanceTrans = transferMoney.Amount - request.Amount;
                balanceAccountCashAndBankTrans.Balance = balanceTrans + backBalanceTrans;
                balanceAccountCashAndBankDepo.Balance = balanceDepo - backBalanceTrans;
                transferMoney.TransferFromAccountId = request.TransferFromAccountId;
                transferMoney.DepositToAccountId = request.DepositToAccountId;
                transferMoney.Amount = request.Amount;
                transferMoney.Memo = request.Memo;
                transferMoney.TransactionNumber = request.TransactionNumber;
                transferMoney.TransactionDate = request.TransactionDate;
            }
            if (request.Amount == 0)
            {
                var valSum = balanceTrans + balanceDepo;
                balanceAccountCashAndBankTrans.Balance = valSum;
                balanceAccountCashAndBankDepo.Balance = balanceDepo - balanceDepo;
                transferMoney.TransferFromAccountId = request.TransferFromAccountId;
                transferMoney.DepositToAccountId = request.DepositToAccountId;
                transferMoney.Amount = request.Amount;
                transferMoney.Memo = request.Memo;
                transferMoney.TransactionNumber = request.TransactionNumber;
                transferMoney.TransactionDate = request.TransactionDate;
            }
            if (request.Amount == transferMoney.Amount)
            {
                balanceAccountCashAndBankTrans.Balance = balanceTrans;
                balanceAccountCashAndBankDepo.Balance = balanceDepo;
                transferMoney.TransferFromAccountId = request.TransferFromAccountId;
                transferMoney.DepositToAccountId = request.DepositToAccountId;
                transferMoney.Amount = request.Amount;
                transferMoney.Memo = request.Memo;
                transferMoney.TransactionNumber = request.TransactionNumber;
                transferMoney.TransactionDate = request.TransactionDate;
            }
            if (balanceTrans < request.Amount)
            {
                result.IsOk = false;
                result.ErrorMessage = "Insufficient balance for this transaction, Failed!";
            }

            transferMoneyId = request.Id;
            List<int> idUpdateTransferMoneyTag = new List<int>();
            List<int> idUpdateTransferMoneyAttachment = new List<int>();

            if (request.TransferMoneyTagList.Count > 0)
            {
                foreach (TransferMoneyUpdateTag i in request.TransferMoneyTagList)
                {
                    idUpdateTransferMoneyTag.Add(i.Id);
                    transferMoneyTag.Add(new TransferMoneyTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = transferMoneyId,
                        CreatedUid = _currentUser.UserId
                    });
                    transferMoneyFetchTag.Add(new TransferMoneyFetchTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = transferMoneyId
                    });
                }
                _context.TransferMoneyTag.UpdateRange(transferMoneyTag);
            }

            if (request.TransferMoneyAttachmentFile.Count > 0)
            {
                foreach (TransferMoneyAttachmentUpdate i in request.TransferMoneyAttachmentFile)
                {
                    idUpdateTransferMoneyAttachment.Add(i.Id);
                    transferMoneyAttachment.Add(new TransferMoneyAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        TransId = transferMoneyId
                    });
                    transferMoneyFetchAttachment.Add(new TransferMoneyFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        TransId = transferMoneyId
                    });
                }
                _context.TransferMoneyAttachment.UpdateRange(transferMoneyAttachment);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var row = new TransferMoneyFetchDto()
            {
                Id = request.Id,
                TransferFromAccountId = request.TransferFromAccountId,
                DepositToAccountId = request.DepositToAccountId,
                Amount = request.Amount,
                Memo = request.Memo,
                TransactionNumber = request.TransactionNumber,
                TransactionDate = request.TransactionDate,
                TransferMoneyTagList = transferMoneyFetchTag,
                TransferMoneyAttachmentList = transferMoneyFetchAttachment,
            };

            if (result.IsOk == false)
            {
                return result;
            }
            else 
            {
                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = row;
            }
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


    public async Task<List<TransferMoneyFetchAttachment>> UpdateAttachmentAsync(List<TransferMoneyAttachmentUpdate> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<TransferMoneyAttachment> TransferMoneyAttachmentList = new List<TransferMoneyAttachment>();
        List<TransferMoneyFetchAttachment> TransferMoneyFetchAttachmentList = new List<TransferMoneyFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (TransferMoneyAttachmentUpdate item in filedata)
            {
                TransferMoneyAttachmentList.Add(new TransferMoneyAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
                TransferMoneyFetchAttachmentList.Add(new TransferMoneyFetchAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
            }

            _context.TransferMoneyAttachment.UpdateRange(TransferMoneyAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return TransferMoneyFetchAttachmentList;
    }

    public async Task<List<TransferMoneyFetchTag>> UpdateTagAsync(List<TransferMoneyUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<TransferMoneyTag> TransferMoneyTag = new List<TransferMoneyTag>();
        List<TransferMoneyFetchTag> TransferMoneyFetchTag = new List<TransferMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (TransferMoneyUpdateTag item in filedata)
            {
                TransferMoneyTag.Add(new TransferMoneyTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
                TransferMoneyFetchTag.Add(new TransferMoneyFetchTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
            }
            _context.TransferMoneyTag.UpdateRange(TransferMoneyTag);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return TransferMoneyFetchTag;
    }



}

public class TransferMoneyUpdateDto : IRequest<RowResponse<TransferMoneyFetchDto>>
{
    public int Id { get; set; }
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public List<TransferMoneyAttachmentUpdate> TransferMoneyAttachmentFile { get; set; }
    public List<TransferMoneyUpdateTag> TransferMoneyTagList { get; set; }
}

public class TransferMoneyAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class TransferMoneyUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}