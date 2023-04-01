using Sparky.Elements;
using Sparky.Tests.Components;

namespace Sparky.Tests.Elements;

public class TerminatorElement : Element
{
    public TerminatorElement(TerminatorComponent component)
        : base(component)
    {
    }

    public override void VisitChildren(Action<Element> visitor)
    {
    }

    protected override void RebuildInternal()
    {
    }
}