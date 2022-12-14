using TabpediaFin.Domain.Expense;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseDeleteHandler : IDeleteByIdHandler<ExpenseFetchDto>
{
    private readonly FinContext _context;

    public ExpenseDeleteHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(DeleteByIdRequestDto<ExpenseFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseFetchDto>();
        try
        {
            // EXPENSE
            var expense = await _context.Expense.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (expense == null || expense.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            _context.Expense.Remove(expense);
            await _context.SaveChangesAsync(cancellationToken);

            // EXPENSE ATTACHMENT
            List<ExpenseAttachment> ExpenseAttachmentList = _context.ExpenseAttachment.Where<ExpenseAttachment>(x => x.TransId == request.Id).ToList();
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
                _context.ExpenseAttachment.RemoveRange(ExpenseAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // TAG
            List<ExpenseTag> ExpenseTagList = _context.ExpenseTag.Where<ExpenseTag>(x => x.TransId == request.Id).ToList();
            if (ExpenseTagList.Count > 0)
            {
                _context.ExpenseTag.RemoveRange(ExpenseTagList);
                await _context.SaveChangesAsync(cancellationToken);

            }

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
