using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FoodTruckApp.Code
{
    public class MyUrl
    {
        public static string Status(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            string slug = name.ToLowerInvariant();

            slug = RemoveDiacritics(slug);
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');

            if (slug.Length > 80)
                slug = slug.Substring(0, 80);

            return slug;
        }

        private static string RemoveDiacritics(string name)
        {
            var normalized = name.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
