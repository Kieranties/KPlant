using System.Collections.Generic;
using System.Threading.Tasks;
using KPlant.Rendering;
using System;

namespace KPlant.Sequence.Model
{
    public class Group : ISequenceElement
    {
        public List<ISequenceElement> Elements { get; } = new List<ISequenceElement>();

        public string Label { get; set; } = null;

        public GroupType Type { get; set; } = GroupType.Group;

        public List<Group> Else { get; } = new List<Group>();

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            await WriteGroup(Type.ToString(), this, renderer);
            await renderer.WriteLineAsync("end");
        }

        protected async Task WriteGroup(string type, Group group, IRenderer renderer)
        {            
            var startLine = type.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(group.Label))
                startLine += $" {group.Label}";

            await renderer.WriteLineAsync(startLine);
            group.Elements.ForEach(async e => await e.Render(renderer));
            group.Else.ForEach(async e => await WriteGroup("else", e, renderer));

        }
    }

    public enum GroupType
    {
        Alt,
        Opt,
        Loop,
        Par,
        Break,
        Critical,
        Group
    }
}