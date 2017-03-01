namespace KPlant.Model
{
    public static class SupportColourExtensions
    {
        public static T Colour<T>(this T instance, string colour)
            where T : ISupportColour
        {
            instance.Colour = colour;
            return instance;
        }
    }
}