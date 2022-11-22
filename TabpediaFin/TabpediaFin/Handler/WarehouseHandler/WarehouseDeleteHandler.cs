namespace TabpediaFin.Handler.WarehouseHandler
{
    public class WarehouseDeleteHandler : IRequestHandler<WarehouseDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public WarehouseDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(WarehouseDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var Warehouse = await _context.Warehouse.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                if (Warehouse != null)
                {
                    _context.Warehouse.Attach(Warehouse);
                    _context.Warehouse.Remove(Warehouse);
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

    public class WarehouseDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
