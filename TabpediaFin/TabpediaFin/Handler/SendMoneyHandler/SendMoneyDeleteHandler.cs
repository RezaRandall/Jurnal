using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.SendMoney;

namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyDeleteHandler : IDeleteByIdHandler<SendMoneyFetchDto>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public SendMoneyDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<SendMoneyFetchDto>> Handle(DeleteByIdRequestDto<SendMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<SendMoneyFetchDto>();
        try
        {
            // SEND MONEY DELETE
            var sendMoney = await _context.SendMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (sendMoney == null || sendMoney.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            if (sendMoney.WitholdingAmount != 0)
            {
                double jumlah = sendMoney.TotalAmount + sendMoney.WitholdingAmount;

                var account = await _context.Account.FirstAsync(x => x.Id == sendMoney.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);

                var perhitunganAccount = account.Balance + jumlah;

                account.Balance = perhitunganAccount;


                var discount = await _context.Account.FirstAsync(x => x.Id == sendMoney.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);

                var potongan = discount.Balance;

                var value = potongan - sendMoney.WitholdingAmount;

                discount.Balance = value;
            }
            else
            {
                var account = await _context.Account.FirstAsync(x => x.Id == sendMoney.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);

                var perhitunganAccount = account.Balance + sendMoney.TotalAmount;

                account.Balance = perhitunganAccount;
            }
            


            // SEND MONEY ATTACHMENT DELETE DATA
            List<SendMoneyAttachment> SendMoneyAttachmentList = _context.SendMoneyAttachment.Where<SendMoneyAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
            if (SendMoneyAttachmentList.Count > 0)
            {
                foreach (SendMoneyAttachment item in SendMoneyAttachmentList)
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
            List<SendMoneyTag> SendMoneyTagList = _context.SendMoneyTag.Where<SendMoneyTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();

            //LIST
            List<SendMoneyList> sendMoneyList = _context.SendMoneyList.Where<SendMoneyList>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
            if (sendMoneyList.Count > 0)
            {
                foreach (SendMoneyList idx in sendMoneyList)
                {
                    var accounts = await _context.Account.FirstAsync(x => x.Id == idx.AccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                    var calAccount = accounts.Balance + idx.Amount;
                    accounts.Balance = calAccount;
                }
            }
            _context.SendMoney.Remove(sendMoney);
            _context.SendMoneyAttachment.RemoveRange(SendMoneyAttachmentList);
            if (SendMoneyTagList.Count > 0)
            {
                _context.SendMoneyTag.RemoveRange(SendMoneyTagList);
            }
            _context.SendMoneyList.RemoveRange(sendMoneyList);
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
