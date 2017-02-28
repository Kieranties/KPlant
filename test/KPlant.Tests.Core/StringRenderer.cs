using KPlant.Rendering;
using System.Text;
using System.Threading.Tasks;

namespace KPlant.Tests.Core
{
    public class StringRenderer : Renderer<RendererOptions>
    {
        private StringBuilder _sb = new StringBuilder();

        public StringRenderer() : base(new RendererOptions())
        {

        }

        public StringRenderer(RendererOptions options)
            :base(options)
        {

        }

        public override void WriteLine(string value) => _sb.Append(FormatForWrite(value));

        public override Task WriteLineAsync(string value)
        {
            WriteLine(value);
            return Task.CompletedTask;
        }        

        public string Value => _sb.ToString();        
    }
}
