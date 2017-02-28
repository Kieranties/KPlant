using System;
using System.IO;
using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public class Renderer : IRenderer
    {
        public Renderer(Stream stream, RenderingOptions options = null)
        {            
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Options = options ?? new RenderingOptions();
        }

        public virtual Stream Stream { get; protected set; }

        public virtual RenderingOptions Options { get; protected set; }
        
        public void WriteLine(string value)
        {
            var bytes = Encode(value);
            Stream.Write(bytes, 0, bytes.Length);
        }
        
        public async Task WriteLineAsync(string value)
        {
            var bytes = Encode(value);
            await Stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }

        protected byte[] Encode(string value)
        {
            return Options.Encoding.GetBytes($"{value}{Options.LineEnding}");
        }
    }    
}
