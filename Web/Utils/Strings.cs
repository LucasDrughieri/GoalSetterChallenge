using System.Text.RegularExpressions;

namespace Web.Helpers
{
    public static class Strings
    {
        public static string RemoveAllNonPrintableCharacters(string target)
        {
            return Regex.Replace(target, @"\p{C}+", string.Empty);
        }
    }
}
