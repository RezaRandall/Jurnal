using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using TabpediaFin.Infrastructure.Library.SimpleCRUD;
using TabpediaFin.Infrastructure.Security;

namespace Dapper;

public static partial class SimpleCRUD
{
    public static async Task<T> FetchAsync<T>(this IDbConnection connection, object id, ICurrentUser currentUser, IDbTransaction transaction = null, int? commandTimeout = null)
    {
        if (currentUser == null)
            throw new ArgumentException("This operation require current user");

        var currenttype = typeof(T);
        var idProps = GetIdProperties(currenttype).ToList();

        if (!idProps.Any())
            throw new ArgumentException("FetchAsync<T> only supports an entity with a [Key] or Id property");

        //var hasTenantIdProperty = HasTenantIdProperty(currenttype);
        //if (!hasTenantIdProperty)
        //    throw new ArgumentException("FetchAsync<T> only supports an entity with a TenantId property");

        int tenantId = currentUser.TenantId;
        if (tenantId == 0)
            throw new ArgumentException("FetchAsync<T> only supports an entity with a TenantId");

        var name = GetTableName(currenttype);
        var sb = new StringBuilder();
        sb.Append("Select ");
        //create a new empty instance of the type to get the base properties
        BuildSelect(sb, GetScaffoldableProperties<T>().ToArray());
        sb.AppendFormat(@" from {0} where ""TenantId"" = @TenantId and", name);

        var dynParms = new DynamicParameters();
        dynParms.Add("@TenantId", tenantId);

        for (var i = 0; i < idProps.Count; i++)
        {
            if (i > 0)
                sb.Append(" and ");
            sb.AppendFormat("{0} = @{1}", GetColumnName(idProps[i]), idProps[i].Name);
        }

        if (idProps.Count == 1)
            dynParms.Add("@" + idProps.First().Name, id);
        else
        {
            foreach (var prop in idProps)
                dynParms.Add("@" + prop.Name, id.GetType().GetProperty(prop.Name).GetValue(id, null));
        }

        if (Debugger.IsAttached)
            Trace.WriteLine(string.Format("Get<{0}>: {1} with Id: {2}", currenttype, sb, id));

        var query = await connection.QueryAsync<T>(sb.ToString(), dynParms, transaction, commandTimeout);
        return query.FirstOrDefault();
    }

    //tes
    public static Task<IEnumerable<T>> FetchListPagedAsync<T>(this IDbConnection connection, int pageNumber, int rowsPerPage, string conditions, string orderby, ICurrentUser currentUser, DynamicParameters parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
    {
        if (string.IsNullOrEmpty(_getPagedListSql))
            throw new Exception("GetListPage is not supported with the current SQL Dialect");

        var currenttype = typeof(T);
        var idProps = GetIdProperties(currenttype).ToList();
        if (!idProps.Any())
            throw new ArgumentException("Entity must have at least one [Key] property");

        int tenantId = currentUser.TenantId;
        if (tenantId == 0)
            throw new ArgumentException("FetchListPagedAsync<T> only supports an entity with a TenantId");

        var name = GetTableName(currenttype);
        var sb = new StringBuilder();
        var query = _getPagedListSql;
        if (string.IsNullOrEmpty(orderby))
        {
            orderby = GetColumnName(idProps.First());
        }

        conditions= conditions ?? string.Empty;
        if (string.IsNullOrWhiteSpace(conditions))
        {
            conditions += @" WHERE ";
        }
        else
        {
            conditions += @" AND ";
        }

        conditions += @" ""TenantId"" = @TenantId ";

        parameters = parameters ?? new DynamicParameters();
        parameters.Add("TenantId", tenantId);

        //create a new empty instance of the type to get the base properties
        BuildSelect(sb, GetScaffoldableProperties<T>().ToArray());
        query = query.Replace("{SelectColumns}", sb.ToString());
        query = query.Replace("{TableName}", name);
        query = query.Replace("{PageNumber}", pageNumber.ToString());
        query = query.Replace("{RowsPerPage}", rowsPerPage.ToString());
        query = query.Replace("{OrderBy}", orderby);
        query = query.Replace("{WhereClause}", conditions);
        query = query.Replace("{Offset}", ((pageNumber - 1) * rowsPerPage).ToString());

        if (Debugger.IsAttached)
            Trace.WriteLine(String.Format("GetListPaged<{0}>: {1}", currenttype, query));

        return connection.QueryAsync<T>(query, parameters, transaction, commandTimeout);
    }



    public static async Task<QueryPagedResult<T>> FetchListPagedAsync<T>(this IDbConnection connection
     , int pageNumber
     , int rowsPerPage
     , string search
     , string sortby
     , bool sortdesc
     , ICurrentUser currentUser)
    {
        if (string.IsNullOrEmpty(_getPagedListSql))
            throw new Exception("FetchListPagedAsync is not supported with the current SQL Dialect");

        var currenttype = typeof(T);
        var idProps = GetIdProperties(currenttype).ToList();
        if (!idProps.Any())
            throw new ArgumentException("Entity must have at least one [Key] property");

        int tenantId = currentUser.TenantId;
        if (tenantId == 0)
            throw new ArgumentException("FetchListPagedAsync<T> only supports an entity with a TenantId");

        var name = GetTableName(currenttype);
        var sb = new StringBuilder();
        var query = _getPagedListSql;

        string conditions = string.Empty;
        string sqlWhere = string.Empty;
        var parameters = new DynamicParameters();
        parameters.Add("TenantId", tenantId);

        if (string.IsNullOrWhiteSpace(search))
        {
            conditions = @" WHERE ""TenantId"" = @TenantId ";
        }
        else
        {
            sqlWhere = SqlHelper.GenerateWhere<T>();
            parameters.Add("Search", $"%{search.Trim().ToLowerInvariant()}%");

            conditions = @$" WHERE {sqlWhere} AND ""TenantId"" = @TenantId ";
        }

        var orderby = SqlHelper.GenerateOrderBy(sortby, sortdesc, @" ""Name"" ASC  ");
        if (string.IsNullOrEmpty(orderby))
        {
            orderby = GetColumnName(idProps.First());
        }

        //create a new empty instance of the type to get the base properties
        BuildSelect(sb, GetScaffoldableProperties<T>().ToArray());
        query = query.Replace("{SelectColumns}", sb.ToString());
        query = query.Replace("{TableName}", name);
        query = query.Replace("{PageNumber}", pageNumber.ToString());
        query = query.Replace("{RowsPerPage}", rowsPerPage.ToString());
        query = query.Replace("{OrderBy}", orderby);
        query = query.Replace("{WhereClause}", conditions);
        query = query.Replace("{Offset}", ((pageNumber - 1) * rowsPerPage).ToString());

        var queryCount = @$"Select count(1) from {name} {conditions}";

        if (Debugger.IsAttached)
            Trace.WriteLine(String.Format("TotalTecord<{0}>: {1}", currenttype, queryCount));

        if (Debugger.IsAttached)
            Trace.WriteLine(String.Format("FetchListPagedAsync<{0}>: {1}", currenttype, query));

        var list = await connection.QueryAsync<T>(query, parameters);
        var totalRecord = await connection.ExecuteScalarAsync<int>(queryCount, parameters);

        var result = new QueryPagedResult<T>()
        {
            List = list?.AsList() ?? new List<T>(),
            TotalRecord = totalRecord,
            PageNumber = pageNumber,
            PageSize = rowsPerPage
        };

        return result;
    }


    //public static Task<int> RecordCountAsync<T>(this IDbConnection connection, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
    //{
    //    var currenttype = typeof(T);
    //    var name = GetTableName(currenttype);

    //    var sb = new StringBuilder();
    //    var whereprops = GetAllProperties(whereConditions).ToArray();
    //    sb.Append("Select count(1)");
    //    sb.AppendFormat(" from {0}", name);
    //    if (whereprops.Any())
    //    {
    //        sb.Append(" where ");
    //        BuildWhere<T>(sb, whereprops);
    //    }

    //    if (Debugger.IsAttached)
    //        Trace.WriteLine(String.Format("RecordCount<{0}>: {1}", currenttype, sb));

    //    return connection.ExecuteScalarAsync<int>(sb.ToString(), whereConditions, transaction, commandTimeout);
    //}


    private static bool HasTenantIdProperty(Type type)
    {
        // return type.GetProperties().Where(p => p.Name.Equals("TenantId", StringComparison.OrdinalIgnoreCase));
        return type.GetProperty("TenantId") != null;
    }
}
