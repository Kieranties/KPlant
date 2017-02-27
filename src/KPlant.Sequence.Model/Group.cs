using System.Collections.Generic;

namespace KPlant.Sequence.Model
{
    public class Group : ISequenceElement
    {
        public List<ISequenceElement> Elements { get; } = new List<ISequenceElement>();

        public string Label { get; set; }

        public GroupType Type { get; set; } = GroupType.Group;

        public List<Group> Else { get; } = new List<Group>();
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