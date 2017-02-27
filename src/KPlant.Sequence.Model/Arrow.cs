using System;
using System.Threading.Tasks;
using KPlant.Rendering;

namespace KPlant.Sequence.Model
{
    public class Arrow : IRenderable
    {
        // Simple statics
        public static Arrow Default = new Arrow();
        public static Arrow Dotted = new Arrow { LineType = ArrowLineType.Dotted };
                
        public ArrowLineType LineType { get; set; } = ArrowLineType.Normal;

        public ArrowHead Head { get; set; } = new ArrowHead();

        public async Task Render(IRenderer renderer)
        {
            await renderer.WriteAsync("-");            
            
            // Line type
            if (LineType == ArrowLineType.Dotted)
            {
                await renderer.WriteAsync("-");
            }

            await Head.Render(renderer);
        }
    }

    public class ArrowHead : IRenderable
    {        
        public ArrowHeadThickness Thickness { get; set; } = ArrowHeadThickness.Normal;
        public ArrowHeadParts Parts { get; set; } = ArrowHeadParts.Both;
        public ArrowHeadStatus Status { get; set; } = ArrowHeadStatus.Normal;

        public virtual async Task Render(IRenderer renderer)
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
                case ArrowHeadParts.Both:
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

    public enum ArrowLineType
    {
        Normal,
        Dotted
    }

    public enum ArrowHeadThickness
    {
        Normal,
        Thin
    }

    public enum ArrowHeadParts
    {
        Top,
        Bottom,
        Both
    }

    public enum ArrowHeadStatus
    {
        Success,
        Fail,
        Normal
    }
}