using System.Diagnostics;
using Sparky.Elements;
using Sparky.Interaction;

namespace Sparky.Actors;

/// <summary>
/// An Actor is a region on screen that can be drawn to, interacted with and/or layout child actors.
/// A basic Actor is only <see cref="ILayoutable"/> to specify it's size and position.
/// See <see cref="IDrawable"/> for drawing inside the Actor's region.
/// See <see cref="IInteractable"/> for processing Interactions inside the Actor's region.
/// </summary>
public abstract class Actor : ILayoutable, IDisposable
{
    private ElementState _state = ElementState.Initial;

    /// <summary>
    /// The parent of this <see cref="Actor"/> or null if the Actor is unmounted or the root Actor.
    /// </summary>
    public Actor? Parent { get; private set; }

    /// <summary>
    /// The <see cref="IActorManager"/> this Actor is managed by or null if the Actor is unmounted.
    /// </summary>
    public IActorManager? Manager { get; private set; }

    /// <inheritdoc />
    public PositionType PositionType { get; protected set; }

    /// <inheritdoc />
    public Box Spacing { get; protected set; }

    /// <inheritdoc />
    public Unit Width { get; protected set; }

    /// <inheritdoc />
    public Unit Height { get; protected set; }

    /// <summary>
    /// Mounts this Actor onto the given parent.
    /// Normally called by <see cref="ActorElement"/> when it's rebuilt.
    /// </summary>
    /// <param name="parent">The parent Actor to mount to</param>
    public virtual void Mount(Actor parent)
    {
        Debug.Assert(_state == ElementState.Initial);
        _state = ElementState.Mounted;
        Parent = parent;
        Manager = parent.Manager;
    }

    internal void MountAsRoot(IActorManager manager)
    {
        Debug.Assert(_state == ElementState.Initial);
        _state = ElementState.Mounted;
        Manager = manager;
    }

    /// <summary>
    /// Unmounts this Actor from it's parent.
    /// Normally called by <see cref="ActorElement"/> on their unmount.
    /// </summary>
    public virtual void Unmount()
    {
        Debug.Assert(_state == ElementState.Mounted);
        _state = ElementState.Unmounted;
    }

    /// <summary>
    /// Calls the given visitor callback for each child of this Actor.
    /// </summary>
    /// <param name="visitor">The callback to call for each child</param>
    public abstract void VisitChildren(Action<Actor> visitor);

    /// <summary>
    /// Disposes this Actor.
    /// Normally called by <see cref="ActorElement"/> on their dispose.
    /// </summary>
    public virtual void Dispose()
    {
        Debug.Assert(_state == ElementState.Unmounted);
        _state = ElementState.Disposed;
        Parent = null;
        Manager = null;
    }
}