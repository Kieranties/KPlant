using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Space : ISequenceElement
    {
        public Space(int? height = null)
        {
            Height = height;
        }

        public int? Height { get; }

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
