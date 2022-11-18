using TabpediaFin.Dto.Common.Request;
using TabpediaFin.Handler.Interfaces;

namespace TabpediaFin.Handler
{
    public class AddressTypeFetchHandler : IQueryByIdHandler<AddressTypeDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public AddressTypeFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<AddressTypeDto>> Handle(QueryByIdDto<AddressTypeDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<AddressTypeDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<AddressTypeDto>(request.Id, _currentUser);

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
