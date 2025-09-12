/// <summary>
/// Author:    Francisco Pinas
/// Partner:   N/A
/// Date:      9/11/2025
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Francisco - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Francisco, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    This file contains the tests for the DependencyGraph class. It ensures that proper lists are returned for the dependents and dependees of a given node,
///    it ensures that these processes are done in a timely manner, and it ensures that the size of the dependency graph is tracked correctly.
/// </summary>

namespace CS3500.DevelopmentTests;

using CS3500.DependencyGraph;

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
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
    /// This stress tests creates a ton of dependency graph objects and constantly replaces them. 
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void StressTestCreateAndReplace()
    {
        for(int i = 0; i < 1000; i++)
        {
            DependencyGraph dg = new();
            dg.AddDependency("" + (char) (i), "" + (char) (i + 1));
            dg.AddDependency("" + (char)(i + 2), "" + (char)(i + 3));
            dg.AddDependency("" + (char)(i + 4), "" + (char)(i + 5));
            dg.ReplaceDependents("A", new HashSet<string> { "X", "Y", "Z" });
            dg.ReplaceDependees("D", new HashSet<string> { "W", "V" });
            Assert.IsTrue(new HashSet<string> { "X", "Y", "Z" }.SetEquals(dg.GetDependents("A")));
            Assert.IsTrue(new HashSet<string> { "W", "V" }.SetEquals(dg.GetDependees("D")));
        }
    }
        // -- BLACK BOX TESTS PRIOR TO IMPLEMENTATION OF DEPENDENCY GRAPH --

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
        Assert.IsFalse(dg.HasDependees("B1"));
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
    /// and dependents are added to the same dependee, a proper list of ALL dependents is returned.
    /// </summary>
    [TestMethod]
    public void TestAddDependents()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A1", "C1");
        dg.AddDependency("A1", "A2");
        Assert.IsTrue(new HashSet<string> { "B1", "C1", "A2" }.SetEquals(dg.GetDependents("A1")));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created
    /// and dependees are added to the same dependant, a proper list of ALL dependees is returned.
    /// </summary>
    [TestMethod]
    public void TestAddDependees()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A2", "B1");
        dg.AddDependency("C1", "B1");
        Assert.IsTrue(new HashSet<string> { "A1", "A2", "C1" }.SetEquals(dg.GetDependees("B1")));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created
    /// and dependents are replaced, the new list of dependents is adopted correctly.
    /// </summary>
    [TestMethod]
    public void TestReplaceDependents()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A1", "C1");
        dg.AddDependency("A1", "A2");
        HashSet<string> replacement = new HashSet<string> { "C5", "D1", "F2" };
        dg.ReplaceDependents("A1", replacement);
        Assert.IsTrue(new HashSet<string> { "C5", "D1", "F2" }.SetEquals(dg.GetDependents("A1")));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created
    /// and dependees are replaced, the new list of dependees is adopted correctly.
    /// </summary>
    [TestMethod]
    public void TestReplaceDependees()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A2", "B1");
        dg.AddDependency("C1", "B1");
        HashSet<string> replacement = new HashSet<string> { "Z8", "G13", "F42" };
        dg.ReplaceDependees("B1", replacement);
        Assert.IsTrue(new HashSet<string> { "Z8", "G13", "F42" }.SetEquals(dg.GetDependees("B1")));
    }

    // -- END OF BLACK BOX TESTS, START OF WHITE BOX TESTS --

    /// <summary>
    /// Simply put, testing that creating a new dependency graph doesn't cause any crashes.
    /// </summary>
    [TestMethod]
    public void TestCreateDependencyGraph()
    {
        new DependencyGraph();
    }

    /// <summary>
    /// This tests that getting dependees of a non-existent node does not cause a crash and returns an empty set.
    /// </summary>
    [TestMethod]
    public void TestGetDependeesNonExistent()
    {
        DependencyGraph dg = new();
        dg.AddDependency("D1", "B1");
        Assert.IsTrue(new HashSet<string> { }.SetEquals(dg.GetDependees("A1")));
    }

    /// <summary>
    /// This tests that getting dependents of a non-existent node does not cause a crash, and returns an empty set.
    /// </summary>
    [TestMethod]
    public void TestGetDependentsNonExistent()
    {
        DependencyGraph dg = new();
        dg.AddDependency("B1", "A1");
        Assert.IsTrue(new HashSet<string> { }.SetEquals(dg.GetDependents("z9")));
    }

    /// <summary>
    /// This tests that checking for dependents of a non-existent node does not cause a crash.
    /// </summary>
    [TestMethod]
    public void TestHasDependentsNonExistent()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependents("test"));
    }

    /// <summary>
    /// This tests that checking for dependees of a non-existent node does not cause a crash.
    /// </summary>
    [TestMethod]
    public void TestHasDependeesNonExistent()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependees("nonexistent"));
    }

    /// <summary>
    /// This tests that when a dependency is added, HasDependees returns true for the dependent.
    /// </summary>
    [TestMethod]
    public void TestHasDependeesTrue()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsTrue(dg.HasDependees("B1"));
    }
    /// <summary>
    /// This tests that when a dependency is added, HasDependents returns true for the dependee.
    /// </summary>
    [TestMethod]
    public void TestHasDependentsTrue()
    {
        DependencyGraph dg = new();
        dg.AddDependency("C1", "88");
        Assert.IsTrue(dg.HasDependents("C1"));
    }

    /// <summary>
    /// This tests that when a dependency is added, the dependee isn't mistakenly marked as having dependees.
    /// </summary>
    [TestMethod]
    public void TestHasDependentsFalse()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependents("B1"));
    }
    /// <summary>
    /// This tests that when a dependency is added, the dependent isn't mistakenly marked as having dependents.
    /// </summary>
    [TestMethod]
    public void TestHasDependeesFalse()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependees("A1"));
    }
    /// <summary>
    /// This tests that adding a duplicate dependency does not increase the size of the dependency graph.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyDuplicate()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A1", "B1");
        Assert.AreEqual(1, dg.Size);
    }
    /// <summary>
    /// This tests that adding a dependency of one string to itself is counted once. 
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfSize()
    {
        DependencyGraph dg = new();
        dg.AddDependency("b11", "b11");
        Assert.AreEqual(1, dg.Size);
    }

    /// <summary>
    /// This tests that adding a dependency of one string to itself displays tracks that it has dependees.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfHasDependees()
    {
        DependencyGraph dg = new();
        dg.AddDependency("b11", "b11");
        Assert.IsTrue(dg.HasDependees("b11"));
    }

    /// <summary>
    /// This tests that adding a dependency of one string to itself displays tracks that it has dependents.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfHasDependents()
    {
        DependencyGraph dg = new();
        dg.AddDependency("b11", "b11");
        Assert.IsTrue(dg.HasDependents("b11"));
    }

    /// <summary>
    /// This tests that adding a dependency of one string to itself does not break the list of dependents.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfCorrectDependents()
    {
        DependencyGraph dg = new();
        dg.AddDependency("duplicate", "duplicate");
        dg.AddDependency("duplicate", "unique");
        Assert.IsTrue(new HashSet<string> { "duplicate", "unique" }.SetEquals(dg.GetDependents("duplicate")));
    }

    /// <summary>
    /// This tests that adding a dependency of one string to itself does not break the list of dependents.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfCorrectDependees()
    {
        DependencyGraph dg = new();
        dg.AddDependency("duplicate", "duplicate");
        dg.AddDependency("duplicate", "unique");
        Assert.IsTrue(new HashSet<string> { "duplicate" }.SetEquals(dg.GetDependees("duplicate")));
    }
    /// <summary>
    /// Removing a non-existant dependee from a dependency should not change the ones that do exist, and should not crash.
    /// </summary>
    [TestMethod]
    public void TestRemoveDependencyNonExistentDependee()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.RemoveDependency("A1", "C1");
        Assert.AreEqual(1, dg.Size);
        Assert.IsTrue(new HashSet<string> { "B1" }.SetEquals(dg.GetDependents("A1")));
        Assert.IsTrue(new HashSet<string> { "A1" }.SetEquals(dg.GetDependees("B1")));
    }
    /// <summary>
    /// Removing a non-existant dependent from a dependency should not change the ones that do exist, and should not crash.
    /// </summary>
    [TestMethod]
    public void TestRemoveDependencyNonExistentDependent()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.RemoveDependency("C1", "B1");
        Assert.AreEqual(1, dg.Size);
        Assert.IsTrue(new HashSet<string> { "B1" }.SetEquals(dg.GetDependents("A1")));
        Assert.IsTrue(new HashSet<string> { "A1" }.SetEquals(dg.GetDependees("B1")));
    }

    /// <summary>
    /// Removing a non-existant dependency from an empty dependency graph should not cause a crash.
    /// </summary>
    [TestMethod]
    public void TestRemoveDependencyFromEmpty()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "B1");
        dg.RemoveDependency("A1", "Chrome");
    }
    /// <summary>
    /// Replacing dependents of an empty dependency graph should not crash, and in fact simply create the new dependencies. 
    /// </summary>
    [TestMethod]
    public void TestReplaceDependentsFromEmpty()
    {
        DependencyGraph dg = new();
        dg.ReplaceDependents("A1", new HashSet<string> { "B1", "C1" });
        Assert.AreEqual(2, dg.Size);
        Assert.IsTrue(new HashSet<string> { "B1", "C1" }.SetEquals(dg.GetDependents("A1")));
    }

    /// <summary>
    /// Replacing dependents of a non-existent dependee should not crash, and in fact simply create the new dependencies. 
    /// Also ensuring that existing uninvolved dependencies are not changed.
    /// </summary>
    [TestMethod]
    public void TestReplaceDependentsFromNonExistantDependee()
    {
        DependencyGraph dg = new();
        dg.AddDependency("X1", "Y1");
        dg.ReplaceDependents("A1", new HashSet<string> { "B1", "C1" });
        Assert.AreEqual(3, dg.Size);
        Assert.IsTrue(new HashSet<string> { "B1", "C1" }.SetEquals(dg.GetDependents("A1")));
        Assert.IsTrue(new HashSet<string> { "Y1" }.SetEquals(dg.GetDependents("X1")));
    }

    /// <summary>
    /// Testing for an extreme corner case where a dependency is created, thus creating a key for the dependee, 
    /// and then replaceDependents is called with an empty list, thus making HasDependents return false even though the key exists. 
    /// </summary>
    [TestMethod]
    public void TestReplaceDependentsHasDependentsEmptyList()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A1", "Y1");
        dg.AddDependency("A1", "354");
        dg.ReplaceDependents("A1", new HashSet<string> {});
        Assert.IsFalse(dg.HasDependents("A1"));
    }

    /// <summary>
    /// Replacing dependees of a non-existent dependent should not crash, and in fact simply create the new dependencies. 
    /// Also ensuring that existing uninvolved dependencies are not changed.
    /// </summary>
    [TestMethod]
    public void TestReplaceDependeeFromNonExistantDependent()
    {
        DependencyGraph dg = new();
        dg.AddDependency("X1", "Y1");
        dg.ReplaceDependees("A1", new HashSet<string> { "B1", "C1" });
        Assert.AreEqual(3, dg.Size);
        Assert.IsTrue(new HashSet<string> { "B1", "C1" }.SetEquals(dg.GetDependees("A1")));
        Assert.IsTrue(new HashSet<string> { "Y1" }.SetEquals(dg.GetDependents("X1")));
    }
    /// <summary>
    /// Replacing dependees of an empty dependency graph should not crash, and in fact simply create the new dependencies. 
    /// </summary>
    [TestMethod]
    public void TestReplaceDependeeFromEmpty()
    {
        DependencyGraph dg = new();
        dg.ReplaceDependees("Empty", new HashSet<string> { "not", "empty" });
        Assert.IsTrue(new HashSet<string> { "not", "empty" }.SetEquals(dg.GetDependees("Empty")));
    }

    /// <summary>
    /// Testing for an extreme corner case where a dependency is created, thus creating a key for the dependent, 
    /// and then replaceDependees is called with an empty list, thus making HasDependees return false even though the key exists. 
    /// </summary>
    [TestMethod]
    public void TestReplaceDependeeHasDependeesEmptyList()
    {
        DependencyGraph dg = new();
        dg.AddDependency("F2", "Child");
        dg.AddDependency("AC1", "Child");
        dg.ReplaceDependees("Child", new HashSet<string> {});
        Assert.IsFalse(dg.HasDependees("Child"));
    }

}
