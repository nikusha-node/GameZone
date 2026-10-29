using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GameZone.Api.Helpers;

public static class SlugHelper
{

    public static string Generate(string text)
    {
        var decomposed = text.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();
        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return Regex.Replace(sb.ToString(), "[^a-z0-9]+", "-").Trim('-');
    }
}