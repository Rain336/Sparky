using DryIoc;
using SkiaSharp;
using Sparky.Actors;
using Sparky.Elements;

namespace Sparky.Application;

/// <summary>
/// Represents the entry point into the framework.
/// </summary>
public static class SparkyFramework
{
    /// <summary>
    /// Starts up the framework by setting up a DI container and platform callbacks.
    /// </summary>
    /// <param name="platform">The platform to register to</param>
    /// <typeparam name="T">The app instance to use for initialization</typeparam>
    public static void Run<T>(IPlatform platform)
        where T : ISparkyApp
    {
        var container = new Container();
        container.RegisterInstance(platform);
        container.Register<ITreeManager, TreeManager>(reuse: Reuse.Singleton);
        container.Register<IActorManager, ActorManager>(reuse: Reuse.Singleton);
        container.Register<ISparkyApp, T>(reuse: Reuse.Singleton);

        var app = container.Resolve<ISparkyApp>();
        app.ConfigureContainer(container);

        Locator.Initialize(container);

        platform.SetResizeCallback(Resize);
        platform.SetDrawCallback(Draw);
    }

    private static void Resize(int width, int height)
    {
        var tree = (TreeManager)Locator.Resolve<ITreeManager>();
        tree.Resize(width, height);
    }

    private static void Draw(SKCanvas canvas)
    {
        var tree = (TreeManager)Locator.Resolve<ITreeManager>();
        tree.Rebuild();

        var actor = (ActorManager)Locator.Resolve<IActorManager>();
        // TODO: Render Actors

        tree.Remove();
    }
}