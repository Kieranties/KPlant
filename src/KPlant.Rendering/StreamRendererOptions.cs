using System.Text;

namespace KPlant.Rendering
{
    public class StreamRendererOptions : RendererOptions
    {
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}