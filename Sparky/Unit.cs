using System.Globalization;

namespace Sparky;

/// <summary>
/// A unit in the Sparky framework.
///
/// Sparky supports four units: Pixels, Percentage, Stretch and Default/Auto.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    public static Unit Pixels(float value)
    {
        return new Unit(UnitType.Pixels, value);
    }

    public static Unit Percentage(float value)
    {
        return new Unit(UnitType.Percentage, value);
    }

    public static Unit Stretch(float value)
    {
        return new Unit(UnitType.Stretch, value);
    }

    public static Unit Parse(string value)
    {
        const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint |
                                    NumberStyles.AllowExponent | NumberStyles.AllowThousands;

        if (value[^1] == 's' && float.TryParse(value[..^1], styles, NumberFormatInfo.InvariantInfo, out var stretch))
        {
            return Stretch(stretch);
        }

        if (value[^1] == '%' && float.TryParse(value[..^1], styles, NumberFormatInfo.InvariantInfo, out var percentage))
        {
            return Percentage(percentage);
        }

        if (float.TryParse(value, styles, NumberFormatInfo.InvariantInfo, out var pixels))
        {
            return Pixels(pixels);
        }

        return default;
    }

    public UnitType Type { get; }
    public float Value { get; }

    private Unit(UnitType type, float value)
    {
        Type = type;
        Value = value;
    }

    //internal float ToPixels(float parent, float auto)
    //{
    //    return Type switch
    //    {
    //        UnitType.Auto => auto,
    //        UnitType.Pixels => Value,
    //        UnitType.Percentage => (Value / 100.0F) * parent,
    //        UnitType.Stretch => 
    //    }
    //}

    public bool Equals(Unit other)
    {
        // Is this a good idea with the cut-off?
        return Type == other.Type && (Type == UnitType.Auto || Math.Abs(Value - other.Value) < 0.000001);
    }

    public override bool Equals(object? obj)
    {
        return obj is Unit other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Type == UnitType.Auto ? 0 : HashCode.Combine((int)Type, Value);
    }

    public override string ToString()
    {
        switch (Type)
        {
            case UnitType.Auto:
                return "auto";

            case UnitType.Pixels:
                return $"{Value}px";

            case UnitType.Percentage:
                return $"{Value}%";

            case UnitType.Stretch:
                return $"{Value}s";

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static bool operator ==(Unit l, Unit r)
        => l.Equals(r);

    public static bool operator !=(Unit l, Unit r)
        => !l.Equals(r);

    public static implicit operator Unit(int value)
        => new(UnitType.Pixels, value);

    public static implicit operator Unit(float value)
        => new(UnitType.Pixels, value);

    public static implicit operator Unit(string value)
        => Parse(value);
}