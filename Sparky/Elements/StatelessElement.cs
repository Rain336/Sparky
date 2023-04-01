using Sparky.Components;

namespace Sparky.Elements;

/// <summary>
/// The Element for <see cref="StatelessComponent"/>.
/// </summary>
public class StatelessElement : CompositeElement
{
    /// <summary>
    /// Constructs a StatelessElement from a <see cref="StatelessComponent"/>.
    /// </summary>
    /// <param name="component">The component of this element</param>
    public StatelessElement(StatelessComponent component)
        : base(component)
    {
    }

    /// <inheritdoc/>
    protected override void RebuildInternal()
    {
        var child = ((StatelessComponent)Component).Render(this);
        var element = child.CreateElement();
        UpdateChild(_child, element);
        _child = element;
    }
}