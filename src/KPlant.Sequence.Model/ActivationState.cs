using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class ActivationStatus : ISequenceElement
    {
        // static resolvers
        public static ActivationStatus Activate(Participant participant) => new ActivationStatus(participant) { State = ActivationState.Activate };
        public static ActivationStatus Deactivate(Participant participant) => new ActivationStatus(participant) { State = ActivationState.Deactivate };
        public static ActivationStatus Destroy(Participant participant) => new ActivationStatus(participant) { State = ActivationState.Destroy };


        public ActivationStatus(Participant participant)
        {
            Participant = participant ?? throw new ArgumentNullException(nameof(participant));
        }

        public Participant Participant { get; }

        public ActivationState State { get; set; } = ActivationState.Activate;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));
            
            var output = $"{State.ToString().ToLowerInvariant()} {Participant.Id}";
            await renderer.WriteLineAsync(output);
        }
    }

    public enum ActivationState
    {
        Activate,
        Deactivate,
        Destroy,
    }
}
