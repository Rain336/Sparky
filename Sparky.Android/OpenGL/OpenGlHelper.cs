using Android.Views;
using Java.Interop;
using Javax.Microedition.Khronos.Egl;

namespace Sparky.Android.OpenGL;

public class OpenGlHelper
{
    private static readonly int[] ConfigSpecGles2 =
    {
        IEGL10.EglRedSize, 8,
        IEGL10.EglGreenSize, 8,
        IEGL10.EglBlueSize, 8,
        IEGL10.EglAlphaSize, 0,
        IEGL10.EglDepthSize, 16,
        IEGL10.EglStencilSize, 0,
        IEGL10.EglRenderableType, 4,
        IEGL10.EglNone
    };

    private static readonly int[] ConfigSpec =
    {
        IEGL10.EglRedSize, 8,
        IEGL10.EglGreenSize, 8,
        IEGL10.EglBlueSize, 8,
        IEGL10.EglAlphaSize, 0,
        IEGL10.EglDepthSize, 16,
        IEGL10.EglStencilSize, 0,
        IEGL10.EglNone
    };

    public IEGL10? Egl { get; private set; }
    public EGLDisplay? Display { get; private set; }
    public EGLConfig? Config { get; private set; }
    public EGLContext? Context { get; private set; }
    public EGLSurface? Surface { get; private set; }

    public bool CanDraw => Surface is not null && Context is not null;

    public void CreateContext()
    {
        Egl = EGLContext.EGL.JavaCast<IEGL10>();
        Display = Egl!.EglGetDisplay(IEGL10.EglDefaultDisplay);
        if (Display == IEGL10.EglNoDisplay)
        {
            throw new PlatformNotSupportedException("Could not get OpenGL display");
        }

        var version = new int[2];
        if (Egl.EglInitialize(Display, version))
        {
            throw new PlatformNotSupportedException("Could not initialize OpenGL display");
        }

        Config = FindConfig(version[0]);
        Context = Egl.EglCreateContext(
            Display,
            Config,
            IEGL10.EglNoContext,
            new[]
            {
                0x00003098, version[0],
                IEGL10.EglNone
            });

        if (Context is null || Context == IEGL10.EglNoContext)
        {
            throw new PlatformNotSupportedException("Could not create OpenGL context");
        }
    }

    public void DestroyContext()
    {
        if (Egl is null)
        {
            return;
        }

        if (Context is not null)
        {
            if (!Egl.EglDestroyContext(Display, Context))
            {
                throw new PlatformNotSupportedException("Failed to destroy OpenGL context");
            }

            Context = null;
        }

        if (Display is not null)
        {
            Egl.EglTerminate(Display);
            Display = null;
        }
    }

    public void CreateSurface(Surface surface)
    {
        if (Egl is null)
        {
            throw new InvalidOperationException("An OpenGL context is required before creating a surface");
        }

        Surface = Egl.EglCreateWindowSurface(Display, Config, surface, null);

        if (Surface is null || Surface == IEGL10.EglNoSurface)
        {
            throw new PlatformNotSupportedException("Could not create OpenGL surface");
        }

        if (!Egl.EglMakeCurrent(Display, Surface, Surface, Context))
        {
            throw new PlatformNotSupportedException("Could not connect OpenGL context to surface");
        }
    }

    public void DestroySurface()
    {
        if (Surface is null)
        {
            return;
        }

        if (!Egl.EglMakeCurrent(Display, IEGL10.EglNoSurface, IEGL10.EglNoSurface, IEGL10.EglNoContext))
        {
            throw new PlatformNotSupportedException("Could not disconnect OpenGL context from surface");
        }

        if (!Egl.EglDestroySurface(Display, Surface))
        {
            throw new PlatformNotSupportedException("Could not destroy OpenGL surface");
        }

        Surface = null;
    }

    public void Swap()
    {
        if (Egl is null || Surface is null)
        {
            throw new PlatformNotSupportedException("Cannot swap without a Surface");
        }

        if (!Egl.EglSwapBuffers(Display, Surface))
        {
            switch (Egl.EglGetError())
            {
                case IEGL10.EglSuccess:
                    return;

                case IEGL11.EglContextLost:
                    DestroySurface();
                    DestroyContext();
                    CreateContext();
                    return;

                default:
                    DestroySurface();
                    return;
            }
        }
    }

    private EGLConfig FindConfig(int majorVersion)
    {
        var buffer = new int[1];
        if (!Egl!.EglChooseConfig(
                Display,
                majorVersion == 2 ? ConfigSpecGles2 : ConfigSpec,
                null,
                0,
                buffer
            ))
        {
            throw new PlatformNotSupportedException("Could not retrieve config count");
        }

        if (buffer[0] <= 0)
        {
            throw new PlatformNotSupportedException("No configuration found");
        }

        var configs = new EGLConfig[buffer[0]];
        if (!Egl.EglChooseConfig(
                Display,
                majorVersion == 2 ? ConfigSpecGles2 : ConfigSpec,
                configs,
                buffer[0],
                buffer
            ))
        {
            throw new PlatformNotSupportedException("Could not retrieve configurations");
        }

        var config = configs.FirstOrDefault(x =>
            Egl.EglGetConfigAttrib(Display, x, IEGL10.EglDepthSize, buffer) && buffer[0] >= 16 &&
            Egl.EglGetConfigAttrib(Display, x, IEGL10.EglRedSize, buffer) && buffer[0] == 8 &&
            Egl.EglGetConfigAttrib(Display, x, IEGL10.EglGreenSize, buffer) && buffer[0] == 8 &&
            Egl.EglGetConfigAttrib(Display, x, IEGL10.EglBlueSize, buffer) && buffer[0] == 8 &&
            Egl.EglGetConfigAttrib(Display, x, IEGL10.EglAlphaSize, buffer) && buffer[0] == 0
        );

        if (config is null)
        {
            throw new PlatformNotSupportedException("No compatible configuration found");
        }

        return config;
    }
}