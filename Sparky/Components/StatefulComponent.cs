using System.Diagnostics;
using Sparky.Elements;

namespace Sparky.Components;

/// <summary>
/// A stateful Component constructs it's child node using the <see cref="Render"/> method and
/// can update it's fields using it's <see cref="SetState"/> method.
/// </summary>
public abstract class StatefulComponent : Component
{
    private StatefulElement? _element;

    /// <inheritdoc />
    public override Element CreateElement()
        => new StatefulElement(this);

    /// <summary>
    /// Called to build the child Component of this Component.
    /// </summary>
    /// <param name="context">A context to access</param>
    /// <returns>The child Component of this Component</returns>
    public abstract Component Render(IRenderContext context);

    /// <summary>
    /// Notifies to the framework that fields have been changed.
    /// </summary>
    protected void SetState()
    {
        Debug.Assert(_element is not null);
        _element?.MarkAsDirty();
    }

    /// <summary>
    /// Notifies to the framework that fields have been changed.
    /// </summary>
    /// <param name="action">A callback to update fields in</param>
    protected void SetState(Action action)
    {
        Debug.Assert(_element is not null);
        action();
        _element?.MarkAsDirty();
    }
    
    internal void ElementMounted(StatefulElement element)
    {
        Debug.Assert(_element is null);
        _element = element;
    }

    internal void ElementUnmounted()
    {
        Debug.Assert(_element is not null);
        _element = null;
    }
}