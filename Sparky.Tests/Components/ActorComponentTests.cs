using Sparky.Actors;
using Sparky.Components;
using Sparky.Testing;
using Sparky.Tests.Actors;

namespace Sparky.Tests.Components;

public class ActorComponentTests : ComponentTestBase
{
    public class DummyActorComponent : ActorComponent
    {
        public override Actor CreateActor()
            => new DummyActor();
    }

    [Fact]
    public void ActorComponent_ActorMounted()
    {
        var component = new DummyActorComponent();

        RenderComponent(component);
        
        Assert.NotEmpty(FindByActor<DummyActor>());
    }
}