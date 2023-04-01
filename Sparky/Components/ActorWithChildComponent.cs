using Sparky.Elements;

namespace Sparky.Components;

/// <summary>
/// An ActorComponent with a single child.
/// </summary>
public abstract class ActorWithChildComponent : ActorComponent
{
    /// <summary>
    /// The child of this ActorComponent. 
    /// </summary>
    public Component Child { get; }

    protected ActorWithChildComponent(Component child)
    {
        Child = child;
    }

    /// <inheritdoc />
    public override Element CreateElement()
        => new ActorWithChildElement(this);
}