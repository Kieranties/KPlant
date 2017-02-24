using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public interface IModelRenderer<TModel>
    {
        Task Render(TModel model, IRenderer renderer);
    }
}
