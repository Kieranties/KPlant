using System.Collections.Generic;

namespace KPlant.Model
{
    public interface IElementCollection<TElement> //: IEnumerable<TElement>
    {
        List<TElement> Elements { get; set; }

        void Add(TElement element);
    }
}
