using Sparky.Elements;

namespace Sparky.Components;

/// <summary>
/// Components are the building blocks of your UI.
/// This base class only contains a method to create the Component's corresponding <see cref="Element"/> class.
/// </summary>
public abstract class Component
{
    /// <summary>
    /// Creates the Element corresponding to this Component.
    /// </summary>
    /// <returns>The Element corresponding to this Component</returns>
    public abstract Element CreateElement();
}