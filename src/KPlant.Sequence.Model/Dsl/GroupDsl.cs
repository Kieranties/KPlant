using System.Collections.Generic;

namespace KPlant.Sequence.Model
{
    public partial class Group
    {
        private static Group Create( GroupType type, ISequenceElement[] elements)
        {
            return new Group(type)
            {
                Elements = new List<ISequenceElement>(elements)
            };
        }

        public static Group Alt(params ISequenceElement[] elements) => Create(GroupType.Alt, elements);
        public static Group Opt(params ISequenceElement[] elements) => Create(GroupType.Opt, elements);
        public static Group Loop(params ISequenceElement[] elements) => Create(GroupType.Loop, elements);
        public static Group Par(params ISequenceElement[] elements) => Create(GroupType.Par, elements);
        public static Group Break(params ISequenceElement[] elements) => Create(GroupType.Break, elements);
        public static Group Critical(params ISequenceElement[] elements) => Create(GroupType.Critical, elements);
        public static Group Of(params ISequenceElement[] elements) => Create(GroupType.Group, elements);       
       
    }
}