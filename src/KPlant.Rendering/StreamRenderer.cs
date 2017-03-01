using System;
using System.IO;
using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public class StreamRenderer : Renderer<StreamRendererOptions>
    {
        public StreamRenderer(Stream stream, StreamRendererOptions options = null)
            : base(options ?? new StreamRendererOptions())
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        protected Stream Stream { get; set; }

        public override void WriteLine(string value)
        {
            var bytes = Encode(value);
            Stream.Write(bytes, 0, bytes.Length);
        }

        public override async Task WriteLineAsync(string value)
        {
            var bytes = Encode(value);
            await Stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }

        protected byte[] Encode(string value)
        {
            return Options.Encoding.GetBytes(FormatForWrite(value));
        }
    }
}