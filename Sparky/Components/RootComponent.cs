using Sparky.Actors;
using Sparky.Elements;

namespace Sparky.Components;

internal class RootComponent : ActorComponent
{
    private RootElement? _element;
    
    public Component Child { get; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }

    public RootComponent(Component child, int width, int height)
    {
        Child = child;
        Width = width;
        Height = height;
    }

    public override Element CreateElement()
        => new RootElement(this);

    public override Actor CreateActor()
        => new RootActor(_element!, Width, Height);

    public void SetElement(RootElement element)
    {
        _element = element;
    }
}