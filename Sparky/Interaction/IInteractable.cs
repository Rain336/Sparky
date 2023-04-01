using Sparky.Actors;

namespace Sparky.Interaction;

/// <summary>
/// Makes an <see cref="Actor"/> receive input events.
/// </summary>
public interface IInteractable
{
    void TouchDown(ITouch[] touches);
    void TouchMoved(ITouch[] touches);
    void TouchUp(ITouch[] touches);
    void TouchCanceled(ITouch[] touches);
}