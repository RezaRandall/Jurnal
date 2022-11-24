namespace TabpediaFin.Handler.ContactAddressTypeHandler
{
    public class ContactAddressTypeDeleteHandler : IRequestHandler<ContactAddressTypeDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ContactAddressTypeDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(ContactAddressTypeDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var ContactAddressType = await _context.ContactAddressType.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);

                _context.ContactAddressType.Attach(ContactAddressType);
                _context.ContactAddressType.Remove(ContactAddressType);
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

    public class ContactAddressTypeDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
