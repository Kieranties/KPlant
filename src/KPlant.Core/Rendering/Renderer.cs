using System;
using System.Linq;
using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public abstract class Renderer<TOptions> : IRenderer
        where TOptions : IRendererOptions
    {
        protected Renderer(TOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            Options = options;
        }

        protected String IndentString { get; set; } = string.Empty;

        protected TOptions Options { get; set; }

        public void Indent(int count)
        {
            IndentString += string.Concat(Enumerable.Repeat(Options.IndentMarker, count));
        }

        public void Outdent(int count)
        {
            var markerLength = Options.IndentMarker.Length;
            var indentLength = IndentString.Length / markerLength;

            if (indentLength < count)
            {
                IndentString = string.Empty;
                return;
            }

            IndentString = IndentString.Remove(0, markerLength * count);
        }

        public abstract void WriteLine(string value);

        public abstract Task WriteLineAsync(string value);

        protected virtual string FormatForWrite(string value) => $"{IndentString}{value}{Options.LineEnding}";
    }
}