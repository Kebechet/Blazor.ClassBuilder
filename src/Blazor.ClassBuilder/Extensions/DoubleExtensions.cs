using System.Globalization;

namespace Blazor.ClassBuilder.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Converts a numeric value to a CSS-safe string.
        /// Uses InvariantCulture to ensure decimal point (.) instead of locale-specific comma (,) for valid CSS.
        /// </summary>
        public static string ToCssString(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
