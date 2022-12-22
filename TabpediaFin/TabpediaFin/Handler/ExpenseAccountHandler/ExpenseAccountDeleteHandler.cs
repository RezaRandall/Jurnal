using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Handler.ExpenseAccountHandler;

public class ExpenseAccountDeleteHandler : IDeleteByIdHandler<ExpenseAccountFetchDto>
{
    private readonly FinContext _context;

    public ExpenseAccountDeleteHandler(FinContext db)
    {
        _context = db;
    }


    public async Task<RowResponse<ExpenseAccountFetchDto>> Handle(DeleteByIdRequestDto<ExpenseAccountFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseAccountFetchDto>();
        try
        {
            var expenseAccount = await _context.ExpenseAccount.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (expenseAccount == null || expenseAccount.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            _context.ExpenseAccount.Remove(expenseAccount);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}
