namespace TabpediaFin.Handler.ItemCategoryHandler
{
    public class ItemCategoryDeleteHandler : IRequestHandler<ItemCategoryDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ItemCategoryDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(ItemCategoryDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var ItemCategory = await _context.ItemCategory.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                if (ItemCategory != null)
                {
                    _context.ItemCategory.Attach(ItemCategory);
                    _context.ItemCategory.Remove(ItemCategory);
                    _context.SaveChanges();
                }

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

    public class ItemCategoryDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
