namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyDeleteHandler : IRequestHandler<ReceiveMoneyDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<bool>> Handle(ReceiveMoneyDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var receiveMoney = await _context.ReceiveMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);

            _context.ReceiveMoney.Attach(receiveMoney);
            _context.ReceiveMoney.Remove(receiveMoney);

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

public class ReceiveMoneyDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; }
}