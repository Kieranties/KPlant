using KPlant.Model;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public partial class Arrow : ISupportColour
    {
        public string Colour { get; set; } = null;

        public ArrowHead FromHead { get; set; } = null;

        public ArrowHead Head { get; set; } = new ArrowHead();

        public ArrowThickness Thickness { get; set; } = ArrowThickness.Normal;

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

            if (Type == ArrowType.Dotted)
            {
                output += "-";
            }

            output += Head.Render(Thickness);

            if (FromHead != null)
            {
                output = FromHead.Render(Thickness, true) + output;
            }

            return output;
        }
    }

    public enum ArrowThickness
    {
        Normal,
        Thin
    }

    public enum ArrowType
    {
        Normal,
        Dotted
    }
}