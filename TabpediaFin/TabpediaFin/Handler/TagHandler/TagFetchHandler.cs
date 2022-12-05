namespace TabpediaFin.Handler.TagHandler
{
    public class TagFetchHandler : IFetchByIdHandler<TagFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public TagFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<TagFetchDto>> Handle(FetchByIdRequestDto<TagFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<TagFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<TagFetchDto>(request.Id, _currentUser);

                    response.IsOk = true;
                    response.Row = row;
                    response.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.Row = null;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }

    [Table("Tag")]
    public class TagFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
