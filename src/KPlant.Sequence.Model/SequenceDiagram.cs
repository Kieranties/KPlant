using KPlant.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPlant.Sequence.Model
{
    public class SequenceDiagram : IRenderable
    {
        private const string START_UML = "@startuml";
        private const string END_UML = "@enduml";

        public List<ISequenceElement> Elements { get; } = new List<ISequenceElement>();

        public async Task Render(IRenderer renderer)
        {
            await renderer.WriteLineAsync(START_UML);
            foreach (var entry in Elements)
            {
                await entry.Render(renderer);
            }
            await renderer.WriteLineAsync(END_UML);
        }
    }
}