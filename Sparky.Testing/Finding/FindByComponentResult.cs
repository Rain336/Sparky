using Sparky.Components;
using Sparky.Elements;

namespace Sparky.Testing.Finding;

public class FindByComponentResult<T> : FindResultBase
    where T : Component
{
    public IEnumerable<T> Components => Elements.Select(x => x.Component).Cast<T>();

    public FindByComponentResult(IEnumerable<Element> elements)
        : base(elements)
    {
    }
}