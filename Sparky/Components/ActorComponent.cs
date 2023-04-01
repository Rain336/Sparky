using Sparky.Actors;
using Sparky.Elements;

namespace Sparky.Components;

/// <summary>
/// An ActorComponent is a Component which also creates an Actor to draw, interact and/or layout children.
/// </summary>
public abstract class ActorComponent : Component
{
    /// <inheritdoc />
    public override Element CreateElement()
        => new ActorElement(this);

    /// <summary>
    /// Creates the Actor of this component.
    /// </summary>
    /// <returns>the Actor of this component</returns>
    public abstract Actor CreateActor();
}