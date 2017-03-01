namespace KPlant.Sequence.Model
{
    public partial class ActivationStatus
    {
        public static ActivationStatus Activate(Participant participant) => new ActivationStatus(participant, ActivationState.Activate);
        public static ActivationStatus Deactivate(Participant participant) => new ActivationStatus(participant, ActivationState.Deactivate);
        public static ActivationStatus Destroy(Participant participant) => new ActivationStatus(participant, ActivationState.Destroy);
    }
}
