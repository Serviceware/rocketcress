using Rocketcress.Core;

namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

/// <summary>
/// Represents a <see cref="ISearchPart"/> that searched for descendant elements.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.SearchParts.SearchPartBase" />
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.INestedSearchPart" />
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.IDepthSearchPart" />
public class DescendantsSearchPart : SearchPartBase, INestedSearchPart, IDepthSearchPart
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DescendantsSearchPart"/> class.
    /// </summary>
    /// <param name="maxDepth">The maximum search depth.</param>
    public DescendantsSearchPart(int maxDepth)
        : this(maxDepth, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescendantsSearchPart"/> class.
    /// </summary>
    /// <param name="maxDepth">The maximum search depth.</param>
    /// <param name="condition">The condition.</param>
    public DescendantsSearchPart(int maxDepth, ISearchCondition condition)
        : this(maxDepth, condition, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescendantsSearchPart"/> class.
    /// </summary>
    /// <param name="maxDepth">The maximum search depth.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="childPart">The child part.</param>
    public DescendantsSearchPart(int maxDepth, ISearchCondition condition, ISearchPart childPart)
    {
        MaxDepth = maxDepth;
        ChildPart = childPart;
        Condition = condition;
    }

    /// <inheritdoc/>
    public int MaxDepth { get; set; }

    /// <inheritdoc/>
    public ISearchPart ChildPart { get; set; }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        var prefix = MaxDepth == 1 ? "/" : MaxDepth < 0 ? "//" : $"//{{{MaxDepth}}}";
        return prefix + GetConditionDescription() + GetSkipTakeDescription();
    }

    /// <inheritdoc/>
    protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
    {
        IEnumerable<AutomationElement> currentLevel = new[] { element };
        List<AutomationElement> nextLevel;
        var depth = 0;

        if (element == AutomationElement.RootElement && (MaxDepth < 0 || MaxDepth > 2))
        {
            Logger.LogWarning("Searching for a control with the Scope Descendants on the root can lead to long search times. " +
                "You can reduce this time by either setting a parent, use Scope Children or set MaxDepth.");
        }

        while (currentLevel.Any() && (depth < MaxDepth || MaxDepth < 0))
        {
            nextLevel = new List<AutomationElement>();

            foreach (var child in currentLevel.SelectMany(x => GetChildren(x, treeWalker)))
            {
                nextLevel.Add(child);
                if (Condition?.Check(child, treeWalker) != false)
                {
                    if (ChildPart == null)
                    {
                        yield return child;
                    }
                    else
                    {
                        foreach (var res in ChildPart.FindElements(child, treeWalker))
                            yield return child;
                    }
                }

                nextLevel.Add(child);
            }

            currentLevel = nextLevel;
            depth++;
        }
    }

    /// <inheritdoc/>
    protected override SearchPartBase CloneInternal()
    {
        var childPart = (ISearchPart)ChildPart?.Clone();
        return new DescendantsSearchPart(MaxDepth, null, childPart);
    }

    private static IEnumerable<AutomationElement> GetChildren(AutomationElement element, TreeWalker treeWalker)
    {
        var current = treeWalker.GetFirstChild(element);
        while (current != null)
        {
            yield return current;
            current = treeWalker.GetNextSibling(current);
        }
    }
}
