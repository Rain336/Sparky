namespace Sparky.Interaction;

/// <summary>
/// The phase of a toucher describes what it did between the last and current event.
/// </summary>
public enum TouchPhase
{
    /// <summary>
    /// The toucher pressed down and started emitting events.
    /// </summary>
    Begin,
    
    /// <summary>
    /// The toucher moved while still pressing down.
    /// </summary>
    Moved,
    
    /// <summary>
    /// The toucher didn't moved while still pressing down.
    /// </summary>
    Stationary,
    
    /// <summary>
    /// The toucher stopped pressing down, completing the gesture.
    /// </summary>
    End,
    
    /// <summary>
    /// The toucher's gesture got interrupted and has to be aborted.
    /// </summary>
    Canceled
}