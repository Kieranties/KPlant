using System.Linq;

namespace KPlant.Sequence.Model
{
    public partial class SequenceDiagram
    {
        public static SequenceDiagram Of(params ISequenceElement[] elements) => new SequenceDiagram { Elements = elements.ToList() };
    }
}