using Sparky.Elements;

namespace Sparky.Actors;

internal class RootActor : Actor
{
    private readonly RootElement _element;

    public RootActor(RootElement element, int width, int height)
    {
        _element = element;
        Width = width;
        Height = height;
    }

    public override void VisitChildren(Action<Actor> visitor)
    {
        void FindActor(Element e)
        {
            if (e is ActorElement ae)
            {
                visitor(ae.Actor!);
                return;
            }
            e.VisitChildren(FindActor);
        }
        _element.VisitChildren(FindActor);
    }
}