using System.Threading.Tasks;
using KPlant.Rendering;
using System;

namespace KPlant.Sequence.Model
{
    public class ArrowHead : IRenderable
    {        
        public ArrowHeadThickness Thickness { get; set; } = ArrowHeadThickness.Normal;
        public ArrowHeadParts Parts { get; set; } = ArrowHeadParts.Normal;
        public ArrowHeadStatus Status { get; set; } = ArrowHeadStatus.Normal;

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

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

            await renderer.WriteAsync(headSymbol);
        }
    }

    public enum ArrowHeadThickness
    {
        Normal,
        Thin
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