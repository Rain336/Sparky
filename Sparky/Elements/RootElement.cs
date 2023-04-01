using Sparky.Actors;
using Sparky.Components;

namespace Sparky.Elements;

internal class RootElement : ActorElement
{
    public Element? Child { get; private set; }

    public RootElement(RootComponent component)
        : base(component)
    {
    }

    public override void VisitChildren(Action<Element> visitor)
    {
        if (Child is not null)
        {
            visitor(Child);
        }
    }

    public void Resize(int width, int height)
    {
        var component = (RootComponent)Component;
        component.Width = width;
        component.Height = height;
        MarkAsDirty();
    }

    protected override void RebuildInternal()
    {
        Actor = ((ActorComponent)Component).CreateActor();
        Actor.MountAsRoot(Locator.Resolve<IActorManager>());

        var component = ((RootComponent)Component).Child;
        var element = component.CreateElement();
        UpdateChild(Child, element);
        Child = element;
    }
}