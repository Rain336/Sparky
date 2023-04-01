using DryIoc;
using Sparky.Components;

namespace Sparky.Application;

/// <summary>
/// The app class is a used to setup the framework.
/// It allows registering services into the DI container.
/// As well as specify the root component to be drawn.
/// </summary>
public interface ISparkyApp
{
    /// <summary>
    /// Allows to register new services into the DI container.
    /// </summary>
    /// <param name="container">The DI container to register into</param>
    void ConfigureContainer(IContainer container);
    
    /// <summary>
    /// Creates the root component of the app.
    /// </summary>
    /// <returns>the root component</returns>
    Component CreateRoot();
}