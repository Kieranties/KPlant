using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Page : ISequenceElement
    {
        public Page(string title = null)
        {
            Title = title;
        }

        public string Title { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            var output = "newpage";
            if (!string.IsNullOrWhiteSpace(Title))
                output += $" {Title.FixNewlinesForOutput()}";

            await renderer.WriteLineAsync(output);
        }
    }
}