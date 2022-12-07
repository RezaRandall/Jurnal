namespace TabpediaFin.Handler.ContactAddressTypeHandler
{
    public class ContactAddressTypeDeleteHandler : IDeleteByIdHandler<ContactAddressTypeFetchDto>
    {
        private readonly FinContext _context;
        private readonly IContactAddressTypeCacheRemover _cacheRemover;

        public ContactAddressTypeDeleteHandler(FinContext db, IContactAddressTypeCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<ContactAddressTypeFetchDto>> Handle(DeleteByIdRequestDto<ContactAddressTypeFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactAddressTypeFetchDto>();

            try
            {
                var ContactAddressType = await _context.ContactAddressType.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (ContactAddressType == null || ContactAddressType.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.ContactAddressType.Remove(ContactAddressType);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new ContactAddressTypeFetchDto();
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
