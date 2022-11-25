namespace TabpediaFin.Handler.ExpenseCategoryHandler
{
    public class ExpenseCategoryDeleteHandler : IRequestHandler<ExpenseCategoryDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ExpenseCategoryDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(ExpenseCategoryDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var ExpenseCategory = await _context.ExpenseCategory.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                
                _context.ExpenseCategory.Attach(ExpenseCategory);
                _context.ExpenseCategory.Remove(ExpenseCategory);

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

    public class ExpenseCategoryDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
