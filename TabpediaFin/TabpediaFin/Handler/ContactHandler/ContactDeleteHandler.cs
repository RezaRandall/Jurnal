namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactDeleteHandler : IRequestHandler<ContactDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ContactDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(ContactDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var Contact = await _context.Contact.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                
                _context.Contact.Attach(Contact);
                _context.Contact.Remove(Contact);
                await _context.SaveChangesAsync(cancellationToken);

                List<ContactAddress> ContactAddress = _context.ContactAddress.Where<ContactAddress>(x => x.ContactId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
                if(ContactAddress.Count > 0)
                {
                    _context.ContactAddress.RemoveRange(ContactAddress);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<ContactPerson> ContactPerson = _context.ContactPerson.Where<ContactPerson>(x => x.ContactId == request.Id && x.TenantId == _currentUser.TenantId).ToList();
                if (ContactPerson.Count > 0)
                {
                    _context.ContactPerson.RemoveRange(ContactPerson);
                    await _context.SaveChangesAsync(cancellationToken);
                }
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

    public class ContactDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
