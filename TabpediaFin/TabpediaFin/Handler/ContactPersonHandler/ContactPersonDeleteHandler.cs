namespace TabpediaFin.Handler.ContactPersonHandler
{
    public class ContactPersonDeleteHandler : IRequestHandler<ContactPersonDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ContactPersonDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(ContactPersonDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var ContactPerson = await _context.ContactPerson.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                if (ContactPerson != null)
                {
                    _context.ContactPerson.Attach(ContactPerson);
                    _context.ContactPerson.Remove(ContactPerson);
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

    public class ContactPersonDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
