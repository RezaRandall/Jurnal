namespace TabpediaFin.Handler.TaxHandler
{
    public class AddressTypeFetchHandler : IQueryByIdHandler<TaxFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public AddressTypeFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<TaxFetchDto>> Handle(QueryByIdDto<TaxFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<TaxFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<TaxFetchDto>(request.Id, _currentUser);

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

    [Table("Tax")]
    public class TaxFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double RatePercent { get; set; }
    }
}
