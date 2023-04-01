using Sparky.Components;
using Sparky.Testing;

namespace Sparky.Tests.Components;

public class StatefulComponentOfTTests : ComponentTestBase
{
    public class DummyState
    {
        public int Count { get; set; }
    }

    public class DummyStatefulComponent : StatefulComponent<DummyState>
    {
        public int RenderCount { get; private set; }
        public bool Rendered { get; private set; }

        public DummyState S => State;

        public DummyStatefulComponent()
            : base(new DummyState())
        {
        }

        public override Component Render(IRenderContext context)
        {
            Assert.False(State.Count > 2);
            Rendered = true;
            RenderCount++;
            return new TerminatorComponent();
        }

        public void Update()
        {
            switch (State.Count)
            {
                case 0:
                case 1:
                    SetState(old => new DummyState
                    {
                        Count = old.Count + 1
                    });
                    break;

                case 2:
                    State.Count++;
                    break;

                default:
                    Assert.True(false);
                    break;
            }
        }
    }

    [Fact]
    public void StatefulComponent_Rendered()
    {
        var component = new DummyStatefulComponent();

        RenderComponent(component);

        Assert.True(component.Rendered);
        Assert.NotEmpty(FindByComponent<TerminatorComponent>());
    }

    [Fact]
    public void StatefulComponent_Updates()
    {
        var component = new DummyStatefulComponent();

        RenderComponent(component);

        Assert.Equal(component.RenderCount, component.S.Count + 1);
        Assert.NotEmpty(FindByComponent<TerminatorComponent>());

        component.Update();
        Render();

        Assert.Equal(component.RenderCount, component.S.Count + 1);

        component.Update();
        Render();

        Assert.Equal(component.RenderCount, component.S.Count + 1);

        component.Update();
        Render();

        Assert.Equal(component.RenderCount, component.S.Count);

        Render();
    }
}