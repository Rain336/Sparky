using Sparky.Components;
using Sparky.Elements;
using Sparky.Tests.Elements;

namespace Sparky.Tests.Components;

public class TerminatorComponent : Component
{
    public override Element CreateElement()
        => new TerminatorElement(this);
}