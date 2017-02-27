using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public interface IRenderable
    {
        Task Render(IRenderer renderer);
    }
}
