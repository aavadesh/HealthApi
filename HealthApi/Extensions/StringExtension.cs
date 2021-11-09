using System.Text.RegularExpressions;

namespace HealthApi.Extensions
{
    public static class StringExtension
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens
            return str;
        }
    }
}
