namespace KPlant.Sequence.Model
{
    public class Message : ISequenceElement
    {
        public Participant From { get; set; }

        public Participant To { get; set; }

        public string Label { get; set; }
    }
}