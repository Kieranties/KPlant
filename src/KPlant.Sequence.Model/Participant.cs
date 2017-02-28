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

            var output = Type.ToString().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(Label))
            {                
                output += $" {Label.FixNewlinesForOutput().EnsureQuotes()} as {Id}";
            }
            else
            {
                output += $" {Id}";
            }

            if (!string.IsNullOrWhiteSpace(Colour))
            {
                output += $" #{Colour}";
            }
            
            await renderer.WriteLineAsync(output);
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