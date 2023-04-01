using Sparky.Components;

namespace Sparky.Elements;

/// <summary>
/// Element class for <see cref="StatefulComponent"/>s.
/// </summary>
public class StatefulElement : CompositeElement
{
    /// <summary>
    /// Constructs a new StatefulElement from a StatefulComponent.
    /// </summary>
    /// <param name="component">The component of this element</param>
    public StatefulElement(StatefulComponent component)
        : base(component)
    {
    }

    /// <inheritdoc />
    public override void Mount(Element parent)
    {
        base.Mount(parent);
        ((StatefulComponent)Component).ElementMounted(this);
    }

    /// <inheritdoc />
    public override void Unmount()
    {
        base.Unmount();
        ((StatefulComponent)Component).ElementUnmounted();
    }

    /// <inheritdoc />
    protected override void RebuildInternal()
    {
        var component = ((StatefulComponent)Component).Render(this);
        var element = component.CreateElement();
        UpdateChild(_child, element);
        _child = element;
    }
}