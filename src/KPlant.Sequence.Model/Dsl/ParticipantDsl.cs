namespace KPlant.Sequence.Model
{
    public partial class Participant
    {
        public static Participant Actor(string id, string label = null) => new Participant(id, label, ParticipantType.Actor);

        public static Participant Boundary(string id, string label = null) => new Participant(id, label, ParticipantType.Boundary);

        public static Participant Called(string id, string label = null) => new Participant(id, label);

        public static Participant Control(string id, string label = null) => new Participant(id, label, ParticipantType.Control);

        public static Participant Database(string id, string label = null) => new Participant(id, label, ParticipantType.Database);

        public static Participant Entity(string id, string label = null) => new Participant(id, label, ParticipantType.Entity);
    }
}