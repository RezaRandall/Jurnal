namespace TabpediaFin.Handler.WarehouseHandler
{
    public class WarehouseDeleteHandler : IDeleteByIdHandler<WarehouseFetchDto>
    {
        private readonly FinContext _context;
        private readonly IWarehouseCacheRemover _cacheRemover;

        public WarehouseDeleteHandler(FinContext db, IWarehouseCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<WarehouseFetchDto>> Handle(DeleteByIdRequestDto<WarehouseFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<WarehouseFetchDto>();

            try
            {
                var Warehouse = await _context.Warehouse.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (Warehouse == null || Warehouse.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.Warehouse.Remove(Warehouse);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new WarehouseFetchDto();
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
