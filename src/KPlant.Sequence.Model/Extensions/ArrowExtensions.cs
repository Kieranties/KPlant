namespace KPlant.Sequence.Model
{
    public static class ArrowExtensions
    {
        public static Arrow Head(this Arrow instance, ArrowHead head)
        {
            instance.Head = head;
            return instance;
        }
    }
}