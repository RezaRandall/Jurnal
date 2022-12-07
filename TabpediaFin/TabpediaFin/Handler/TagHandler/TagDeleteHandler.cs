namespace TabpediaFin.Handler.TagHandler
{
    public class TagDeleteHandler : IDeleteByIdHandler<TagFetchDto>
    {
        private readonly FinContext _context;
        private readonly ITagCacheRemover _cacheRemover;

        public TagDeleteHandler(FinContext db, ITagCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<TagFetchDto>> Handle(DeleteByIdRequestDto<TagFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<TagFetchDto>();

            try
            {
                var Tag = await _context.Tag.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (Tag == null || Tag.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.Tag.Remove(Tag);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new TagFetchDto();
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
