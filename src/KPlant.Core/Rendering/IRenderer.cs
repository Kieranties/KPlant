using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public interface IRenderer
    {
        void WriteLine(string value);

        Task WriteLineAsync(string value);
    }
}
