using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KPlant.Rendering;
using System.Linq;

namespace KPlant.Sequence.Model
{
    public class Ref : ISequenceElement
    {
        // multi-line wrapped statements currently not supported

        public List<Participant> Over { get; set; } = new List<Participant>();

        public string Label { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (Over == null || Over.Count == 0)
                throw new MissingRenderingDataException(nameof(Over), typeof(Ref));

            var output = "ref over ";
            output += string.Join(", ", Over.Select(x => x.Id));
            if (!string.IsNullOrWhiteSpace(Label))
                output += $" : {Label.FixNewlinesForOutput()}";

            await renderer.WriteLineAsync(output);
        }
    }
}
