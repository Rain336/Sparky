namespace Sparky.Testing;

[AttributeUsage(AttributeTargets.Assembly)]
public class TestingAppAttribute : Attribute
{
    public Type TestingApp { get; }

    public TestingAppAttribute(Type testingApp)
    {
        TestingApp = testingApp;
    }
}