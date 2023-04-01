using Sparky.Actors;
using Sparky.Components;
using Sparky.Elements;
using Sparky.Testing.Finding;

namespace Sparky.Testing;

public abstract class ComponentTestBase
{
    protected ITreeManager TreeManager { get; }
    protected IActorManager ActorManager { get; }

    protected ComponentTestBase()
    {
        SparkyTesting.EnsureInitialized();

        TreeManager = Locator.Resolve<ITreeManager>();
        ActorManager = Locator.Resolve<IActorManager>();
    }

    protected void RenderComponent(Component component)
    {
        TreeManager.AttachRoot(component);
    }

    protected void Render()
    {
        var manager = (TreeManager)TreeManager;
        manager.Rebuild();

        manager.Remove();
    }

    protected FindByComponentResult<T> FindByComponent<T>()
        where T : Component
    {
        var result = new List<Element>();

        void FindByComponent(Element element)
        {
            if (element.Component is T)
            {
                result.Add(element);
            }

            element.VisitChildren(FindByComponent);
        }

        TreeManager.Root?.VisitChildren(FindByComponent);
        return new FindByComponentResult<T>(result);
    }

    protected T[] FindByActor<T>()
        where T : Actor
    {
        var result = new List<T>();

        void FindByActor(Actor actor)
        {
            if (actor is T t)
            {
                result.Add(t);
            }
            
            actor.VisitChildren(FindByActor);
        }
        
        ActorManager.Root?.VisitChildren(FindByActor);
        return result.ToArray();
    }
}