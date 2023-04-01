using SkiaSharp;
using Vulkan;

namespace Sparky.Android.Vulkan;

public class VulkanRenderer : IDisposable
{
    private GRBackendRenderTarget[]? _renderTargets;
    private SKSurface[]? _surfaces;

    public bool CanDraw => _surfaces is not null;

    public SKCanvas Draw(uint idx, Action<SKCanvas> draw)
    {
        var surface = _surfaces![idx];
        var canvas = surface.Canvas;

        using (new SKAutoCanvasRestore(canvas, true))
        {
            draw(canvas);
        }

        return canvas;
    }

    public void UpdateRenderTargets(GRContext context, int width, int height, Image[] images, GRVkImageInfo info)
    {
        if (_surfaces is not null)
        {
            foreach (var surface in _surfaces)
            {
                surface.Dispose();
            }
        }

        if (_renderTargets is not null)
        {
            foreach (var target in _renderTargets)
            {
                target.Dispose();
            }
        }

        _renderTargets = new GRBackendRenderTarget[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            info.Image = ((INonDispatchableHandleMarshalling)images[i]).Handle;
            _renderTargets[i] = new GRBackendRenderTarget(width, height, (int)info.SampleCount, info);
        }

        _surfaces = new SKSurface[images.Length];
        for (int i = 0; i < _renderTargets.Length; i++)
        {
            _surfaces[i] = SKSurface.Create(
                context,
                _renderTargets[i],
                GRSurfaceOrigin.TopLeft,
                ((Format)info.Format).ToColorType(),
                SKColorSpace.CreateSrgb()
            );
        }
    }

    public void Dispose()
    {
        if (_surfaces is not null)
        {
            foreach (var surface in _surfaces)
            {
                surface.Dispose();
            }
        }

        if (_renderTargets is not null)
        {
            foreach (var target in _renderTargets)
            {
                target.Dispose();
            }
        }
    }
}