using System.Collections.Concurrent;
using Android.Views;
using SkiaSharp;

namespace Sparky.Android.OpenGL;

public class OpenGlThread : AndroidPlatform
{
    private readonly Thread _thread;
    private readonly ConcurrentQueue<Action<OpenGlHelper, OpenGlRenderer?>> _tasks = new();
    private readonly CancellationTokenSource _cts = new();
    private bool _paused;
    private int _width;
    private int _height;
    private Java.Lang.IRunnable? _postDrawCallback;
    private Action<SKCanvas>? _callback;
    private Action<int, int>? _resize;

    public int Id => _thread.ManagedThreadId;
    private bool CanDraw => !_paused && _callback is not null && _width > 0 && _height > 0;

    public OpenGlThread()
    {
        _thread = new Thread(Run);
    }

    public override void SetDrawCallback(Action<SKCanvas> callback)
    {
        _tasks.Enqueue((_, _) => _callback = callback);
    }

    public override void SetResizeCallback(Action<int, int> callback)
    {
        _tasks.Enqueue((_, _) =>
        {
            _resize = callback;
            callback(_width, _height);
        });
    }

    public override void Start()
        => _thread.Start();

    public override void ReplaceSurface(Surface surface)
    {
        _tasks.Enqueue((helper, _) =>
        {
            helper.DestroySurface();
            helper.CreateSurface(surface);
        });
    }

    public override void DestroySurface()
    {
        _tasks.Enqueue((helper, _) => helper.DestroySurface());
    }

    public override void ResizeSurface(Surface surface, int width, int height)
    {
        _tasks.Enqueue((helper, renderer) =>
        {
            _width = width;
            _height = height;
            helper.DestroySurface();
            helper.CreateSurface(surface);
            renderer?.Resize(width, height);
            _resize?.Invoke(width, height);
        });
    }

    public override void Pause()
    {
        _tasks.Enqueue((helper, _) =>
        {
            _paused = true;
            helper.DestroySurface();
            helper.DestroyContext();
        });
    }

    public override void Resume()
    {
        _tasks.Enqueue((helper, _) =>
        {
            _paused = false;
            helper.CreateContext();
        });
    }

    public override void Destroy()
    {
        _cts.Cancel();
    }

    public override void SetPostDrawCallback(Java.Lang.IRunnable runnable)
    {
        _tasks.Enqueue((_, _) => _postDrawCallback = runnable);
    }

    private void Run()
    {
        var helper = new OpenGlHelper();
        helper.CreateContext();

        OpenGlRenderer? renderer = null;

        while (!_cts.IsCancellationRequested)
        {
            while (_tasks.TryDequeue(out var task))
            {
                task(helper, renderer);
            }

            if (CanDraw && helper.CanDraw)
            {
                if (renderer is null)
                {
                    renderer = new();
                    renderer.Resize(_width, _height);
                }

                renderer.Draw(_callback!);

                helper.Swap();

                if (_postDrawCallback is not null)
                {
                    _postDrawCallback.Run();
                    _postDrawCallback = null;
                }
            }
        }

        renderer?.Dispose();
        helper.DestroySurface();
        helper.DestroyContext();
    }
}