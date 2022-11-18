using TabpediaFin.Dto.Common.Request;
using TabpediaFin.Handler.Interfaces;

namespace TabpediaFin.Handler
{

    public class TagFetchHandler : IQueryByIdHandler<TagDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public TagFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<TagDto>> Handle(QueryByIdDto<TagDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<TagDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<TagDto>(request.Id, _currentUser);

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

}
