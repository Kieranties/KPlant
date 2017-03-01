using KPlant.Model;
using KPlant.Rendering;
using System;
using System.Threading.Tasks;

namespace KPlant.Sequence.Model
{
    public class Page : ISequenceElement, ISupportLabel
    {
        public Page(string label = null)
        {
            Label = label;
        }

        public string Label { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            var output = "newpage";
            if (!string.IsNullOrWhiteSpace(Label))
                output += $" {Label.FixNewlinesForOutput()}";

            await renderer.WriteLineAsync(output);
        }
    }
}