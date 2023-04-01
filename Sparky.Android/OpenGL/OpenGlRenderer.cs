using Android.Opengl;
using SkiaSharp;

namespace Sparky.Android.OpenGL;

public class OpenGlRenderer : IDisposable
{
    private const SKColorType ColorType = SKColorType.Rgba8888;

    private readonly GRContext _context;
    private GRBackendRenderTarget? _renderTarget;
    private SKSurface? _surface;

    public OpenGlRenderer()
    {
        _context = GRContext.CreateGl(GRGlInterface.Create());
    }

    public void Draw(Action<SKCanvas> draw)
    {
        GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit | GLES20.GlStencilBufferBit);

        if (_renderTarget is null)
        {
            return;
        }

        var canvas = _surface!.Canvas;
        using (new SKAutoCanvasRestore(canvas, true))
        {
            draw(canvas);
        }

        canvas.Flush();
        _context.Flush();
    }

    public void Resize(int width, int height)
    {
        GLES20.GlViewport(0, 0, width, height);

        var buffer = new int[3];
        GLES20.GlGetIntegerv(GLES20.GlFramebufferBinding, buffer, 0);
        GLES20.GlGetIntegerv(GLES20.GlStencilBits, buffer, 1);
        GLES20.GlGetIntegerv(GLES20.GlSamples, buffer, 2);

        var maxSamples = _context.GetMaxSurfaceSampleCount(ColorType);
        var samples = buffer[2] > maxSamples ? maxSamples : buffer[2];

        var info = new GRGlFramebufferInfo((uint)buffer[0], ColorType.ToGlSizedFormat());

        _surface?.Dispose();
        _renderTarget?.Dispose();

        _renderTarget = new GRBackendRenderTarget(width, height, samples, buffer[1], info);
        _surface = SKSurface.Create(_context, _renderTarget, GRSurfaceOrigin.BottomLeft, ColorType);
    }

    public void Dispose()
    {
        _surface?.Dispose();
        _renderTarget?.Dispose();
        _context.Dispose();
    }
}