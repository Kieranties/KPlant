using System.Collections.Generic;
using System.Threading.Tasks;
using KPlant.Rendering;
using System;
using KPlant.Model;
using System.Collections;

namespace KPlant.Sequence.Model
{
    public class Group : ISequenceElement, IElementCollection<ISequenceElement>
    {
        private static Group Create( GroupType type, ISequenceElement[] elements)
        {
            return new Group
            {
                Type = type,
                Elements = new List<ISequenceElement>(elements)
            };
        }

        public static Group Alt(params ISequenceElement[] elements) => Create(GroupType.Alt, elements);
        public static Group Opt(params ISequenceElement[] elements) => Create(GroupType.Opt, elements);
        public static Group Loop(params ISequenceElement[] elements) => Create(GroupType.Loop, elements);
        public static Group Par(params ISequenceElement[] elements) => Create(GroupType.Par, elements);
        public static Group Break(params ISequenceElement[] elements) => Create(GroupType.Break, elements);
        public static Group Critical(params ISequenceElement[] elements) => Create(GroupType.Critical, elements);
        public static Group For(params ISequenceElement[] elements) => Create(GroupType.Group, elements);
        
        
        public List<ISequenceElement> Elements { get; set; } = new List<ISequenceElement>();

        public string Label { get; set; } = null;

        public GroupType Type { get; set; } = GroupType.Group;

        public List<Group> Else { get; set; } = new List<Group>();

        public void Add(ISequenceElement element) => Elements.Add(element);

        public IEnumerator<ISequenceElement> GetEnumerator() => Elements.GetEnumerator();

        public async Task Render(IRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            await WriteGroup(Type.ToString(), this, renderer);
            
            await renderer.WriteLineAsync("end");
        }

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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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


    public static class GroupExtensions
    {
        public static Group WithLabel(this Group group, string label)
        {
            group.Label = label;
            return group;
        }

        public static Group WithElse(this Group group, params Group[] @else)
        {
            group.Else.AddRange(@else);
            return group;
        }
    }
}