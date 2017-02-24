using System.Collections.Generic;

namespace KPlant.Sequence.Model
{
    public class SequenceDiagram
    {
        public List<ISequenceElement> Elements { get; } = new List<ISequenceElement>();          
    }
}