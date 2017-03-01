using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Arrow : IStringRenderable
    {
        // Simple statics
        public static Arrow Default => new Arrow();

        public static Arrow Dotted => new Arrow { Type = ArrowType.Dotted };

        public string Colour { get; set; } = null;

        public ArrowHead Head { get; set; } = new ArrowHead();

        public ArrowType Type { get; set; } = ArrowType.Normal;

        public string Render()
        {
            if (Head == null)
                throw new MissingRenderingDataException(nameof(Head), typeof(Arrow));

            var output = "-";

            if (!string.IsNullOrWhiteSpace(Colour))
            {
                output += $"[#{Colour}]";
            }

            // Line type
            if (Type == ArrowType.Dotted)
            {
                output += "-";
            }

            output += Head.Render();

            return output;
        }
    }

    public enum ArrowType
    {
        Normal,
        Dotted
    }
}