using KPlant.Model;
using KPlant.Rendering;
using System;
using System.Threading.Tasks;

namespace KPlant.Sequence.Model
{
    public partial class Delay : ISequenceElement, IEditableLabel
    {
        public Delay(string label = null)
        {
            Label = label;
        }

        public string Label { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            var output = "...";
            if (!string.IsNullOrWhiteSpace(Label))
                output += $"{Label.FixNewlinesForOutput()}...";

            await renderer.WriteLineAsync(output);
        }
    }
}