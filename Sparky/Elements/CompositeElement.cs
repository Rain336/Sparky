using Sparky.Components;

namespace Sparky.Elements;

/// <summary>
/// An Element that is made up of other Elements.
/// </summary>
public abstract class CompositeElement : Element
{
    protected Element? _child;
    
    protected CompositeElement(Component component)
        : base(component)
    {
    }

    /// <inheritdoc/>
    public override void VisitChildren(Action<Element> visitor)
    {
        if (_child is not null)
        {
            visitor(_child);
        }
    }
}