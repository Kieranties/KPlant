namespace KPlant.Sequence.Model
{
    public partial class Arrow
    {
        public static Arrow Default() => new Arrow();

        public static Arrow Dotted() => new Arrow { Type = ArrowType.Dotted };
    }
}