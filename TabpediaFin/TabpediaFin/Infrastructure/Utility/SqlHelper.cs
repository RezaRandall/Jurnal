using System.Reflection;

namespace TabpediaFin.Infrastructure.Utility;

public static class SqlHelper
{
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
                    sql += @$" AND LOWER(""{fieldName}"") LIKE @Search ";
                }
            }
        }

        // if (!string.IsNullOrWhiteSpace(sql)) sql += ") ";
        return sql;
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
