using System;

namespace KPlant.Rendering
{
    public class RendererOptions : IRendererOptions
    {
        public string LineEnding { get; set; } = Environment.NewLine;

        public string IndentMarker { get; set; } = "\t";
    }
}
