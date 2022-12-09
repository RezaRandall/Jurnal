using TabpediaFin.Handler.ExpenseHandler;

namespace TabpediaFin.Handler.TransferMoneyHandler;

public class TransferMoneyDeleteHandler : IRequestHandler<TransferMoneyDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public TransferMoneyDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<bool>> Handle(TransferMoneyDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var transferMoney = await _context.TransferMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);

            _context.TransferMoney.Attach(transferMoney);
            _context.TransferMoney.Remove(transferMoney);

            await _context.SaveChangesAsync(cancellationToken);

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = true;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


}
public class TransferMoneyDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; }
}