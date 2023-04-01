using Android.Graphics;
using Android.OS;
using Android.Views;
using Java.Lang;
using Sparky.Android.OpenGL;
using Sparky.Android.Vulkan;

namespace Sparky.Android;

public class SparkyActivity : Activity, ISurfaceHolderCallback2
{
    private readonly AndroidPlatform _platform;

    public SparkyActivity()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
        {
            _platform = new VulkanThread();
        }
        else
        {
            _platform = new OpenGlThread();
        }
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        _platform.Start();
        Window.TakeSurface(this);
    }

    public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
    {
        _platform.ResizeSurface(holder.Surface, width, height);
    }

    public void SurfaceCreated(ISurfaceHolder holder)
    {
        _platform.ReplaceSurface(holder.Surface);
    }

    public void SurfaceDestroyed(ISurfaceHolder holder)
    {
        _platform.DestroySurface();
    }

    public void SurfaceRedrawNeededAsync(ISurfaceHolder holder, IRunnable drawingFinished)
    {
        _platform.SetPostDrawCallback(drawingFinished);
    }

    public void SurfaceRedrawNeeded(ISurfaceHolder holder)
    {
        // Should not be called, since we overwrite SurfaceRedrawNeededAsync.
    }

    public override bool OnTouchEvent(MotionEvent? e)
    {
        if (e is null)
        {
            return false;
        }

        switch (e.Action)
        {
            case MotionEventActions.Down:
                break;
        }

        return false;
    }

    protected override void OnPause()
    {
        base.OnPause();
        _platform.Pause();
    }

    protected override void OnResume()
    {
        base.OnResume();
        _platform.Resume();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _platform.Destroy();
    }
}