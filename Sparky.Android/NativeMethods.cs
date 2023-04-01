using System.Runtime.InteropServices;
using System.Security;

namespace Sparky.Android;

[SuppressUnmanagedCodeSecurity]
internal static class NativeMethods
{
    private const string AndroidRuntimeLibrary = "android";

    [DllImport(AndroidRuntimeLibrary, EntryPoint = "ANativeWindow_fromSurface")]
    internal static extern IntPtr NativeWindowFromSurface(IntPtr jniEnv, IntPtr handle);

    [DllImport(AndroidRuntimeLibrary, EntryPoint = "ANativeWindow_release")]
    internal static extern void NativeWindowRelease(IntPtr window);
}