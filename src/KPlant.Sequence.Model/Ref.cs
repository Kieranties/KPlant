using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KPlant.Rendering;
using System.Linq;

namespace KPlant.Sequence.Model
{
    public class Ref : ISequenceElement
    {
        public static Ref Over(string label, params Participant[] participants) => new Ref(label, participants);


        // multi-line wrapped statements currently not supported

        public Ref(string label, params Participant[] participants)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentOutOfRangeException(nameof(label));
            Participants = new List<Participant>(participants);
        }

        public List<Participant> Participants { get; }

        public string Label { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if(string.IsNullOrWhiteSpace(Label))
                throw new MissingRenderingDataException(nameof(Label), typeof(Ref));

            if (Participants == null || Participants.Count == 0)
                throw new MissingRenderingDataException(nameof(Participants), typeof(Ref));

            var output = "ref over ";
            output += string.Join(", ", Participants.Select(x => x.Id));
            output += $" : {Label.FixNewlinesForOutput()}";

            await renderer.WriteLineAsync(output);
        }
    }
}
