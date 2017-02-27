using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Participant : ISequenceElement
    {
        public string Id { get; set; }

        public ParticipantType Type { get; set; } = ParticipantType.Participant;

        public async Task Render(IRenderer renderer)
        {
            var line = $"{Type} {Id}";
            await renderer.WriteLineAsync(line);
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