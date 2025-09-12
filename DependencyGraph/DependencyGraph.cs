// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// Version 1.3 - H. James de St. Germain Fall 2024
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)

namespace CS3500.DependencyGraph;

/// <summary>
///   <para>
///     (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
///     (in other words: s1 must be evaluated before t1.)
///   </para>
///   <para>
///     A DependencyGraph can be modeled as a set of ordered pairs of strings.
///     Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
///     if s1 equals s2 and t1 equals t2.
///   </para>
///   <remarks>
///     Recall that sets never contain duplicates.
///     If an attempt is made to add an element to a set, and the element is already
///     in the set, the set remains unchanged.
///   </remarks>
///   <para>
///     Given a DependencyGraph DG:
///   </para>
///   <list type="number">
///     <item>
///       If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///       (The set of things that depend on s.)
///     </item>
///     <item>
///       If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///       (The set of things that s depends on.)
///     </item>
///   </list>
///   <para>
///      For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}.
///   </para>
///   <code>
///     dependents("a") = {"b", "c"}
///     dependents("b") = {"d"}
///     dependents("c") = {}
///     dependents("d") = {"d"}
///     dependees("a")  = {}
///     dependees("b")  = {"a"}
///     dependees("c")  = {"a"}
///     dependees("d")  = {"b", "d"}
///   </code>
/// </summary>
public class DependencyGraph
{
    // The key of this dictionary is the dependee (parent), which point to a set of dependents (children).
    private Dictionary<string, HashSet<string>> parents;
    // The key of this dictionary is the dependent (child), which point to a set of dependees (parents).
    private Dictionary<string, HashSet<string>> children;
    // Tracks the number of ordered pairs in the graph.
    private int count;
    /// <summary>
    ///   Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///   The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        parents = new Dictionary<string, HashSet<string>>();
        children = new Dictionary<string, HashSet<string>>();
        count = 0;
    }

    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return count; }
    }

    /// <summary>
    ///   Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {

        if (!parents.ContainsKey(nodeName))
        {
            return false;
        }
        // Could be a case that the key is created but replaceDependents passes an empty list to the key, so we check count.
        else
        {
            return parents[nodeName].Count > 0;
        }
    }

    /// <summary>
    ///   Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        if (!children.ContainsKey(nodeName))
        {
            return false;
        }
        // Could be a case that the key is created but replaceDependees passes an empty list to the key, so we check count.
        else
        {
            return children[nodeName].Count > 0;
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependents of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        if (!parents.ContainsKey(nodeName))
        {
            return new HashSet<string>();
        }
        else
        {
            return parents[nodeName];
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependees of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        if (!children.ContainsKey(nodeName))
        {
            return new HashSet<string>();
        }
        else
        {
            return children[nodeName];
        }
    }

    /// <summary>
    /// <para> 
    ///   Adds the ordered pair (dependee, dependent), if it doesn't already exist (otherwise nothing happens).
    /// </para>
    /// <para>
    ///   This can be thought of as: dependee must be evaluated before dependent.
    /// </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first. </param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after the other node has been. </param>
    public void AddDependency(string dependee, string dependent)
    {
        bool entryAdded = false;
        // Create the parent (dependent) entry if it doesn't exist, then add the child (dependent).
        if (!parents.ContainsKey(dependee))
        {
            parents[dependee] = new HashSet<string>();
            parents[dependee].Add(dependent);
            entryAdded = true;
        }

        // Check if the dependent already exists for this dependee.
        else if (parents[dependee].Add(dependent))
        {
            entryAdded = true;
        }

        // Create the child entry if it doesn't exist, then add the parent (dependee).
        if (!children.ContainsKey(dependent))
        {
            children[dependent] = new HashSet<string>();
            children[dependent].Add(dependee);
        }
        else
        {
            children[dependent].Add(dependee);
        }

        // If we added a new ordered pair, increment the count.
        if (entryAdded)
        {
            count++;
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists (otherwise nothing happens).
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first. </param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until the other node has been. </param>
    public void RemoveDependency(string dependee, string dependent)
    {
        if (parents.ContainsKey(dependee))
        {
            // Note: Nothing will be removed here if the dependent doesn't exist in the set.
            parents[dependee].Remove(dependent);
            // Need to check that this ordered pair actually exists
            if (children.ContainsKey(dependent))
            {
                if(children[dependent].Remove(dependee))
                {
                    count--;
                }
            }
        }
    }

    /// <summary>
    ///   Removes all existing ordered pairs of the form (nodeName, *).  Then, for each
    ///   t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependents are being replaced. </param>
    /// <param name="newDependents"> The new dependents for nodeName. </param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {
        // If the parent doesn't exist, just add the new dependencies.
        if (parents.ContainsKey(nodeName))
        {
            // Remove dependencies from parents
            var currentDependents = new HashSet<string>(parents[nodeName]);
            foreach (var dependent in currentDependents)
            {
                RemoveDependency(nodeName, dependent);
            }
        }
        // Now, create the new dependencies
        foreach (var newDependent in newDependents)
        {
            AddDependency(nodeName, newDependent);
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes all existing ordered pairs of the form (*, nodeName).  Then, for each
    ///     t in newDependees, adds the ordered pair (t, nodeName).
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependees are being replaced. </param>
    /// <param name="newDependees"> The new dependees for nodeName. Could be empty.</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        // If the child doesn't exist, just add the new dependencies.
        if (children.ContainsKey(nodeName))
        {
            // Remove dependencies from children
            var currentDependees = new HashSet<string>(children[nodeName]);
            foreach (var dependee in currentDependees)
            {
                RemoveDependency(dependee, nodeName);
            }
        }
        // Now, create the new dependencies
        foreach (var newDependee in newDependees)
        {
            AddDependency(newDependee, nodeName);
        }
    }
}
