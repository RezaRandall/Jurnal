namespace TabpediaFin.Handler.ContactPersonHandler
{
    public class ContactPersonDeleteHandler : IDeleteByIdHandler<ContactPersonFetchDto>
    {
        private readonly FinContext _context;
        private readonly IContactPersonCacheRemover _cacheRemover;

        public ContactPersonDeleteHandler(FinContext db, IContactPersonCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<ContactPersonFetchDto>> Handle(DeleteByIdRequestDto<ContactPersonFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactPersonFetchDto>();

            try
            {
                var ContactPerson = await _context.ContactPerson.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (ContactPerson == null || ContactPerson.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.ContactPerson.Remove(ContactPerson);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new ContactPersonFetchDto();
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
