using System.IO;
using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public interface IRenderer
    {
        Stream Stream { get; }

        RenderingOptions Options { get; }
        
        Task Render<TModel>(TModel model);

        void Write(string value);

        void WriteLine(string value);

        Task WriteAsync(string value);

        Task WriteLineAsync(string value);
    }
}
