namespace CS3500.DevelopmentTests;

using CS3500.DependencyGraph;

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphExampleStressTests
{
    /// <summary>
    /// This test creates a large string array of 200 with characters a,b,c... throughout the ascii table. These characters are then iterated through repeatedly to add and remove dependencies, 
    /// testing if these operations are done within two seconds and are also done accurately (each dependency is added and removed correctly).
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void StressTest()
    {
        DependencyGraph dg = new();

        // A bunch of strings to use
        const int SIZE = 200;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }

        // The correct answers
        HashSet<string>[] dependents = new HashSet<string>[SIZE];
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Add a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 4; j < SIZE; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Add some back
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove some more
        for (int i = 0; i < SIZE; i += 2)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Make sure everything is right
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new HashSet<string>(dg.GetDependees(letters[i]))));
        }
    }
    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, a new dependency graph is empty when first created.
    /// As such, the size should be zero.
    /// </summary>
    [TestMethod]
    public void TestEmptyBySize()
    {
        DependencyGraph dg = new();
        Assert.AreEqual(0, dg.Size);
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, a new dependency graph is empty when first created.
    /// As such, HasDependees should always return false.
    /// </summary>
    [TestMethod]
    public void TestEmptyByDependees()
    {
        DependencyGraph dg = new();
        Assert.IsFalse(dg.HasDependees("A1"));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, a new dependency graph is empty when first created.
    /// As such, HasDependent should always return false.
    /// </summary>
    [TestMethod]
    public void TestEmptyByDependents()
    {
        DependencyGraph dg = new();
        Assert.IsFalse(dg.HasDependents("A1"));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created and a dependency is added,
    /// size should be one.
    /// </summary>
    [TestMethod]
    public void TestSizeAdd()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        Assert.AreEqual(1, dg.Size);
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created and a dependency is added,
    /// then removed, size should be zero.
    /// </summary>
    [TestMethod]
    public void TestSizeRemove()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.RemoveDependency("A1", "B1");
        Assert.AreEqual(0, dg.Size);
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created and a dependency is added,
    /// then removed, the dependency should no longer exist, and thus hasDependees returns False.
    /// </summary>
    [TestMethod]
    public void TestRemoveDependees()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.RemoveDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependees("A1"));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created and a dependency is added,
    /// then removed, the dependency should no longer exist, and thus hasDependents returns False.
    /// </summary>
    [TestMethod]
    public void TestRemoveDependents()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.RemoveDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependents("A1"));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created
    /// and dependents are added to the same Dependee, a proper list of ALL dependents is returned.
    /// </summary>
    [TestMethod]
    public void TestAddDependents()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A1", "C1");
        dg.AddDependency("A1", "A2");
        Assert.IsTrue(new HashSet<string> { "B1" , "C1" , "A2" }.SetEquals(dg.GetDependents("A1")));
    }

}
