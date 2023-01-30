using TabpediaFin.Domain.ReceiveMoney;

namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyDeleteHandler : IDeleteByIdHandler<ReceiveMoneyFetchDto>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser; 
    }

    public async Task<RowResponse<ReceiveMoneyFetchDto>> Handle(DeleteByIdRequestDto<ReceiveMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ReceiveMoneyFetchDto>();
        try
        {
            // RECEIVE MONEY
            var receiveMoney = await _context.ReceiveMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (receiveMoney == null || receiveMoney.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            var account = await _context.Account.FirstAsync(x => x.Id == receiveMoney.DepositToAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var perhitunganAccount = account.Balance - receiveMoney.TotalAmount;
            account.Balance = perhitunganAccount;

            // RECEIVE MONEY ATTACHMENT DELETE DATA
            List<ReceiveMoneyAttachment> ReceiveMoneyAttachmentList = _context.ReceiveMoneyAttachment.Where<ReceiveMoneyAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
            if (ReceiveMoneyAttachmentList.Count > 0)
            {
                foreach (ReceiveMoneyAttachment item in ReceiveMoneyAttachmentList)
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
            List<ReceiveMoneyTag> ReceiveMoneyTagList = _context.ReceiveMoneyTag.Where<ReceiveMoneyTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();

            // LIST 
            List<ReceiveMoneyList> receiveMoneyList = _context.ReceiveMoneyList.Where<ReceiveMoneyList>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
            if (receiveMoneyList.Count > 0)
            {
                foreach (ReceiveMoneyList i in receiveMoneyList)
                {
                    var accounts = await _context.Account.FirstAsync(x => x.Id == i.AccountId, cancellationToken);
                    var calAccount = accounts.Balance - i.Amount;
                    accounts.Balance = calAccount;
                }
                //await _context.SaveChangesAsync(cancellationToken);
            }
                _context.ReceiveMoney.Remove(receiveMoney);
                _context.ReceiveMoneyAttachment.RemoveRange(ReceiveMoneyAttachmentList);
                if (ReceiveMoneyTagList.Count > 0)
                {
                    _context.ReceiveMoneyTag.RemoveRange(ReceiveMoneyTagList);
                }
                _context.ReceiveMoneyList.RemoveRange(receiveMoneyList);
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
