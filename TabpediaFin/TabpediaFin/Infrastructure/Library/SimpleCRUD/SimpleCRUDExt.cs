using System.Diagnostics;
using System.Reflection;
using System.Text;

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
            throw new ArgumentException("Fetch<T> only supports an entity with a [Key] or Id property");

        //var hasTenantIdProperty = HasTenantIdProperty(currenttype);
        //if (!hasTenantIdProperty)
        //    throw new ArgumentException("Fetch<T> only supports an entity with a TenantId property");

        int tenantId = currentUser.TenantId;
        if (tenantId == 0)
            throw new ArgumentException("Fetch<T> only supports an entity with a TenantId");

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



    private static bool HasTenantIdProperty(Type type)
    {
        // return type.GetProperties().Where(p => p.Name.Equals("TenantId", StringComparison.OrdinalIgnoreCase));
        return type.GetProperty("TenantId") != null;
    }
}
