namespace Sparky.Actors;

/// <summary>
/// Represents something that has an area and can be laid out.
/// </summary>
public interface ILayoutable
{
    /// <summary>
    /// Determines how this layoutable is position. Either by it's parent or by itself.
    /// </summary>
    PositionType PositionType { get; }
    
    /// <summary>
    /// Defines spacing around the layout's area.
    /// </summary>
    Box Spacing { get; }
    
    /// <summary>
    /// Defines the width of the layout's area.
    /// </summary>
    Unit Width { get; }
    
    /// <summary>
    /// Defines the height of the layout's area.
    /// </summary>
    Unit Height { get; }
}