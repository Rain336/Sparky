namespace Sparky.Tests;

public class UnitTests
{
    [Fact]
    public void UnitFromString_Number()
    {
        var expected = Unit.Pixels(12);

        var parsed = Unit.Parse("12");

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void UnitFromString_Percentage()
    {
        var expected = Unit.Percentage(15.56F);

        var parsed = Unit.Parse("15.56%");

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void UnitFromString_Stretch()
    {
        var expected = Unit.Stretch(200);

        var parsed = Unit.Parse("200s");

        Assert.Equal(expected, parsed);
    }
}