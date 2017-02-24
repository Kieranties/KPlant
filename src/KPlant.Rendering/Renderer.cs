using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace KPlant.Rendering
{
    public class Renderer : IRenderer
    {
        protected ConcurrentDictionary<Type, object> _rendererCache = new ConcurrentDictionary<Type, object>();
        
        public Renderer(Stream stream, RenderingOptions options = null)
        {            
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Options = options ?? new RenderingOptions();
        }

        public virtual Stream Stream { get; protected set; }

        public virtual RenderingOptions Options { get; protected set; }
        
        public async Task Render(object model)
        {
            //TODO: Fix up the reflection here - blurgh
            var modelType = model.GetType();
            var renderer = _rendererCache.GetOrAdd(modelType, _ =>
            {
                var type = Options.ResolveRenderer(modelType);
                if (type != null)
                {
                    return Activator.CreateInstance(type);
                }
                throw new Exception($"Could not resolve renderer for {modelType}");
            });

            var task = (Task)renderer
                .GetType()
                .GetRuntimeMethod("Render", new[] { modelType, typeof(IRenderer) })
                .Invoke(renderer, new[] { model, this });

            await task.ConfigureAwait(false);
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
    }
}
