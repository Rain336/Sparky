namespace Sparky;

public readonly record struct Box(Unit Left, Unit Top, Unit Right, Unit Bottom)
{
    public static Box Parse(string value)
    {
        var spits = value.Split(' ');
        return spits.Length switch
        {
            4 => new(spits[0], spits[1], spits[2], spits[3]),
            2 => new(spits[0], spits[1]),
            1 => new(spits[0]),
            _ => default
        };
    }

    public Box(Unit x, Unit y)
        : this(x, y, x, y)
    {
    }

    public Box(Unit value)
        : this(value, value, value, value)
    {
    }

    public static implicit operator Box(string value)
        => Parse(value);

    public override string ToString()
    {
        if (Left == Right && Top == Bottom)
        {
            if (Left == Top)
            {
                return Left.ToString();
            }

            return $"{Left} {Top}";
        }

        return $"{Left} {Top} {Right} {Bottom}";
    }
}