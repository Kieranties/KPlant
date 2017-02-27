using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Arrow : IRenderable
    {
        // Simple statics
        public static Arrow Default => new Arrow();
        public static Arrow Dotted => new Arrow { Type = ArrowType.Dotted };
                
        public ArrowType Type { get; set; } = ArrowType.Normal;

        public string Colour { get; set; } = null;

        public ArrowHead Head { get; set; } = new ArrowHead();

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (Head == null)
                throw new MissingRenderingDataException(nameof(Head), typeof(Arrow));

            await renderer.WriteAsync("-");

            if (!string.IsNullOrWhiteSpace(Colour))
            {
                await renderer.WriteAsync($"[#{Colour}]");
            }

            // Line type
            if (Type == ArrowType.Dotted)
            {
                await renderer.WriteAsync("-");
            }

            await Head.Render(renderer);
        }
    }

    public enum ArrowType
    {
        Normal,
        Dotted
    }    
}