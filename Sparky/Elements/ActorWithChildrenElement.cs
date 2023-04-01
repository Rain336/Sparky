using Sparky.Components;

namespace Sparky.Elements;

public class ActorWithChildrenElement : ActorElement
{
    private Element[]? _children;

    public ActorWithChildrenElement(ActorWithChildrenComponent component)
        : base(component)
    {
    }

    public override void VisitChildren(Action<Element> visitor)
    {
        if (_children is not null)
        {
            foreach (var child in _children)
            {
                visitor(child);
            }
        }
    }

    protected override void RebuildInternal()
    {
        base.RebuildInternal();

        var components = ((ActorWithChildrenComponent)Component).Children;
        var newChildren = new Element[components.Length];
        for (int i = 0; i < components.Length; i++)
        {
            var element = components[i].CreateElement();
            UpdateChild(_children?[i], element);
            newChildren[i] = element;
        }

        _children = newChildren;
    }
}