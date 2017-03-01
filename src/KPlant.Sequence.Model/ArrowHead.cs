using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class ArrowHead : IStringRenderable
    {
        public ArrowHeadParts Parts { get; set; } = ArrowHeadParts.Normal;

        public ArrowHeadStatus Status { get; set; } = ArrowHeadStatus.Normal;

        public ArrowHeadThickness Thickness { get; set; } = ArrowHeadThickness.Normal;

        public string Render()
        {
            string headSymbol = null;
            switch (Parts)
            {
                case ArrowHeadParts.Top:
                    headSymbol = @"\";
                    break;

                case ArrowHeadParts.Bottom:
                    headSymbol = "/";
                    break;

                case ArrowHeadParts.Normal:
                    headSymbol = ">";
                    break;
            }

            if (Thickness == ArrowHeadThickness.Thin)
                headSymbol += headSymbol;

            switch (Status)
            {
                case ArrowHeadStatus.Success:
                    headSymbol += "o";
                    break;

                case ArrowHeadStatus.Fail:
                    headSymbol += "x";
                    break;

                case ArrowHeadStatus.Normal:
                    break;
            }

            return headSymbol;
        }
    }

    public enum ArrowHeadParts
    {
        Normal,
        Top,
        Bottom
    }

    public enum ArrowHeadStatus
    {
        Normal,
        Success,
        Fail
    }

    public enum ArrowHeadThickness
    {
        Normal,
        Thin
    }
}