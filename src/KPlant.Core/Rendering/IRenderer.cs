using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public interface IRenderer
    {
        void Indent(int count);

        void Outdent(int count);

        void WriteLine(string value);

        Task WriteLineAsync(string value);
    }

    public static class RendererExtensions
    {
        public static void Indent(this IRenderer renderer)
        {
            renderer.Indent(1);
        }

        public static void Outdent(this IRenderer renderer)
        {
            renderer.Outdent(1);
        }
    }
}