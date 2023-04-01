using System.Diagnostics;
using Sparky.Components;

namespace Sparky.Elements;

/// <summary>
/// Elements are helper classes to Components.
/// They are required, because Components don't have any relationships or traversal abilities.
/// Each Component has a corresponding Element, which it creates.
/// </summary>
public abstract class Element : IRenderContext, IDisposable
{
    private ElementState _state = ElementState.Initial;

    /// <summary>
    /// The parent of this Element.
    /// Can be null if the Element isn't mounted or is the root Element.
    /// </summary>
    public Element? Parent { get; private set; }

    /// <summary>
    /// The Manager of this Element tree.
    /// Can be null if the Element isn't mounted.
    /// </summary>
    public ITreeManager? Manager { get; private set; }

    /// <summary>
    /// Whenever this Element will be rebuild next frame.
    /// </summary>
    public bool Dirty { get; private set; }

    /// <summary>
    /// The corresponding Component to this Element.
    /// </summary>
    public Component Component { get; }

    /// <summary>
    /// Returns if this element is mounted to the element tree.
    /// </summary>
    public bool IsMounted => _state == ElementState.Mounted;

    protected Element(Component component)
    {
        Component = component;
    }

    /// <summary>
    /// Attaches this Element to the given parent.
    /// This should only be called if the Element hasn't been mounted before.
    /// </summary>
    /// <param name="parent">The new parent of this Element</param>
    public virtual void Mount(Element parent)
    {
        Debug.Assert(_state == ElementState.Initial);
        _state = ElementState.Mounted;
        Parent = parent;
        Manager = parent.Manager;
        RebuildInternal();
    }

    internal void MountAsRoot(ITreeManager manager)
    {
        Debug.Assert(_state == ElementState.Initial);
        _state = ElementState.Mounted;
        Manager = manager;
        RebuildInternal();
    }

    /// <summary>
    /// Unmounts this Element and marks it for deletion. 
    /// </summary>
    public virtual void Unmount()
    {
        Debug.Assert(_state == ElementState.Mounted);
        _state = ElementState.Unmounted;
        Manager!.MarkForDelete(this);
    }

    /// <summary>
    /// A helper function to update a child of this Element.
    /// It first unmounts the old Element and then mounts the new one.
    /// You can set newChild to null to delete a child Element.
    /// You can set oldChild to null to attach a new child Element.
    /// </summary>
    /// <param name="oldChild">The old child to unmount</param>
    /// <param name="newChild">The new child to mount onto this one.</param>
    public void UpdateChild(Element? oldChild, Element? newChild)
    {
        Debug.Assert(oldChild is null || ReferenceEquals(oldChild.Parent, this));
        Debug.Assert(newChild is null || newChild._state == ElementState.Initial);
        oldChild?.Unmount();
        newChild?.Mount(this);
    }

    /// <summary>
    /// Calls the <see cref="visitor"/> for each child of this Element.
    /// </summary>
    /// <param name="visitor">Gets called for each child</param>
    public abstract void VisitChildren(Action<Element> visitor);

    /// <summary>
    /// Called by the <see cref="TreeManager"/> to rebuild this Element.
    /// </summary>
    public void Rebuild()
    {
        if (_state != ElementState.Mounted || !Dirty)
        {
            return;
        }

        RebuildInternal();
        Dirty = false;
    }

    /// <summary>
    /// Marks this Component as dirty to be rebuilt on the next frame. 
    /// </summary>
    public void MarkAsDirty()
    {
        Debug.Assert(_state == ElementState.Mounted);
        Debug.Assert(Manager?.IsBuilding == false);
        if (Dirty)
        {
            return;
        }

        Manager?.MarkForRebuild(this);
        Dirty = true;
    }

    /// <summary>
    /// Called by <see cref="Rebuild"/> to do the actual rebuild.
    /// </summary>
    protected abstract void RebuildInternal();
    
    /// <summary>
    /// Disposes this Element.
    /// </summary>
    public virtual void Dispose()
    {
        Debug.Assert(_state == ElementState.Unmounted);
        _state = ElementState.Disposed;
        Parent = null;
        Manager = null;
    }
}