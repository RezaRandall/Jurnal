using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.SendMoney;

namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyDeleteHandler : IDeleteByIdHandler<SendMoneyFetchDto>
{
    private readonly FinContext _context;

    public SendMoneyDeleteHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<SendMoneyFetchDto>> Handle(DeleteByIdRequestDto<SendMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<SendMoneyFetchDto>();
        try
        {
            // SEND MONEY DELETE
            var transferMoney = await _context.SendMoney.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (transferMoney == null || transferMoney.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            _context.SendMoney.Remove(transferMoney);
            await _context.SaveChangesAsync(cancellationToken);

            // SEND MONEY ATTACHMENT DELETE DATA
            List<SendMoneyAttachment> SendMoneyAttachmentList = _context.SendMoneyAttachment.Where<SendMoneyAttachment>(x => x.TransId == request.Id).ToList();
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
                _context.SendMoneyAttachment.RemoveRange(SendMoneyAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // TAG
            List<SendMoneyTag> SendMoneyTagList = _context.SendMoneyTag.Where<SendMoneyTag>(x => x.TransId == request.Id).ToList();
            if (SendMoneyTagList.Count > 0)
            {
                _context.SendMoneyTag.RemoveRange(SendMoneyTagList);
                await _context.SaveChangesAsync(cancellationToken);

            }

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
