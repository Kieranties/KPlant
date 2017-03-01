using KPlant.Model;
using KPlant.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPlant.Sequence.Model
{
    public partial class Group : ISequenceElement, IElementCollection<ISequenceElement>, IEditableLabel
    {
        public Group(GroupType type, string label = null)
        {
            Type = type;
            Label = label;
        }

        public Group(string label = null) : this(GroupType.Group, label)
        {
        }

        public List<ISequenceElement> Elements { get; set; } = new List<ISequenceElement>();

        public List<Group> Else { get; set; } = new List<Group>();

        public string Label { get; set; } = null;

        public GroupType Type { get; }

        public void Add(ISequenceElement element) => Elements.Add(element);

        public IEnumerator<ISequenceElement> GetEnumerator() => Elements.GetEnumerator();

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            await WriteGroup(Type.ToString(), this, renderer);

            await renderer.WriteLineAsync("end");
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected async Task WriteGroup(string type, Group group, IRenderer renderer)
        {
            var output = type.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(group.Label))
                output += $" {group.Label}";

            await renderer.WriteLineAsync(output);
            renderer.Indent();
            group.Elements.ForEach(async e => await e.Render(renderer));
            renderer.Outdent();
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