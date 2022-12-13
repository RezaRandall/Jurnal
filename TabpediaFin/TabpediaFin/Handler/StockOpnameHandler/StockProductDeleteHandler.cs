namespace TabpediaFin.Handler.StockHandler
{
    public class StockProductDeleteHandler : IDeleteByIdHandler<StockProductListDto>
    {
        private readonly FinContext _context;
        //private readonly IStockProductCacheRemover _cacheRemover;

        public StockProductDeleteHandler(FinContext db/*, IStockProductCacheRemover cacheRemover*/)
        {
            _context = db;
            //_cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<StockProductListDto>> Handle(DeleteByIdRequestDto<StockProductListDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<StockProductListDto>();

            try
            {
                var StockProduct = await _context.ItemStock.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (StockProduct == null || StockProduct.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.ItemStock.Remove(StockProduct);
                await _context.SaveChangesAsync(cancellationToken);

                //_cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new StockProductListDto();
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
