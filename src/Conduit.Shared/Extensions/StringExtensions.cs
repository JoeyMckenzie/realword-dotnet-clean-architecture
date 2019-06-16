namespace Conduit.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool ExistsAndIsValid(this string value)
        {
            return value != null && value.Length == 0;
        }
    }
}