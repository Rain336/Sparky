using SkiaSharp;
using Vulkan;

namespace Sparky.Android;

public static class SkiaVulkanExtensions
{
    public static SKColorType ToColorType(this Format format)
    {
        return format switch
        {
            Format.R5G6B5UnormPack16 => SKColorType.Rgb565,
            Format.R8G8B8A8Unorm or Format.R8G8B8A8Snorm or Format.R8G8B8A8Uscaled or Format.R8G8B8A8Sscaled
                or Format.R8G8B8A8Uint or Format.R8G8B8A8Sint or Format.R8G8B8A8Srgb => SKColorType.Rgba8888,
            Format.R8G8B8Unorm or Format.R8G8B8Snorm or Format.R8G8B8Uscaled or Format.R8G8B8Sscaled
                or Format.R8G8B8Uint or Format.R8G8B8Sint or Format.R8G8B8Srgb => SKColorType.Rgb888x,
            Format.B8G8R8A8Unorm or Format.B8G8R8A8Snorm or Format.B8G8R8A8Uscaled or Format.B8G8R8A8Sscaled
                or Format.B8G8R8A8Uint or Format.B8G8R8A8Sint or Format.B8G8R8A8Srgb => SKColorType.Bgra8888,
            Format.R8Unorm or Format.R8Snorm or Format.R8Uscaled or Format.R8Sscaled
                or Format.R8Uint or Format.R8Sint or Format.R8Srgb => SKColorType.Gray8,
            Format.R8G8Unorm or Format.R8G8Snorm or Format.R8G8Uscaled or Format.R8G8Sscaled
                or Format.R8G8Uint or Format.R8G8Sint or Format.R8G8Srgb => SKColorType.Rg88,
            Format.R16G16Unorm or Format.R16G16Snorm or Format.R16G16Uscaled or Format.R16G16Sscaled
                or Format.R16G16Uint or Format.R16G16Sint => SKColorType.Rg1616,
            Format.R16G16B16A16Unorm or Format.R16G16B16A16Snorm or Format.R16G16B16A16Uscaled
                or Format.R16G16B16A16Sscaled or Format.R16G16B16A16Uint
                or Format.R16G16B16A16Sint => SKColorType.Rgba16161616,
            _ => SKColorType.Unknown
        };
    }
}