using System.Diagnostics;
using Sparky.Elements;

namespace Sparky.Components;

/// <summary>
/// A stateful Component constructs it's child node using the <see cref="Render"/> method and
/// can track a state <see cref="T"/>. 
/// </summary>
/// <typeparam name="T">The type of state to keep track of</typeparam>
public abstract class StatefulComponent<T> : Component
{
    private StatefulElement<T>? _element;

    /// <summary>
    /// Gets the current state of this Component.
    /// </summary>
    protected T State { get; private set; }

    protected StatefulComponent(T state)
    {
        State = state;
    }

    /// <inheritdoc />
    public override Element CreateElement()
        => new StatefulElement<T>(this);

    /// <summary>
    /// Called to build the child Component of this Component.
    /// It takes a <see cref="setState"/> delegate to indicate a state change.
    /// </summary>
    /// <param name="context">A context to access</param>
    /// <returns>The child Component of this Component</returns>
    public abstract Component Render(IRenderContext context);

    /// <summary>
    /// Updates the state object to <see cref="value"/>.
    /// </summary>
    /// <param name="value">The new value of the state object</param>
    protected void SetState(T value)
    {
        Debug.Assert(_element is not null);
        State = value;
        _element?.MarkAsDirty();
    }

    /// <summary>
    /// Updates the state object using <see cref="func"/>.
    /// </summary>
    /// <param name="func">Returns the new value of the state object</param>
    protected void SetState(Func<T> func)
    {
        Debug.Assert(_element is not null);
        State = func();
        _element?.MarkAsDirty();
    }

    /// <summary>
    /// Updates the state object using <see cref="func"/> with the old state as parameter.
    /// </summary>
    /// <param name="func">Returns the new value of the state object</param>
    protected void SetState(Func<T, T> func)
    {
        Debug.Assert(_element is not null);
        State = func(State);
        _element?.MarkAsDirty();
    }

    internal void ElementMounted(StatefulElement<T> element)
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