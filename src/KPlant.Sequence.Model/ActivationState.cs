using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public partial class ActivationStatus : ISequenceElement
    {
        public ActivationStatus(Participant participant, ActivationState state)
        {
            Participant = participant ?? throw new ArgumentNullException(nameof(participant));
            State = state;
        }

        public Participant Participant { get; }

        public ActivationState State { get; }

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
