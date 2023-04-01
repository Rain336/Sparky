using DryIoc;
using Sparky.Components;

namespace Sparky.Application;

/// <summary>
/// Helper clas making the DI register method optional.
/// </summary>
public abstract class SparkyApp : ISparkyApp
{
    /// <inheritdoc />
    public virtual void ConfigureContainer(IContainer container)
    {
    }

    /// <inheritdoc />
    public abstract Component CreateRoot();
}

/// <summary>
/// Helper class, constructing the root component <see cref="T"/> using DI.
/// </summary>
/// <typeparam name="T">The type of the root component</typeparam>
public abstract class SparkyApp<T> : SparkyApp
    where T : Component
{
    /// <inheritdoc />
    public override Component CreateRoot()
        => Locator.Resolve<T>();
}