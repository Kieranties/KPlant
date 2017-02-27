using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Page : ISequenceElement
    {
        public string Title { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            await renderer.WriteAsync("newpage");
            if (!string.IsNullOrWhiteSpace(Title))
                await renderer.WriteAsync(" " + Title.FixNewlinesForOutput());

            await renderer.WriteLineAsync(string.Empty);
        }
    }
}