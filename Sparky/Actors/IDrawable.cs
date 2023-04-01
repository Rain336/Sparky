using SkiaSharp;

namespace Sparky.Actors;

/// <summary>
/// Allows an <see cref="Actor"/> to draw to it's area.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Gets called every time the Actor should be redrawn.
    /// </summary>
    /// <param name="canvas">The canvas to draw to</param>
    void Draw(SKCanvas canvas);
}