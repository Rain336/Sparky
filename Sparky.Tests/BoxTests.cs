namespace Sparky.Tests;

public class BoxTests
{
    [Fact]
    public void BoxFromString_WithoutUnits_AllValues()
    {
        var expected = new Box(15, 12.5F, 64, 1234.5678F);

        var parsed = Box.Parse("15 12.5 64 1234.5678");

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void BoxFromString_WithoutUnits_TwoValues()
    {
        var expected = new Box(15, 1234.5678F);

        var parsed = Box.Parse("15 1234.5678");

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void BoxFromString_WithoutUnits_OneValues()
    {
        var expected = new Box(15);

        var parsed = Box.Parse("15");

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void BoxFromString_WithUnits_AllValues()
    {
        var expected = new Box(15, 12.5F.Percentage(), 64.Stretch(), 1234.5678F);

        var parsed = Box.Parse("15 12.5% 64s 1234.5678");

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void BoxFromString_WithUnits_TwoValues()
    {
        var expected = new Box(15.Stretch(), 1234.5678F);

        var parsed = Box.Parse("15s 1234.5678");

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void BoxFromString_WithUnits_OneValues()
    {
        var expected = new Box(1234.5678F.Percentage());

        var parsed = Box.Parse("1234.5678%");

        Assert.Equal(expected, parsed);
    }
}