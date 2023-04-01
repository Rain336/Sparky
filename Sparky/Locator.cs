using DryIoc;

namespace Sparky;

public static class Locator
{
    public static IContainer Container { get; private set; }

    internal static void Initialize(IContainer container)
    {
        if (Container is not null)
        {
            return;
        }

        Container = container;
    }

    public static T Resolve<T>()
    {
        return Container.Resolve<T>();
    }
}