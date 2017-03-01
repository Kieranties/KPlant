using System;

namespace KPlant.Rendering
{
    public class RendererOptions : IRendererOptions
    {
        public string IndentMarker { get; set; } = "\t";

        public string LineEnding { get; set; } = Environment.NewLine;
    }
}