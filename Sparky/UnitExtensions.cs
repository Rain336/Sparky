namespace Sparky;

/// <summary>
/// Helper class to construct units from ints and floats.
/// </summary>
public static class UnitExtensions
{
    public static Unit Percentage(this int value)
        => Unit.Percentage(value);
    
    public static Unit Percentage(this float value)
        => Unit.Percentage(value);
    
    public static Unit Stretch(this int value)
        => Unit.Stretch(value);
    
    public static Unit Stretch(this float value)
        => Unit.Stretch(value);
}