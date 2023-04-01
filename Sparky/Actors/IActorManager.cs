namespace Sparky.Actors;

/// <summary>
/// The Actor manager is responsible for making draw calls to <see cref="IDrawable"/> Actors and
/// passing interaction events to the correct Actor.
/// </summary>
public interface IActorManager
{
    Actor? Root { get; }
}