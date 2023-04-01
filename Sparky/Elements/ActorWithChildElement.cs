using Sparky.Components;

namespace Sparky.Elements;

public class ActorWithChildElement : ActorElement
{
    private Element? _child;
    
    /// <summary>
    /// Constructs a new ActorWithChildElement from a ActorWithChildComponent.
    /// </summary>
    /// <param name="component">The component of this element</param>
    public ActorWithChildElement(ActorWithChildComponent component)
        : base(component)
    {
    }

    /// <inheritdoc />
    public override void VisitChildren(Action<Element> visitor)
    {
        if (_child is not null)
        {
            visitor(_child);
        }
    }

    /// <inheritdoc />
    protected override void RebuildInternal()
    {
        base.RebuildInternal();
        
        var element = ((ActorWithChildComponent)Component).Child.CreateElement();
        UpdateChild(_child, element);
        _child = element;
    }
}