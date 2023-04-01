using Sparky.Actors;
using Sparky.Components;
using Sparky.Testing;
using Sparky.Tests.Actors;

namespace Sparky.Tests.Components;

public class ActorWithChildComponentTests : ComponentTestBase
{
    public class DummyActorWithChildComponent : ActorWithChildComponent
    {
        public DummyActorWithChildComponent()
            : base(new TerminatorComponent())
        {
        }

        public override Actor CreateActor()
            => new DummyActor();
    }

    [Fact]
    public void ActorComponent_ActorMounted()
    {
        var component = new DummyActorWithChildComponent();

        RenderComponent(component);

        Assert.NotEmpty(FindByComponent<TerminatorComponent>());
        Assert.NotEmpty(FindByActor<DummyActor>());
    }
}