using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class ActivationStatus : ISequenceElement
    {
        public Participant Participant { get; set; }

        public ActivationState State { get; set; } = ActivationState.Activate;

        public async Task Render(IRenderer renderer)
        {

            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (Participant == null)
                throw new MissingRenderingDataException(nameof(Participant), typeof(ActivationStatus));

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
