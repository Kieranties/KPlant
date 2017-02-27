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
        
        public void Write(string value)
        {
            var bytes = Encode(value);
            Stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteLine(string value) => Write($"{value}{Options.LineEnding}");

        public async Task WriteAsync(string value)
        {
            var bytes = Encode(value);
            await Stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }
        
        public Task WriteLineAsync(string value) => WriteAsync($"{value}{Options.LineEnding}");
        
        protected byte[] Encode(string value) => Options.Encoding.GetBytes(value);        
    }    
}
