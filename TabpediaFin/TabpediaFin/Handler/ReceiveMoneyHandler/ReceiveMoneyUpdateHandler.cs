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
        var result = new RowResponse<ReceiveMoneyFetchDto>();

        try
        {
            var receiveMoney = await _context.ReceiveMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            receiveMoney.DepositFromAccountId = request.DepositFromAccountId;
            receiveMoney.VendorId = request.VendorId;
            receiveMoney.TransactionDate = request.TransactionDate;
            receiveMoney.TransactionNo = request.TransactionNo;
            receiveMoney.PriceIncludesTax = request.PriceIncludesTax;
            receiveMoney.ReceiveFromAccountId = request.ReceiveFromAccountId;
            receiveMoney.Description = request.Description;
            receiveMoney.TaxId = request.TaxId;
            receiveMoney.Amount = request.Amount;
            receiveMoney.Memo = request.Memo;
            receiveMoney.TotalAmount = request.TotalAmount;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ReceiveMoneyFetchDto()
            {
                Id = request.Id,
                DepositFromAccountId = request.DepositFromAccountId,
                VendorId = request.VendorId,
                TransactionDate = request.TransactionDate,
                TransactionNo = request.TransactionNo,
                PriceIncludesTax = request.PriceIncludesTax,
                ReceiveFromAccountId = request.ReceiveFromAccountId,
                Description = request.Description,
                TaxId = request.TaxId,
                Amount = request.Amount,
                Memo = request.Memo,
                TotalAmount = request.TotalAmount,
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
}