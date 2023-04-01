using Sparky.Components;
using Sparky.Testing;

namespace Sparky.Tests.Components;

public class StatefulComponentTests : ComponentTestBase
{
    public class DummyStatefulComponent : StatefulComponent
    {
        public int RenderCount { get; private set; }
        public int Count { get; private set; }
        public bool Rendered { get; private set; }
        
        public override Component Render(IRenderContext context)
        {
            Assert.False(Count > 2);
            Rendered = true;
            RenderCount++;
            return new TerminatorComponent();
        }

        public void Update()
        {
            switch (Count)
            {
                case 0:
                case 1:
                    SetState(() => Count++);
                    break;
                
                case 2:
                    Count++;
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

        Assert.Equal(component.RenderCount, component.Count + 1);
        Assert.NotEmpty(FindByComponent<TerminatorComponent>());
        
        component.Update();
        Render();
        
        Assert.Equal(component.RenderCount, component.Count + 1);
        
        component.Update();
        Render();
        
        Assert.Equal(component.RenderCount, component.Count + 1);
        
        component.Update();
        Render();
        
        Assert.Equal(component.RenderCount, component.Count);
        
        Render();
    }
}