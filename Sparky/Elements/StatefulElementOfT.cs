using Sparky.Components;

namespace Sparky.Elements;

/// <summary>
/// Element class for <see cref="StatefulComponent{T}"/>.
/// </summary>
/// <typeparam name="T">The state object from the component</typeparam>
public class StatefulElement<T> : CompositeElement
{
    /// <summary>
    /// Constructs a new StatefulElement[T] from a StatefulComponent[T].
    /// </summary>
    /// <param name="component">The component of this element</param>
    public StatefulElement(StatefulComponent<T> component)
        : base(component)
    {
    }

    /// <inheritdoc />
    public override void Mount(Element parent)
    {
        base.Mount(parent);
        ((StatefulComponent<T>)Component).ElementMounted(this);
    }

    /// <inheritdoc />
    public override void Unmount()
    {
        base.Unmount();
        ((StatefulComponent<T>)Component).ElementUnmounted();
    }

    /// <inheritdoc />
    protected override void RebuildInternal()
    {
        var component = ((StatefulComponent<T>)Component).Render(this);
        var element = component.CreateElement();
        UpdateChild(_child, element);
        _child = element;
    }
}