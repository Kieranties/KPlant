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
    }
}
