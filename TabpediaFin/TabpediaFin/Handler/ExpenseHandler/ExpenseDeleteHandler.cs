using TabpediaFin.Domain.Expense;
using TabpediaFin.Domain.SendMoney;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseDeleteHandler : IDeleteByIdHandler<ExpenseFetchDto>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(DeleteByIdRequestDto<ExpenseFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseFetchDto>();
        try
        {
            // EXPENSE
            var expense = await _context.Expense.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (expense == null || expense.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            if (expense.WitholdingAmount != 0)
            {
                double jumlah = expense.TotalAmount + expense.WitholdingAmount;
                var account = await _context.Account.FirstAsync(x => x.Id == expense.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var perhitunganAccount = account.Balance + jumlah;
                account.Balance = perhitunganAccount;

                var discount = await _context.Account.FirstAsync(x => x.Id == expense.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var potongan = discount.Balance;
                var value = potongan - expense.WitholdingAmount;
                discount.Balance = value;
            }
            else
            {
                var account = await _context.Account.FirstAsync(x => x.Id == expense.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var perhitunganAccount = account.Balance + expense.TotalAmount;
                account.Balance = perhitunganAccount;
            }

            // EXPENSE ATTACHMENT
            List<ExpenseAttachment> ExpenseAttachmentList = _context.ExpenseAttachment.Where<ExpenseAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
            if (ExpenseAttachmentList.Count > 0)
            {
                foreach (ExpenseAttachment item in ExpenseAttachmentList)
                {
                    FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

            }

            // TAG
            List<ExpenseTag> ExpenseTagList = _context.ExpenseTag.Where<ExpenseTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();

            //LIST
            List<ExpenseList> expenseList = _context.ExpenseList.Where<ExpenseList>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
            if (expenseList.Count > 0)
            {
                foreach (ExpenseList idx in expenseList)
                {
                    var accounts = await _context.Account.FirstAsync(x => x.Id == idx.ExpenseAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                    var calAccount = accounts.Balance - idx.Amount;
                    accounts.Balance = calAccount;
                }
            }
            _context.Expense.Remove(expense);
            _context.ExpenseAttachment.RemoveRange(ExpenseAttachmentList);
            if (ExpenseTagList.Count > 0)
            {
                _context.ExpenseTag.RemoveRange(ExpenseTagList);
            }
            _context.ExpenseList.RemoveRange(expenseList);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = new ExpenseFetchDto();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}
