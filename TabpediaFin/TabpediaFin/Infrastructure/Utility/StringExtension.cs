using System.Text.RegularExpressions;

namespace TabpediaFin.Infrastructure.Utility;

public static class StringExtension
{
    public static string ToCamelCase(this string str)
    {
        var words = str.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);

        var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
            m =>
            {
                return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
            });

        var tailWords = words.Skip(1)
            .Select(word => char.ToUpper(word[0]) + word.Substring(1))
            .ToArray();

        return $"{leadWord}{string.Join(string.Empty, tailWords)}";
    }


    public static string ToPascalCase(this string str)
    {
        var camelCase = str.ToCamelCase();
        return string.Concat(camelCase[0].ToString().ToUpper(), camelCase.AsSpan(1));
    }
}
