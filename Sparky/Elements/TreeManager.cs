using System.Diagnostics;
using Sparky.Components;

namespace Sparky.Elements;

internal class TreeManager : ITreeManager
{
    private List<Element> _requireRebuild = new();
    private List<Element> _requireDelete = new();
    private int _width;
    private int _height;

    public Element? Root { get; private set; }

    public bool Ready => Root is not null;

    public bool IsBuilding { get; private set; }

    public void AttachRoot(Component root)
    {
        var component = new RootComponent(root, _width, _height);
        Root = component.CreateElement();
        Root.MountAsRoot(this);
    }

    public void MarkForRebuild(Element element)
    {
        Debug.Assert(!element.Dirty);
        _requireRebuild.Add(element);
    }

    public void MarkForDelete(Element element)
    {
        _requireDelete.Add(element);
    }

    internal void Resize(int width, int height)
    {
        _width = width;
        _height = height;
        if (Root is RootElement root)
        {
            root.Resize(width, height);
        }
    }

    internal void Rebuild()
    {
        if (!Ready)
        {
            return;
        }

        IsBuilding = true;
        var array = Interlocked.Exchange(ref _requireRebuild, new());
        array.Sort(DirtyElementComparer.Instance);

        var i = 0;
        while (i < array.Count)
        {
            var element = array[i];
            element.Rebuild();

            array.Sort(DirtyElementComparer.Instance);
            while (i > 0 && array[i].Dirty)
            {
                i--;
            }

            if (!array[i].Dirty)
            {
                i++;
            }
        }

        IsBuilding = false;
    }

    internal void Remove()
    {
        if (!Ready)
        {
            return;
        }

        var array = Interlocked.Exchange(ref _requireDelete, new());

        for (var i = array.Count - 1; i >= 0; i--)
        {
            array[i].Dispose();
        }
    }

    private class DirtyElementComparer : IComparer<Element>
    {
        public static readonly DirtyElementComparer Instance = new();

        private DirtyElementComparer()
        {
        }

        public int Compare(Element? x, Element? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(null, y))
            {
                return 1;
            }

            if (ReferenceEquals(null, x))
            {
                return -1;
            }

            if (x.Dirty && !y.Dirty)
            {
                return 1;
            }

            if (!x.Dirty && y.Dirty)
            {
                return -1;
            }

            return 0;
        }
    }
}