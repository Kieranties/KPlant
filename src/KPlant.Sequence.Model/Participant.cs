using KPlant.Rendering;
using System;
using System.Threading.Tasks;

namespace KPlant.Sequence.Model
{
    public partial class Participant : ISequenceElement
    {
        public Participant(string id, string label = null, ParticipantType type = ParticipantType.Participant)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id), "Cannot be null or whitespace");

            Type = type;
            Id = id;
            Label = label;
        }

        public Participant(string id, ParticipantType type) : this(id, null, type)
        {
        }

        public string Colour { get; set; } = null;

        public string Id { get; }

        public string Label { get; set; } = null;

        public ParticipantType Type { get; }

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

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