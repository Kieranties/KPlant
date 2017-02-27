using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public interface IRenderer
    {
        void Write(string value);

        void WriteLine(string value);

        Task WriteAsync(string value);

        Task WriteLineAsync(string value);
    }
}
