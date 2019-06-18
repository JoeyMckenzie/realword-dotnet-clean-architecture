namespace Conduit.Shared.Extensions
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        /// <summary>
        /// Validates the string value is not null, or empty.
        /// </summary>
        /// <param name="value">Reference string value</param>
        /// <returns>True if the value exists and is empty</returns>
        public static bool IsValidEmptyString(this string value)
        {
            return value != null && value.Length == 0;
        }

        /// <summary>
        /// Converts the string sentence to hyphenated slug.
        /// </summary>
        /// <param name="value">Reference string sentence</param>
        /// <returns>Slugified version of the phrase</returns>
        public static string ToSlug(this string value)
        {
            // Regex validators
            var wordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);
            var invalidCharacters = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);
            var multipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

            // Normalize the case
            value = value.ToLowerInvariant();

            // Remove diacritics (accents)
            value = RemoveDiacritics(value);

            // Replace all word delimiters with hyphens
            value = wordDelimiters.Replace(value, "-");

            // Remove invalid characters
            value = invalidCharacters.Replace(value, string.Empty);

            // Replace multiple hyphens with single hyphen
            value = multipleHyphens.Replace(value, "-");

            // Trim hyphens from the end
            return value.Trim('-');
        }

        /// <summary>
        /// Removes all special accent characters from the string value.
        /// <see cref="http://www.siao2.com/2007/05/14/2629747.aspx"/>
        /// </summary>
        /// <param name="rawStringValue">Reference string value possibly containing accent characters</param>
        /// <returns>Scrubbed string with removed accents</returns>
        private static string RemoveDiacritics(string rawStringValue)
        {
            var normalizedStringValue = rawStringValue.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var normalizedCharacter in normalizedStringValue)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(normalizedCharacter);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(normalizedCharacter);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}