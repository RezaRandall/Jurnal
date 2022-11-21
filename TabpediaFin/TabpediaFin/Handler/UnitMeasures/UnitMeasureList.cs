using MediatR;

namespace TabpediaFin.Handler.UnitMeasure;

public class UnitMeasureList
{
    //public class Query : IRequest<List<UnitMeasureDto>>
    //{
    //    public string SearchBy { get; }

    //    public Query
    //       (
    //        string searchBy = ""
    //       )
    //    {
    //        SearchBy = searchBy;;
    //    }
    //}

    //public class QueryHandler : IRequestHandler<Query, List<UnitMeasureDto>>
    //{
    //    private readonly IDbConnection _dbConnection;
    //    private readonly ICurrentUser _currentUser;

    //    public QueryHandler(
    //        IDbConnection connection,
    //        ICurrentUser currentUser)
    //    {
    //        _dbConnection = connection;
    //        _currentUser = currentUser;
    //    }

    //    //Implement Paging
    //    public async Task<List<UnitMeasureDto>> Handle(
    //        Query message,
    //        CancellationToken cancellationToken)
    //    {
    //        //var searchByCriteria = message.SearchBy
    //        //    ? $" AND STRFTIME('%Y', e.Date) = '{message.Year}'"
    //        //    : string.Empty;
    //        string sqlsearch = "";
    //        if (message.SearchBy != null && message.SearchBy != "")
    //        {
    //            sqlsearch = @"AND ""Name"" LIKE '%" + message.SearchBy + "%' or \"Description\" LIKE '%" + message.SearchBy + "%' ";
    //        }
    //        //var sql = $@"SELECT * FROM ""UnitMeasure"" WHERE ""TenantId"" = @TenantId ";
    //        var sql = @"SELECT * FROM ""UnitMeasure"" WHERE ""TenantId"" = @TenantId " + sqlsearch + " ";

    //        var expenses = await _dbConnection.QueryAsync<UnitMeasureDto>(sql, new
    //        {
    //            tenantId = _currentUser.TenantId,
    //        });

    //        return expenses.ToList();
    //    }
    //}

}

//public class UnitMeasureList : BaseRepository, IUnitMeasure
//{
//    public class Query : IRequest<List<UnitMeasureDto>>
//    {
//        public string SearchBy { get; }

//        public Query
//           (
//            string searchBy = ""
//           )
//        {
//            SearchBy = searchBy;;
//        }
//    }

//    public class QueryHandler : IRequestHandler<Query, List<UnitMeasureDto>>
//    {
//        private readonly IDbConnection _dbConnection;
//        private readonly ICurrentUser _currentUser;

//        public QueryHandler(
//            IDbConnection connection,
//            ICurrentUser currentUser)
//        {
//            _dbConnection = connection;
//            _currentUser = currentUser;
//        }

//        //Implement Paging
//        public async Task<List<UnitMeasureDto>> Handle(
//            Query message,
//            CancellationToken cancellationToken)
//        {
//            //var searchByCriteria = message.SearchBy
//            //    ? $" AND STRFTIME('%Y', e.Date) = '{message.Year}'"
//            //    : string.Empty;

//            string sqlsearch = "";
//            if (message.SearchBy != null && message.SearchBy != "")
//            {
//                sqlsearch = @"AND ""Name"" LIKE '%" + message.SearchBy + "%' or \"Description\" LIKE '%" + message.SearchBy + "%' ";
//            }
//            var sql = $@"SELECT * FROM ""UnitMeasure"" WHERE ""TenantId"" = @TenantId ";

//            var expenses = await _dbConnection.QueryAsync<UnitMeasureDto>(sql, new
//            {
//                tenantId = _currentUser.TenantId,
//            });

//            return expenses.ToList();
//        }
//    }

//    public class GetListUnitMeasureHandler : IRequestHandler<GetUnitMeasureListQuery, UnitMeasure>
//    {
//        private readonly IUnitMeasure _unitMeasure;
//        public GetListUnitMeasureHandler(IUnitMeasure unitMeasure)
//        {
//            _unitMeasure = unitMeasure;
//        }

//        async Task<UnitMeasure> IRequestHandler<GetUnitMeasureListQuery, UnitMeasure>.Handle(GetUnitMeasureListQuery request, CancellationToken cancellationToken)
//        {
//            var result = await _unitMeasure.GetUnitMeasureList(request);
//            return result;
//        }
//    }

//}