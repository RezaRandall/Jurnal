using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using static Dapper.SimpleCRUD;

namespace TabpediaFin.Infrastructure.Utility;

public static class SqlHelper
{
    private static readonly ConcurrentDictionary<Type, string> TableNames = new ConcurrentDictionary<Type, string>();
    private static readonly ConcurrentDictionary<string, string> ColumnNames = new ConcurrentDictionary<string, string>();

    private static ITableNameResolver _tableNameResolver = new TableNameResolver();
    private static IColumnNameResolver _columnNameResolver = new ColumnNameResolver();


    public static string GenerateWhere<T>()
    {
        var sql = string.Empty;

        PropertyInfo[] propCollection = typeof(T).GetProperties();
        foreach (PropertyInfo property in propCollection)
        {
            foreach (var attribute in property.GetCustomAttributes(true))
            {
                var fieldName = property.Name;
                if (attribute is ColumnAttribute)
                {
                    fieldName = (attribute as ColumnAttribute)?.Name ?? string.Empty;
                }

                if (attribute is SearchableAttribute)
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        sql += " OR ";
                    }
                    sql += @$" LOWER(""{fieldName}"") LIKE @Search ";
                }
            }
        }

        // if (!string.IsNullOrWhiteSpace(sql)) sql += ") ";
        return $" {sql} ";
    }


    public static string GenerateOrderBy(string sort, bool isDesc = false, string defaultSort = @" ""Name"" ")
    {
        if (string.IsNullOrWhiteSpace(sort)) 
        {
            return defaultSort;
        }

        return GenerateOrderBy(sort, isDesc);
    }


    private static string GenerateOrderBy(string sort, bool isDesc)
    {
        if (string.IsNullOrWhiteSpace(sort)) return string.Empty;
        var colname = sort.ToPascalCase();
        var order = isDesc ? " DESC " : "";

        return $@" ""{colname}"" {order} ";
    }
}
