using NPOI.SS.Formula.Functions;
using TabpediaFin.Handler.TransferMoneyHandler;

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
        var result = new RowResponse<TransferMoneyFetchDto>();

        try
        {
            var tranferMoney = await _context.TransferMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            tranferMoney.TransferFromAccountId = request.TransferFromAccountId;
            tranferMoney.DepositToAccountId = request.DepositToAccountId;
            tranferMoney.Amount = request.Amount;
            tranferMoney.Memo = request.Memo;
            tranferMoney.TransactionNumber = request.TransactionNumber;
            tranferMoney.TransactionDate = request.TransactionDate;

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

public class TransferMoneyUpdateDto : IRequest<RowResponse<TransferMoneyFetchDto>>
{
    public int Id { get; set; }
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public int Memo { get; set; } = 0;
    //public int Tag { get; set; } = 0;
    public string TransactionNumber { get; set; } = string.Empty;
    //public string FileName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
}