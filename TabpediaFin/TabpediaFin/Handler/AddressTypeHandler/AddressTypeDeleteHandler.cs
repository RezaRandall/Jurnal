namespace TabpediaFin.Handler.AddressTypeHandler
{
    public class AddressTypeDeleteHandler : IRequestHandler<AddressTypeDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public AddressTypeDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(AddressTypeDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var AddressType = await _context.AddressType.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);

                _context.AddressType.Attach(AddressType);
                _context.AddressType.Remove(AddressType);
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

    public class AddressTypeDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
