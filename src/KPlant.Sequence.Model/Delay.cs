using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Delay : ISequenceElement
    {
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
