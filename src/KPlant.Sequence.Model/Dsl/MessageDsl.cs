namespace KPlant.Sequence.Model
{
    public partial class Message
    {
        public static Message Between(Participant from, Participant to, string label = null) => new Message(from, to, label);
    }
}