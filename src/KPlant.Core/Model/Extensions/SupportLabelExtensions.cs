namespace KPlant.Model
{
    public static class SupportLabelExtensions
    {
        public static T Label<T>(this T instance, string label)
            where T : ISupportLabel
        {
            instance.Label = label;
            return instance;
        }
    }
}