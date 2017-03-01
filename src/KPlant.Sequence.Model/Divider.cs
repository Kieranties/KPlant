using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Divider : ISequenceElement
    {
        public Divider(string label = null)
        {
            Label = label;
        }

        public string Label { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            var output = "==";
            if(!string.IsNullOrWhiteSpace(Label))
                output += $" {Label.FixNewlinesForOutput()} ";
            output += "==";

            await renderer.WriteLineAsync(output);
        }
    }
}
