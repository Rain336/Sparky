using Sparky.Actors;
using Sparky.Components;
using Sparky.Testing;
using Sparky.Tests.Actors;

namespace Sparky.Tests.Components;

public class ActorWithChildrenComponentTests : ComponentTestBase
{
    public class DummyActorWithChildrenComponent : ActorWithChildrenComponent
    {
        public DummyActorWithChildrenComponent()
            : base(new[] { new TerminatorComponent(), new TerminatorComponent(), new TerminatorComponent() })
        {
        }

        public override Actor CreateActor()
            => new DummyActor();
    }

    [Fact]
    public void ActorComponent_ActorMounted()
    {
        var component = new DummyActorWithChildrenComponent();

        RenderComponent(component);

        Assert.Equal(3, FindByComponent<TerminatorComponent>().Count);
        Assert.NotEmpty(FindByActor<DummyActor>());
    }
}