using HtmlAgilityPack;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace OnDemandTutor.Helper;

public static class ConverterHelper
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string SerializeObject(this object obj)
    {
        return JsonSerializer.Serialize(obj, options);
    }

    public static T? DeserializeObject<T>(this string jsonStr)
    {
        if (!string.IsNullOrEmpty(jsonStr))
            return JsonSerializer.Deserialize<T>(jsonStr, options);
        return default;
    }

    public static string Base64Encode(this string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(this string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static Guid ToGuid(this string guidStr)
    {
        if (Guid.TryParse(guidStr, out var result))
            return result;

        throw new DataException($"{guidStr} not is guild id");
    }

    public static string ToPascalCase(this string input)
    {
        return string.Join('.', input.Split('.').Select(x => ConvertToPascalCase(x)));
    }

    private static string ConvertToPascalCase(string input)
    {
        var invalidCharsRgx = new Regex(@"[^_a-zA-Z0-9]");
        var whiteSpace = new Regex(@"(?<=\s)");
        var startsWithLowerCaseChar = new Regex("^[a-z]");
        var firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
        var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
        var upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

        // replace white spaces with undescore, then replace all invalid chars with empty string
        var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(input, "_"), string.Empty)
            // split by underscores
            .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
            // set first letter to uppercase
            .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
            // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
            .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
            // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
            .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
            // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
            .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

        return string.Concat(pascalCase);
    }

    public static string? ConvertHtmlToPlainText(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        return htmlDoc.DocumentNode.InnerText;
    }

    public static string ToCamelCase(this string str)
    {
        var words = str.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
        var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
            m => { return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value; });
        var tailWords = words.Skip(1)
            .Select(word => char.ToUpper(word[0]) + word.Substring(1))
            .ToArray();
        return $"{leadWord}{string.Join(string.Empty, tailWords)}";
    }
}