using SkiaSharp;

namespace Sparky.Interaction;

/// <summary>
/// Represents a touch on the screen.
/// Can be from a finger, a stylus or even a mouse.
/// </summary>
public interface ITouch
{
    /// <summary>
    /// The current position of the touch.
    /// </summary>
    SKPoint Position { get; }

    /// <summary>
    /// A list of touches between the current and previous event.
    /// </summary>
    List<SKPoint> PreviousPositions { get; }

    /// <summary>
    /// The last time this touch got updated.
    /// </summary>
    DateTime Timestamp { get; }

    /// <summary>
    /// The current phase of the touch.
    /// </summary>
    TouchPhase Phase { get; }

    /// <summary>
    /// The current pressure emitted by this touch.
    /// </summary>
    float Pressure { get; }
}