namespace Oxard.Maui.XControls.Extensions;

/// <summary>
/// Helpers for visual tree
/// </summary>
public static class VisualTreeExtensions
{
    /// <summary>
    /// Search the first parent of type <typeparamref name="T"/> in Element.Parent
    /// </summary>
    /// <typeparam name="T">Type of parent</typeparam>
    /// <param name="element">Instance</param>
    /// <returns>The parent of type <typeparamref name="T"/> if found; otherwise null</returns>
    public static T FindParent<T>(this Element element) where T : Element
    {
        var actualElement = element;

        while (actualElement.Parent != null && !(actualElement.Parent is T))
            actualElement = actualElement.Parent;

        return actualElement.Parent as T;
    }

    /// <summary>
    /// Search all child of type <typeparamref name="T"/> in this parent
    /// </summary>
    /// <typeparam name="T">Type of child</typeparam>
    /// <param name="element">Parent instance</param>
    /// <returns>All children of type <typeparamref name="T"/></returns>
    public static IEnumerable<T> FindChildren<T>(this Element element) where T : Element
    {
        List<T> children = new List<T>();
        if (element is ILayoutController layoutController)
        {
            foreach (var child in layoutController.Children)
            {
                if (child is T typedChild)
                    children.Add(typedChild);
                else
                    children.AddRange(child.FindChildren<T>());
            }
        }

        return children;
    }
}
