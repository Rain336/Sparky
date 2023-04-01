using System.Reflection;
using DryIoc;
using Sparky.Actors;
using Sparky.Application;
using Sparky.Elements;

namespace Sparky.Testing;

public static class SparkyTesting
{
    public static void EnsureInitialized()
    {
        if (Locator.Container is not null)
        {
            return;
        }
        
        var container = new Container();
        container.Register<ITreeManager, TreeManager>(reuse: Reuse.Singleton);
        container.Register<IActorManager, ActorManager>(reuse: Reuse.Singleton);
        
        var entry = Assembly.GetEntryAssembly();
        var attribute = entry?.GetCustomAttribute<TestingAppAttribute>();
        if (attribute?.TestingApp is not null)
        {
            container.Register(typeof(ISparkyApp), attribute.TestingApp, Reuse.Singleton);
            var app = container.Resolve<ISparkyApp>();
            app.ConfigureContainer(container);
        }
        
        Locator.Initialize(container);
    }
}