namespace KPlant.Sequence.Model
{
    public static class MessageExtensions
    {
        public static Message WithArrow(this Message instance, Arrow arrow)
        {
            instance.Arrow = arrow;
            return instance;
        }
    }
}
