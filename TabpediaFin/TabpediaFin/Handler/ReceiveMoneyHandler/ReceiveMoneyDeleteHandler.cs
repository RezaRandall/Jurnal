using TabpediaFin.Domain.ReceiveMoney;

namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyDeleteHandler : IDeleteByIdHandler<ReceiveMoneyFetchDto>
{
    private readonly FinContext _context;

    public ReceiveMoneyDeleteHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ReceiveMoneyFetchDto>> Handle(DeleteByIdRequestDto<ReceiveMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ReceiveMoneyFetchDto>();
        try
        {
            // RECEIVE MONEY
            var receiveMoney = await _context.ReceiveMoney.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (receiveMoney == null || receiveMoney.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            _context.ReceiveMoney.Remove(receiveMoney);
            await _context.SaveChangesAsync(cancellationToken);

            // RECEIVE MONEY ATTACHMENT DELETE DATA
            List<ReceiveMoneyAttachment> ReceiveMoneyAttachmentList = _context.ReceiveMoneyAttachment.Where<ReceiveMoneyAttachment>(x => x.TransId == request.Id).ToList();
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
                _context.ReceiveMoneyAttachment.RemoveRange(ReceiveMoneyAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // TAG
            List<ReceiveMoneyTag> ReceiveMoneyTagList = _context.ReceiveMoneyTag.Where<ReceiveMoneyTag>(x => x.TransId == request.Id).ToList();
            if (ReceiveMoneyTagList.Count > 0)
            {
                _context.ReceiveMoneyTag.RemoveRange(ReceiveMoneyTagList);
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
