namespace KPlant
{
    public static class KPlantStringExtensions
    {
        public static string FixNewlinesForOutput(this string value)
        {
            // replace newline
            return value
                .Replace("\r\n", "\\n")
                .Replace("\n", "\\n");
        }

        public static string EnsureQuotes(this string value)
        {
            return $"\"{value}\"";
        }
    }
}
