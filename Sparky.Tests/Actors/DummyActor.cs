using Sparky.Actors;

namespace Sparky.Tests.Actors;

public class DummyActor : Actor
{
    public override void VisitChildren(Action<Actor> visitor)
    {
    }
}