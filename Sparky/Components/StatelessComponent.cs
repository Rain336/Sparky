using Sparky.Elements;

namespace Sparky.Components;

/// <summary>
/// A stateless Component constructs it's child node using the <see cref="Render"/> method and
/// doesn't track any internal state.
/// </summary>
public abstract class StatelessComponent : Component
{
    /// <inheritdoc />
    public override Element CreateElement()
        => new StatelessElement(this);

    /// <summary>
    /// Called to build the child Component of this Component.
    /// </summary>
    /// <returns>The child Component of this Component</returns>
    public abstract Component Render(IRenderContext context);
}