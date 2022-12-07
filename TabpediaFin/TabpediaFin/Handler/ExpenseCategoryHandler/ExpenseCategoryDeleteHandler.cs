namespace TabpediaFin.Handler.ExpenseCategoryHandler
{
    public class ExpenseCategoryDeleteHandler : IDeleteByIdHandler<ExpenseCategoryFetchDto>
    {
        private readonly FinContext _context;
        private readonly IExpenseCategoryCacheRemover _cacheRemover;

        public ExpenseCategoryDeleteHandler(FinContext db, IExpenseCategoryCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<ExpenseCategoryFetchDto>> Handle(DeleteByIdRequestDto<ExpenseCategoryFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ExpenseCategoryFetchDto>();

            try
            {
                var ExpenseCategory = await _context.ExpenseCategory.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (ExpenseCategory == null || ExpenseCategory.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.ExpenseCategory.Remove(ExpenseCategory);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new ExpenseCategoryFetchDto();
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
