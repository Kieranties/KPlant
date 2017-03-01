namespace KPlant.Sequence.Model
{
    public partial class Participant
    {
        public static Participant Called(string id, string label = null) => new Participant(ParticipantType.Participant, id, label);
        public static Participant Actor(string id, string label = null) => new Participant(ParticipantType.Actor, id, label);
        public static Participant Boundary(string id, string label = null) => new Participant(ParticipantType.Boundary, id, label);
        public static Participant Control(string id, string label = null) => new Participant(ParticipantType.Control, id, label);
        public static Participant Entity(string id, string label = null) => new Participant(ParticipantType.Entity, id, label);
        public static Participant Database(string id, string label = null) => new Participant(ParticipantType.Database, id, label);
    }    
}