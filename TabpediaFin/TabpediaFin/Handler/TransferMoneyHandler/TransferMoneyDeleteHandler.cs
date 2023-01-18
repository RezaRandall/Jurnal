using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.TransferMoney;

namespace TabpediaFin.Handler.TransferMoneyHandler;

public class TransferMoneyDeleteHandler : IDeleteByIdHandler<TransferMoneyFetchDto>
{
    private readonly FinContext _context;

    public TransferMoneyDeleteHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<TransferMoneyFetchDto>> Handle(DeleteByIdRequestDto<TransferMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TransferMoneyFetchDto>();
        try
        {
            // TRANSFER MONEY
            var transferMoney = await _context.TransferMoney.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (transferMoney == null || transferMoney.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            var account = await _context.Account.FirstAsync(x => x.Id == transferMoney.TransferFromAccountId, cancellationToken);
            var sumAccount = account.Balance + transferMoney.Amount;
            account.Balance = sumAccount;

            var accounts = await _context.Account.FirstAsync(x => x.Id == transferMoney.DepositToAccountId, cancellationToken);
            var sumAccounts = accounts.Balance - transferMoney.Amount;
            accounts.Balance = sumAccounts;

            //await _context.SaveChangesAsync(cancellationToken);

            // TRANSFER MONEY ATTACHMENT DELETE DATA
            List<TransferMoneyAttachment> TransferMoneyAttachmentList = _context.TransferMoneyAttachment.Where<TransferMoneyAttachment>(x => x.TransId == request.Id).ToList();
            if (TransferMoneyAttachmentList.Count > 0)
            {
                foreach (TransferMoneyAttachment item in TransferMoneyAttachmentList)
                {
                    FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                //await _context.SaveChangesAsync(cancellationToken);
            }

            // TAG
            List<TransferMoneyTag> TransferMoneyTagList = _context.TransferMoneyTag.Where<TransferMoneyTag>(x => x.TransId == request.Id).ToList();
            if (TransferMoneyTagList.Count > 0)
            {
                _context.TransferMoneyTag.RemoveRange(TransferMoneyTagList);
                //await _context.SaveChangesAsync(cancellationToken);
            }
            _context.TransferMoney.Remove(transferMoney);
            _context.TransferMoneyAttachment.RemoveRange(TransferMoneyAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


}
