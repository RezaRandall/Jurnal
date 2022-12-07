namespace TabpediaFin.Handler.ContactAddressHandler
{
    public class ContactAddressDeleteHandler : IDeleteByIdHandler<ContactAddressFetchDto>
    {
        private readonly FinContext _context;
        private readonly IContactAddressCacheRemover _cacheRemover;

        public ContactAddressDeleteHandler(FinContext db, IContactAddressCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<ContactAddressFetchDto>> Handle(DeleteByIdRequestDto<ContactAddressFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactAddressFetchDto>();

            try
            {
                var ContactAddress = await _context.ContactAddress.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (ContactAddress == null || ContactAddress.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.ContactAddress.Remove(ContactAddress);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new ContactAddressFetchDto();
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
