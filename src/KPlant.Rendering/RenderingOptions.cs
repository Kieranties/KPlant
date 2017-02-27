using System;
using System.Text;

namespace KPlant.Rendering
{
    public class RenderingOptions
    {        
        public RenderingOptions(string lineEnding = null, Encoding encoding = null)
        {
            LineEnding = lineEnding ?? Environment.NewLine;
            Encoding = encoding ?? Encoding.UTF8;
        }

        public string LineEnding { get; }

        public Encoding Encoding { get; }  
    }
}
