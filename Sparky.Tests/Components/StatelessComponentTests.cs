using Sparky.Components;
using Sparky.Testing;

namespace Sparky.Tests.Components;

public class StatelessComponentTests : ComponentTestBase
{
    public class DummyStatelessComponent : StatelessComponent
    {
        public bool Rendered { get; private set; }

        public override Component Render(IRenderContext context)
        {
            Rendered = true;
            return new TerminatorComponent();
        }
    }

    [Fact]
    public void StatelessComponent_Rendered()
    {
        var component = new DummyStatelessComponent();

        RenderComponent(component);

        Assert.True(component.Rendered);
        Assert.NotEmpty(FindByComponent<TerminatorComponent>());
    }
}