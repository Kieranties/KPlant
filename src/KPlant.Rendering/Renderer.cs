using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public class Renderer : IRenderer
    {
        protected ConcurrentDictionary<Type, object> _rendererCache = new ConcurrentDictionary<Type, object>();

        public Renderer(Stream stream, RenderingOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public virtual Stream Stream { get; protected set; }

        public virtual RenderingOptions Options { get; protected set; }
        
        public async Task Render<TModel>(TModel model)
        {
            var renderer = Resolve<TModel>();
            await renderer.Render(model, this);
        }

        public void Write(string value)
        {
            var bytes = Encode(value);
            Stream.Write(bytes, 0, bytes.Length);
        }

        public async Task WriteAsync(string value)
        {
            var bytes = Encode(value);
            await Stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }

        public void WriteLine(string value) => Write($"{value}{Options.LineEnding}");

        public Task WriteLineAsync(string value) => WriteAsync($"{value}{Options.LineEnding}");        

        protected byte[] Encode(string value) => Options.Encoding.GetBytes(value);

        protected virtual IModelRenderer<TModel> Resolve<TModel>()
        {
            var instance = _rendererCache.GetOrAdd(typeof(TModel), _ =>
            {
                var type = Options.ResolveRenderer(typeof(TModel));
                if (type != null)
                {
                    return Activator.CreateInstance(type);
                }
                throw new Exception($"Could not resolve renderer for {typeof(TModel)}");
            });

            return instance as IModelRenderer<TModel>;
        }
    }
}
