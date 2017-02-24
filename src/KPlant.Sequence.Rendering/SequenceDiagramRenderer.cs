using KPlant.Rendering;
using KPlant.Sequence.Model;
using System.Threading.Tasks;

namespace KPlant.Sequence.Rendering
{
    public class SequenceDiagramRenderer : DiagramRenderer<SequenceDiagram>
    {
        protected override async Task RenderDiagram(SequenceDiagram diagram, IRenderer renderer)
        {
            foreach (var entry in diagram.Elements)
            {
                await renderer.Render(entry);
            }
        }
    }
}
