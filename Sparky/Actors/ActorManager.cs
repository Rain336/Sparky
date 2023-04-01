using System.Diagnostics;
using SkiaSharp;

namespace Sparky.Actors;

internal class ActorManager : IActorManager
{
    public Actor? Root { get; private set; }

    internal void MountRoot(Actor actor)
    {
        Root = actor;
    }

    internal void Draw(SKCanvas canvas)
    {
        Debug.Assert(Root is not null);

        void DrawChild(Actor actor)
        {
            if (actor is IDrawable drawable)
            {
                drawable.Draw(canvas);
            }
            actor.VisitChildren(DrawChild);
        }
        Root.VisitChildren(DrawChild);
    }
}