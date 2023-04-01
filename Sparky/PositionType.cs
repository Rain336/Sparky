namespace Sparky;

/// <summary>
/// Describes how an Actor is position.
/// </summary>
public enum PositionType
{
    /// <summary>
    /// The position of this Actor is controlled by it's parent.
    /// </summary>
    ParentDirected,
    
    /// <summary>
    /// The position of this Actor is controlled by itself.
    /// </summary>
    SelfDirected
}