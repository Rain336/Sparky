using System.Reflection;
using Android.Views;
using Java.Interop;
using SkiaSharp;
using Vulkan;
using Vulkan.Android;

namespace Sparky.Android.Vulkan;

public class VulkanHelper : IDisposable
{
    public const ImageUsageFlags ImageUsage = ImageUsageFlags.ColorAttachment;

    public static readonly string[] InstanceExtensions =
    {
        "VK_KHR_surface",
        "VK_KHR_android_surface"
    };

    public static readonly string[] DeviceExtensions =
    {
        "VK_KHR_swapchain"
    };

    public static readonly ApplicationInfo ApplicationInfo;

    private readonly Instance _instance;

    private IntPtr _window;
    private SurfaceKhr? _surface;
    private PhysicalDevice? _physicalDevice;
    private uint _maxSamples;
    private uint _graphicsQueueIndex;
    private uint _presentQueueIndex;
    private Device? _device;
    private GRContext? _context;
    private Queue? _graphicsQueue;
    private Queue? _presentQueue;
    private Fence? _hasImage;

    private SwapchainKhr? _swapchain;
    private Image[]? _images;

    public bool CanDraw => _device is not null && _swapchain is not null;

    public VulkanHelper()
    {
        var appName = Assembly.GetEntryAssembly()?.GetName();
        var frameworkName = typeof(Unit).Assembly.GetName();

        var appVersion = appName?.Version is null
            ? 0
            : global::Vulkan.Version.Make(
                (uint)appName.Version.Major,
                (uint)appName.Version.Minor,
                (uint)appName.Version.Revision
            );
        var frameworkVersion = global::Vulkan.Version.Make(
            (uint)frameworkName.Version!.Major,
            (uint)frameworkName.Version!.Minor,
            (uint)frameworkName.Version!.Revision
        );

        using var info = new ApplicationInfo
        {
            ApplicationName = appName?.Name ?? "",
            ApplicationVersion = appVersion,
            EngineName = "Sparky Framework",
            EngineVersion = frameworkVersion
        };

        _instance = new(new()
        {
            ApplicationInfo = ApplicationInfo,
            EnabledExtensionNames = InstanceExtensions
        });
    }

    #region Surface and Device

    public void CreateSurface(Surface surface)
    {
        _window = NativeMethods.NativeWindowFromSurface(JniEnvironment.EnvironmentPointer, surface.Handle);

        _surface = _instance.CreateAndroidSurfaceKHR(new()
        {
            Window = _window
        });

        if (_device is null)
        {
            CreateDevice();
        }
        else
        {
            TryUpdateSurface();
        }
    }

    public void DestroySurface()
    {
        if (_surface is not null)
        {
            _instance.DestroySurfaceKHR(_surface);
            _surface = null;
        }

        if (_window != IntPtr.Zero)
        {
            NativeMethods.NativeWindowRelease(_window);
            _window = IntPtr.Zero;
        }
    }

    private void CreateDevice()
    {
        if (_device is not null)
        {
            _device.WaitIdle();
            _context!.Dispose();
            _device.DestroyFence(_hasImage);
        }

        _physicalDevice = _instance.EnumeratePhysicalDevices().FirstOrDefault(
            x => x.EnumerateDeviceExtensionProperties().Any(x => x.ExtensionName == DeviceExtensions[0])
        );
        if (_physicalDevice is null)
        {
            throw new Exception("No fitting device found");
        }

        SetSampleCount();

        var extensions = GRVkExtensions.Create(
            GetProcedureAddress,
            ((IMarshalling)_instance).Handle,
            ((IMarshalling)_physicalDevice).Handle,
            InstanceExtensions,
            DeviceExtensions
        );

        var queueFamilies = _physicalDevice.GetQueueFamilyProperties();

        _graphicsQueueIndex = queueFamilies
            .SkipWhile(x => (x.QueueFlags & QueueFlags.Graphics) != 0)
            .Select((_, idx) => (uint)idx)
            .First();

        _presentQueueIndex = queueFamilies
            .Select((_, idx) => (uint)idx)
            .Last(x => _physicalDevice.GetSurfaceSupportKHR(x, _surface));

        _device = _physicalDevice.CreateDevice(new()
        {
            QueueCreateInfos = new[]
            {
                new DeviceQueueCreateInfo
                {
                    QueueFamilyIndex = _graphicsQueueIndex,
                    QueuePriorities = new[] { 1F }
                },
                new DeviceQueueCreateInfo
                {
                    QueueFamilyIndex = _presentQueueIndex,
                    QueuePriorities = new[] { 1F }
                }
            },
            EnabledExtensionNames = DeviceExtensions
        });

        _graphicsQueue = _device.GetQueue(_graphicsQueueIndex, 0);
        _presentQueue = _device.GetQueue(_presentQueueIndex, 0);

        var features2 = _physicalDevice.GetFeatures2KHR();

        _context = GRContext.CreateVulkan(new()
        {
            VkInstance = ((IMarshalling)_instance).Handle,
            VkPhysicalDevice = ((IMarshalling)_physicalDevice).Handle,
            VkDevice = ((IMarshalling)_device).Handle,
            VkQueue = ((IMarshalling)_graphicsQueue).Handle,
            GraphicsQueueIndex = _graphicsQueueIndex,
            Extensions = extensions,
            VkPhysicalDeviceFeatures2 = ((IMarshalling)features2).Handle,
            GetProcedureAddress = GetProcedureAddress
        });

        _hasImage = _device.CreateFence(new());
    }

    private void SetSampleCount()
    {
        var properties = _physicalDevice!.GetProperties();
        var counts = properties.Limits.FramebufferColorSampleCounts & properties.Limits.FramebufferDepthSampleCounts;
        if ((counts & SampleCountFlags.Count64) != 0)
        {
            _maxSamples = 64;
        }
        else if ((counts & SampleCountFlags.Count32) != 0)
        {
            _maxSamples = 32;
        }
        else if ((counts & SampleCountFlags.Count16) != 0)
        {
            _maxSamples = 16;
        }
        else if ((counts & SampleCountFlags.Count8) != 0)
        {
            _maxSamples = 8;
        }
        else if ((counts & SampleCountFlags.Count4) != 0)
        {
            _maxSamples = 4;
        }
        else if ((counts & SampleCountFlags.Count2) != 0)
        {
            _maxSamples = 2;
        }
        else
        {
            _maxSamples = 1;
        }
    }

    private void TryUpdateSurface()
    {
        if (!_physicalDevice!.GetSurfaceSupportKHR(_presentQueueIndex, _surface))
        {
            CreateDevice();
        }
    }

    #endregion

    #region Swapcahin

    public void CreateSwapchain(uint width, uint height, VulkanRenderer renderer)
    {
        if (_surface is null)
        {
            throw new InvalidOperationException("Trying to create Swapchain before Surface");
        }

        var capabilities = _physicalDevice!.GetSurfaceCapabilitiesKHR(_surface);

        var formats = _physicalDevice.GetSurfaceFormatsKHR(_surface);
        var format = formats.FirstOrDefault(
            x => x is { Format: Format.B8G8R8A8Srgb, ColorSpace: ColorSpaceKhr.SrgbNonlinear }
        );
        if (format.Format == Format.Undefined)
        {
            format = formats[0];
        }

        var modes = _physicalDevice.GetSurfacePresentModesKHR(_surface);
        var mode = modes.Contains(PresentModeKhr.Mailbox) ? PresentModeKhr.Mailbox : PresentModeKhr.Fifo;

        Extent2D extent;
        if (capabilities.CurrentExtent.Width != uint.MaxValue)
        {
            extent = capabilities.CurrentExtent;
        }
        else
        {
            extent = new()
            {
                Width = Math.Clamp(width, capabilities.MinImageExtent.Width, capabilities.MaxImageExtent.Width),
                Height = Math.Clamp(height, capabilities.MinImageExtent.Height, capabilities.MaxImageExtent.Height)
            };
        }

        var imageCount = capabilities.MinImageCount + 1;
        if (capabilities.MaxImageCount > 0 && imageCount > capabilities.MaxImageCount)
        {
            imageCount = capabilities.MaxImageCount;
        }

        var info = new SwapchainCreateInfoKhr
        {
            Surface = _surface,
            MinImageCount = imageCount,
            ImageFormat = format.Format,
            ImageColorSpace = format.ColorSpace,
            ImageExtent = extent,
            ImageArrayLayers = 1,
            ImageUsage = ImageUsage,
            PreTransform = capabilities.CurrentTransform,
            CompositeAlpha = CompositeAlphaFlagsKhr.Opaque,
            PresentMode = mode,
            Clipped = true,
            OldSwapchain = _swapchain
        };

        if (_graphicsQueueIndex != _presentQueueIndex)
        {
            info.ImageSharingMode = SharingMode.Concurrent;
            info.QueueFamilyIndices = new[]
            {
                _graphicsQueueIndex,
                _presentQueueIndex
            };
        }
        else
        {
            info.ImageSharingMode = SharingMode.Exclusive;
        }

        _swapchain = _device!.CreateSwapchainKHR(info);

        _images = _device.GetSwapchainImagesKHR(_swapchain);

        var maxSamples = (uint)_context!.GetMaxSurfaceSampleCount(format.Format.ToColorType());

        var imageInfo = new GRVkImageInfo
        {
            Format = (uint)format.Format,
            ImageUsageFlags = (uint)ImageUsage,
            SampleCount = _maxSamples > maxSamples ? maxSamples : _maxSamples,
            SharingMode = (uint)info.ImageSharingMode,
        };

        renderer.UpdateRenderTargets(_context, (int)extent.Width, (int)extent.Height, _images, imageInfo);
    }

    public void DestroySwapchain()
    {
        if (_swapchain is null)
        {
            return;
        }

        _device!.WaitIdle();
        _device.DestroySwapchainKHR(_swapchain);
    }

    #endregion

    public uint AcquireNextImage()
    {
        return _device!.AcquireNextImageKHR(_swapchain, ulong.MaxValue, null, _hasImage);
    }

    public void FlushAndPresent(SKCanvas canvas, uint idx)
    {
        _device!.WaitForFence(_hasImage, true, ulong.MaxValue);
        _device.ResetFence(_hasImage);

        canvas.Flush();
        _context!.Flush();

        _context.Submit(true);

        _presentQueue!.PresentKHR(new()
        {
            Swapchains = new[] { _swapchain },
            ImageIndices = new[] { idx }
        });
    }

    public void Dispose()
    {
        DestroySwapchain();
        DestroySurface();

        if (_device is not null)
        {
            _device.DestroyFence(_hasImage);
            _context!.Dispose();
        }

        _instance.Dispose();
    }

    private IntPtr GetProcedureAddress(string name, IntPtr instance, IntPtr device)
    {
        if (device != IntPtr.Zero)
        {
            return _device!.GetProcAddr(name);
        }

        return _instance.GetProcAddr(name);
    }
}