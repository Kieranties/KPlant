namespace KPlant.Sequence.Model
{
    public class Participant : ISequenceElement
    {
        public string Id { get; set; }

        public ParticipantType Type { get; set; } = ParticipantType.Participant;
    }

    public enum ParticipantType
    {
        Participant,
        Actor,
        Boundary,
        Control,
        Entity,
        Database
    }
}