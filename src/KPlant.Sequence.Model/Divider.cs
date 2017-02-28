using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Divider : ISequenceElement
    {
        public string Label { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            await renderer.WriteAsync("==");
            if(!string.IsNullOrWhiteSpace(Label))
                await renderer.WriteAsync($" {Label.FixNewlinesForOutput()} ");
            await renderer.WriteLineAsync("==");
        }
    }
}
