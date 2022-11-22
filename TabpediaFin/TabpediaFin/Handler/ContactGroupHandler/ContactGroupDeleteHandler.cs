namespace TabpediaFin.Handler.ContactGroupHandler
{
    public class ContactGroupDeleteHandler : IRequestHandler<ContactGroupDeleteDto, RowResponse<bool>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ContactGroupDeleteHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<bool>> Handle(ContactGroupDeleteDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<bool>();
            try
            {
                var ContactGroup = await _context.ContactGroup.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                if (ContactGroup != null)
                {
                    _context.ContactGroup.Attach(ContactGroup);
                    _context.ContactGroup.Remove(ContactGroup);
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

    public class ContactGroupDeleteDto : IRequest<RowResponse<bool>>
    {
        public int Id { get; set; }

    }
}
