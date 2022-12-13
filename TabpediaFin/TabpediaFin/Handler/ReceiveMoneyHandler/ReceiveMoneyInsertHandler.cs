﻿using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.TransferMoney;
using TabpediaFin.Handler.TransferMoneyHandler;
using TabpediaFin.Handler.UploadAttachmentHandler;

namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyInsertHandler : IRequestHandler<ReceiveMoneyInsertDto, RowResponse<ReceiveMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyInsertHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ReceiveMoneyFetchDto>> Handle(ReceiveMoneyInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ReceiveMoneyFetchDto>();
        int transIdResult;
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);

        var receiveMoney = new ReceiveMoney()
        {
            DepositFromAccountId = request.DepositFromAccountId,
            VendorId = request.VendorId,
            TransactionDate = TransDate,
            TransactionNo = request.TransactionNo,
            PriceIncludesTax = request.PriceIncludesTax,
            ReceiveFromAccountId = request.ReceiveFromAccountId,
            Description = request.Description,
            TaxId = request.TaxId,
            Amount = request.Amount,
            Memo = request.Memo,
            TotalAmount = request.TotalAmount,
        };

        try
        {
            await _context.ReceiveMoney.AddAsync(receiveMoney, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            transIdResult = receiveMoney.Id;
            // UploadAttachmentService service = new UploadAttachmentService();
            //List<uploadreturn> filedata = await service.UploadAttachmentAsync(request.AttachmentFile, _currentUser.TenantId, transIdResult);
            //List<ReceiveMoneyFetchAttachment> returnfile = await PostAttachmentAsync(filedata, cancellationToken);
            List<ReceiveMoneyFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);

            var row = new ReceiveMoneyFetchDto()
            {
                DepositFromAccountId = receiveMoney.DepositFromAccountId,
                VendorId = receiveMoney.VendorId,
                TransactionDate = receiveMoney.TransactionDate,
                TransactionNo = receiveMoney.TransactionNo,
                PriceIncludesTax = receiveMoney.PriceIncludesTax,
                ReceiveFromAccountId = receiveMoney.ReceiveFromAccountId,
                Description = receiveMoney.Description,
                TaxId = receiveMoney.TaxId,
                Amount = receiveMoney.Amount,
                Memo = receiveMoney.Memo,
                TotalAmount = receiveMoney.TotalAmount,
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

    //public async Task<List<ReceiveMoneyFetchAttachment>> PostAttachmentAsync(List<uploadreturn> filedata, CancellationToken cancellationToken)
    //{
    //    List<ReceiveMoneyAttachment> ReceiveMoneyAttachmentList = new List<ReceiveMoneyAttachment>();
    //    List<ReceiveMoneyFetchAttachment> ReceiveMoneyFetchAttachmentList = new List<ReceiveMoneyFetchAttachment>();

    //    if (filedata.Count > 0)
    //    {
    //        foreach (uploadreturn item in filedata)
    //        {
    //            ReceiveMoneyAttachmentList.Add(new ReceiveMoneyAttachment
    //            {
    //                FileName = item.FileName,
    //                FileUrl = item.FileUrl,
    //                Extension = item.Extension,
    //                FileSize = item.FileSize,
    //                TransId = item.TransId,
    //            });
    //            ReceiveMoneyFetchAttachmentList.Add(new ReceiveMoneyFetchAttachment
    //            {
    //                FileName = item.FileName,
    //                FileUrl = item.FileUrl,
    //                Extension = item.Extension,
    //                FileSize = item.FileSize,
    //                TransId = item.TransId,
    //            });
    //        }

    //        await _context.ReceiveMoneyAttachment.AddRangeAsync(ReceiveMoneyAttachmentList, cancellationToken);
    //        await _context.SaveChangesAsync(cancellationToken);
    //    }

    //    return ReceiveMoneyFetchAttachmentList;
    //}

    public async Task<List<ReceiveMoneyFetchTag>> PostTagAsync(List<ReceiveMoneyInsertTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ReceiveMoneyTag> ReceiveMoneyTag = new List<ReceiveMoneyTag>();
        List<ReceiveMoneyFetchTag> ReceiveMoneyFetchTag = new List<ReceiveMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (ReceiveMoneyInsertTag item in filedata)
            {
                ReceiveMoneyTag.Add(new ReceiveMoneyTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
                ReceiveMoneyFetchTag.Add(new ReceiveMoneyFetchTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
            }

            await _context.ReceiveMoneyTag.AddRangeAsync(ReceiveMoneyTag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ReceiveMoneyFetchTag;
    }



}

public class ReceiveMoneyInsertDto : IRequest<RowResponse<ReceiveMoneyFetchDto>>
{
    public int DepositFromAccountId { get; set; } = 0;
    public int VendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public int TransactionNo { get; set; } = 0;
    public bool PriceIncludesTax { get; set; } = false;
    public int ReceiveFromAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    //public string AttachmentFileName { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public ICollection<IFormFile> AttachmentFile { get; set; }
    public List<ReceiveMoneyInsertTag> TagList { get; set; }
}

public class ReceiveMoneyInsertTag
{
    public int TagId { get; set; } = 0;
}