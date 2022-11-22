namespace TabpediaFin.Handler.ContactAddressHandler
{
    public class ContactAddressDeleteHandler : IRequestHandler<ContactAddressDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ContactAddressDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(ContactAddressDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var ContactAddress = await _context.ContactAddress.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                
                _context.ContactAddress.Attach(ContactAddress);
                _context.ContactAddress.Remove(ContactAddress);

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

    public class ContactAddressDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
