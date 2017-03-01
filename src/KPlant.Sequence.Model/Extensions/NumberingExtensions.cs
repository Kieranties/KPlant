namespace KPlant.Sequence.Model
{
    public static class NumberingExtensions
    {
        public static Numbering WithFormat(this Numbering instance, string format)
        {
            instance.Format = format;
            return instance;
        }
    }
}