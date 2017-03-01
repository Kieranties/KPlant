namespace KPlant.Sequence.Model
{
    public static class Separator
    {
        public static Delay Delay(string label = null) => new Delay(label);

        public static Divider Divider(string label = null) => new Divider(label);

        public static Page Page(string title = null) => new Page(title);

        public static Space Space(int? height = null) => new Space(height);
    }
}