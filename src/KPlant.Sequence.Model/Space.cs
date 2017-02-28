using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Space : ISequenceElement
    {
        public int? Height { get; set; } = null;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));
            
            var output = "||";
            if (Height.HasValue)
                output += $"{Height.Value}|";

            output += "|";
            await renderer.WriteLineAsync(output);
        }
    }
}
