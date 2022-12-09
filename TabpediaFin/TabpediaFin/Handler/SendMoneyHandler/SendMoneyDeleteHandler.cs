namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyDeleteHandler : IRequestHandler<SendMoneyDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public SendMoneyDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<bool>> Handle(SendMoneyDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var transferMoney = await _context.SendMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);

            _context.SendMoney.Attach(transferMoney);
            _context.SendMoney.Remove(transferMoney);

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

public class SendMoneyDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; }
}