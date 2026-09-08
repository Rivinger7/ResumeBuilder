using System.Text.RegularExpressions;

namespace ResumeBuilder.Domain.Helpers;

public static class CustomString
{
    public static string ToSpacedWords(this string value)
    {
        return Regex.Replace(value, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1").TrimStart();
    }
}
