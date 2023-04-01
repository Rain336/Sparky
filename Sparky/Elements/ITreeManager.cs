using Sparky.Components;

namespace Sparky.Elements;

/// <summary>
/// The tree manager is responsible for rebuilding changed elements during a render pass.
/// </summary>
public interface ITreeManager
{
    /// <summary>
    /// Tells whenever the tree manager is currently building the tree. 
    /// </summary>
    bool IsBuilding { get; }
    
    /// <summary>
    /// The root element, if a root has been attached.
    /// </summary>
    Element? Root { get; }
    
    /// <summary>
    /// Attaches a new root element.
    /// This is called at startup to attach the initial root,
    /// but can be called to switch to a new one.
    /// </summary>
    /// <param name="root">The new root Component of the tree</param>
    void AttachRoot(Component root);

    /// <summary>
    /// Marks the given element for rebuild in the next render pass.
    /// </summary>
    /// <param name="element">The element to rebuild</param>
    void MarkForRebuild(Element element);

    /// <summary>
    /// marks the given element for deletion at the end of the render pass.
    /// </summary>
    /// <param name="element">The element to delete</param>
    void MarkForDelete(Element element);
}