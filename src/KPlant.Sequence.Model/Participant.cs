using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Participant : ISequenceElement
    {
        public string Id { get; set; } = null;

        public ParticipantType Type { get; set; } = ParticipantType.Participant;

        public string Label { get; set; } = null;

        public string Colour { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (string.IsNullOrWhiteSpace(Id))
                throw new MissingRenderingDataException(nameof(Id), GetType());

            await renderer.WriteAsync(Type.ToString().ToLowerInvariant());
            if (!string.IsNullOrWhiteSpace(Label))
            {                
                await renderer.WriteAsync($" \"{Label.FixNewlinesForOutput()}\" as {Id}");
            }
            else
            {
                await renderer.WriteAsync($" {Id}");
            }

            if (!string.IsNullOrWhiteSpace(Colour))
            {
                await renderer.WriteAsync($" #{Colour}");
            }
            
            await renderer.WriteLineAsync(string.Empty);
        }
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