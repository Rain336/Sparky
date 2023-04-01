using Sparky.Elements;

namespace Sparky.Components;

/// <summary>
/// An ActorComponent with multiple child components.
/// </summary>
public abstract class ActorWithChildrenComponent : ActorComponent
{
    /// <summary>
    /// The array of children of this ActorComponent.
    /// </summary>
    public Component[] Children { get; }

    protected ActorWithChildrenComponent(Component[] children)
    {
        Children = children;
    }

    /// <inheritdoc />
    public override Element CreateElement()
        => new ActorWithChildrenElement(this);
}