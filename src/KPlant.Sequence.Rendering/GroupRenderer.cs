using KPlant.Rendering;
using KPlant.Sequence.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KPlant.Sequence.Rendering
{
    public class GroupRenderer : IModelRenderer<Group>
    {
        public async Task Render(Group model, IRenderer renderer)
        {
            await WriteGroup(model.Type.ToString(), model, renderer);
            await renderer.WriteLineAsync("end");
        }

        private async Task WriteGroup(string type, Group group, IRenderer renderer)
        {
            var startLine = type;
            if (!string.IsNullOrWhiteSpace(group.Label))
                startLine += $" {group.Label}";

            await renderer.WriteLineAsync(startLine);
            group.Elements.ForEach(async e => await renderer.Render(e));
            group.Else.ForEach(async e => await WriteGroup("else", e, renderer));
            
        }
    }
}
