namespace TabpediaFin.Handler.ContactGroupHandler
{
    public class ContactGroupDeleteHandler : IDeleteByIdHandler<ContactGroupFetchDto>
    {
        private readonly FinContext _context;
        private readonly IContactGroupCacheRemover _cacheRemover;

        public ContactGroupDeleteHandler(FinContext db, IContactGroupCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<ContactGroupFetchDto>> Handle(DeleteByIdRequestDto<ContactGroupFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactGroupFetchDto>();

            try
            {
                var ContactGroup = await _context.ContactGroup.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (ContactGroup == null || ContactGroup.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.ContactGroup.Remove(ContactGroup);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new ContactGroupFetchDto();
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
