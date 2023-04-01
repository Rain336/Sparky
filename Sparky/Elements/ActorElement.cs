using Sparky.Actors;
using Sparky.Components;

namespace Sparky.Elements;

/// <summary>
/// Element class for <see cref="ActorComponent"/>s.
/// </summary>
public class ActorElement : Element
{
    /// <summary>
    /// The <see cref="Actor"/> that is associated to this element.
    /// </summary>
    public Actor? Actor { get; protected set; }
    
    /// <summary>
    /// Constructs a new ActorElement from a ActorComponent.
    /// </summary>
    /// <param name="component">The component of this element</param>
    public ActorElement(ActorComponent component)
        : base(component)
    {
    }
    
    /// <inheritdoc />
    public override void Unmount()
    {
        Actor?.Unmount();
        base.Unmount();
    }

    /// <inheritdoc />
    public override void VisitChildren(Action<Element> visitor)
    {
    }

    /// <inheritdoc />
    protected override void RebuildInternal()
    {
        // TODO: Move into mount and let the actor rebuild
        Actor = ((ActorComponent)Component).CreateActor();
        var parent = FindParentActor();
        Actor.Mount(parent);
    }

    private Actor FindParentActor()
    {
        var current = Parent;
        while (current is not null)
        {
            if (current is ActorElement e)
            {
                return e.Actor!;
            }
            
            current = current.Parent;
        }

        throw new InvalidOperationException("Cannot find parent Actor!");
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        Actor?.Dispose();
        base.Dispose();
    }
}