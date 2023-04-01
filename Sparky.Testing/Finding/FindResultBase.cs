using System.Collections;
using Sparky.Elements;

namespace Sparky.Testing.Finding;

public abstract class FindResultBase : IReadOnlyList<Element>
{
    protected readonly List<Element> Elements;

    protected FindResultBase(IEnumerable<Element> elements)
    {
        Elements = new List<Element>(elements);
    }

    public IEnumerator<Element> GetEnumerator()
    {
        return Elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Elements.GetEnumerator();
    }

    public int Count => Elements.Count;

    public Element this[int index] => Elements[index];
}