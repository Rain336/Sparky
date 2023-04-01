using Android.Views;
using Java.Lang;
using SkiaSharp;
using Sparky.Application;

namespace Sparky.Android;

public abstract class AndroidPlatform : IPlatform
{
    public abstract void SetDrawCallback(Action<SKCanvas> callback);
    public abstract void SetResizeCallback(Action<int, int> callback);

    public abstract void Start();
    public abstract void ReplaceSurface(Surface surface);
    public abstract void DestroySurface();
    public abstract void ResizeSurface(Surface surface, int width, int height);
    public abstract void Pause();
    public abstract void Resume();
    public abstract void Destroy();
    public abstract void SetPostDrawCallback(IRunnable runnable);
}