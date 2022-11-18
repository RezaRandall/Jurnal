namespace TabpediaFin.Handler.UnitMeasure;

public class UnitMeasureList
{
    public class Query : IRequest<List<UnitMeasureDto>>
    {
        //public string SearchBy { get; }

        public Query
           (
            //string searchBy = ""
           )
        {
            //SearchBy = searchBy;;
        }
    }

    public class QueryHandler : IRequestHandler<Query, List<UnitMeasureDto>>
    {
        private readonly IDbConnection _dbConnection;
        private readonly ICurrentUser _currentUser;

        public QueryHandler(
            IDbConnection connection,
            ICurrentUser currentUser)
        {
            _dbConnection = connection;
            _currentUser = currentUser;
        }

        //Implement Paging
        public async Task<List<UnitMeasureDto>> Handle(
            Query message,
            CancellationToken cancellationToken)
        {
            //var searchByCriteria = message.SearchBy
            //    ? $" AND STRFTIME('%Y', e.Date) = '{message.Year}'"
            //    : string.Empty;
            var sql = $@"SELECT * FROM ""UnitMeasure"" WHERE ""TenantId"" = @TenantId ";

            var unitMeasures = await _dbConnection.QueryAsync<UnitMeasureDto>(sql, new
            {
                tenantId = _currentUser.TenantId,
            });

            return unitMeasures.ToList();
        }
    }
}
