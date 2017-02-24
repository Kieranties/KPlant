using System;
using System.Threading.Tasks;

namespace KPlant.Rendering
{
    public abstract class DiagramRenderer<TDiagram> : IModelRenderer<TDiagram>
    {
        private const string START_UML = "@startuml";
        private const string END_UML = "@enduml";

        public async Task Render(TDiagram model, IRenderer renderer)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));


            await renderer.WriteLineAsync(START_UML);
            await RenderDiagram(model, renderer);
            await renderer.WriteLineAsync(END_UML);
        }

        protected abstract Task RenderDiagram(TDiagram diagram, IRenderer renderer);
    }
}
