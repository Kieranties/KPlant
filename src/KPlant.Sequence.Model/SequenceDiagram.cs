using KPlant.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using KPlant.Model;

namespace KPlant.Sequence.Model
{
    public partial class SequenceDiagram : IRenderable, IElementCollection<ISequenceElement>
    {
        private const string START_UML = "@startuml";
        private const string END_UML = "@enduml";

        public List<ISequenceElement> Elements { get; set; } = new List<ISequenceElement>();

        public IEnumerator<ISequenceElement> GetEnumerator() => Elements.GetEnumerator();

        public void Add(ISequenceElement element) => Elements.Add(element);

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            await renderer.WriteLineAsync(START_UML);
            foreach (var entry in Elements)
            {
                await entry.Render(renderer);
            }
            await renderer.WriteLineAsync(END_UML);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}