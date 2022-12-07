namespace TabpediaFin.Handler.ItemCategoryHandler
{
    public class ItemCategoryDeleteHandler : IDeleteByIdHandler<ItemCategoryFetchDto>
    {
        private readonly FinContext _context;
        private readonly IItemCategoryCacheRemover _cacheRemover;

        public ItemCategoryDeleteHandler(FinContext db, IItemCategoryCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<ItemCategoryFetchDto>> Handle(DeleteByIdRequestDto<ItemCategoryFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ItemCategoryFetchDto>();

            try
            {
                var ItemCategory = await _context.ItemCategory.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (ItemCategory == null || ItemCategory.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.ItemCategory.Remove(ItemCategory);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new ItemCategoryFetchDto();
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
