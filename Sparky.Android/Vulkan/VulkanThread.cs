using System.Collections.Concurrent;
using Android.Views;
using SkiaSharp;

namespace Sparky.Android.Vulkan;

public class VulkanThread : AndroidPlatform
{
    private readonly Thread _thread;
    private readonly ConcurrentQueue<Action<VulkanHelper, VulkanRenderer>> _tasks = new();
    private readonly CancellationTokenSource _cts = new();
    private bool _paused;
    private uint _width;
    private uint _height;
    private Java.Lang.IRunnable? _postDrawCallback;
    private Action<SKCanvas>? _callback;
    private Action<int, int>? _resize;

    private bool CanDraw => !_paused && _callback is not null && _width > 0 && _height > 0;

    public VulkanThread()
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
            callback((int)_width, (int)_height);
        });
    }

    public override void Start()
        => _thread.Start();

    public override void ReplaceSurface(Surface surface)
    {
        _tasks.Enqueue((helper, renderer) =>
        {
            helper.DestroySwapchain();
            helper.DestroySurface();
            helper.CreateSurface(surface);
            helper.CreateSwapchain(_width, _height, renderer);
        });
    }

    public override void DestroySurface()
    {
        _tasks.Enqueue((helper, _) =>
        {
            helper.DestroySwapchain();
            helper.DestroySurface();
        });
    }

    public override void ResizeSurface(Surface surface, int width, int height)
    {
        _tasks.Enqueue((helper, renderer) =>
        {
            helper.DestroySwapchain();
            helper.DestroySurface();
            helper.CreateSurface(surface);
            helper.CreateSwapchain((uint)width, (uint)height, renderer);
            _resize?.Invoke(width, height);
            _width = (uint)width;
            _height = (uint)height;
        });
    }

    public override void Pause()
    {
        _tasks.Enqueue((helper, _) =>
        {
            _paused = true;
            helper.DestroySwapchain();
            helper.DestroySurface();
        });
    }

    public override void Resume()
    {
        _tasks.Enqueue((_, _) => _paused = false);
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
        using var helper = new VulkanHelper();
        using var renderer = new VulkanRenderer();

        while (!_cts.IsCancellationRequested)
        {
            while (_tasks.TryDequeue(out var task))
            {
                task(helper, renderer);
            }

            if (CanDraw && helper.CanDraw && renderer.CanDraw)
            {
                var idx = helper.AcquireNextImage();

                var canvas = renderer.Draw(idx, _callback!);

                helper.FlushAndPresent(canvas, idx);

                if (_postDrawCallback is not null)
                {
                    _postDrawCallback.Run();
                    _postDrawCallback = null;
                }
            }
        }
    }
}