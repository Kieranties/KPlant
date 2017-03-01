using System;

namespace KPlant.Sequence.Model
{
    public class ArrowHead
    {
        public ArrowHeadParts Parts { get; set; } = ArrowHeadParts.Normal;

        public ArrowHeadStatus Status { get; set; } = ArrowHeadStatus.Normal;

        public string Render(ArrowThickness thickness, bool invert = false)
        {
            var output = string.Empty;
            string[] topParts = invert ? new[] { "/", @"\", "<" } : new[] { @"\", "/", ">" };
            switch (Parts)
            {
                case ArrowHeadParts.Top:
                    output = topParts[0];
                    break;

                case ArrowHeadParts.Bottom:
                    output = topParts[1];
                    break;

                case ArrowHeadParts.Normal:
                    output = topParts[2];
                    break;
            }

            if (thickness == ArrowThickness.Thin)
                output += output;

            var statusString = string.Empty;
            switch (Status)
            {
                case ArrowHeadStatus.Success:
                    statusString = "o";
                    break;

                case ArrowHeadStatus.Fail:
                    statusString = "x";
                    break;

                case ArrowHeadStatus.Normal:
                    break;
            }

            output = invert ? statusString + output : output + statusString;

            return output;
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
}