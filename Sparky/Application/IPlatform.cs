using SkiaSharp;

namespace Sparky.Application;

/// <summary>
/// Implements interactions with the native platform.
/// </summary>
public interface IPlatform
{
    /// <summary>
    /// Sets the callback to be called when a new frame should be drawn.
    /// </summary>
    /// <param name="callback">called when a new frame should be drawn</param>
    void SetDrawCallback(Action<SKCanvas> callback);

    /// <summary>
    /// Sets the callback to be called when the native window gets resized.
    /// </summary>
    /// <param name="callback">called when the native window gets resized</param>
    void SetResizeCallback(Action<int, int> callback);
}