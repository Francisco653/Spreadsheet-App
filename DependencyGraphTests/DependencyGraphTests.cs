// <copyright file="DependencyGraphTests.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>

using DG = CS3500.DependencyGraph.DependencyGraph;

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

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests.
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    // -- BLACK BOX TESTS PRIOR TO IMPLEMENTATION OF DEPENDENCY GRAPH --

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, a new dependency graph is empty when first created.
    /// As such, the size should be zero.
    /// </summary>
    [TestMethod]
    public void TestEmptyBySize()
    {
        DG dg = new();
        Assert.AreEqual(0, dg.Size);
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, a new dependency graph is empty when first created.
    /// As such, HasDependees should always return false.
    /// </summary>
    [TestMethod]
    public void TestEmptyByDependees()
    {
        DG dg = new();
        Assert.IsFalse(dg.HasDependees("A1"));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, a new dependency graph is empty when first created.
    /// As such, HasDependent should always return false.
    /// </summary>
    [TestMethod]
    public void TestEmptyByDependents()
    {
        DG dg = new();
        Assert.IsFalse(dg.HasDependents("A1"));
    }

    /// <summary>
    /// This is a black box test implemented before the DependencyGraph class was implemented. This ensures that by API, when a new dependency graph is created and a dependency is added,
    /// size should be one.
    /// </summary>
    [TestMethod]
    public void TestSizeAdd()
    {
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A1", "C1");
        dg.AddDependency("A1", "A2");
        HashSet<string> replacement = new() { "C5", "D1", "F2" };
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
        DG dg = new();
        dg.AddDependency("A1", "B1");
        dg.AddDependency("A2", "B1");
        dg.AddDependency("C1", "B1");
        HashSet<string> replacement = new() { "Z8", "G13", "F42" };
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
        new DG();
    }

    /// <summary>
    /// This tests that getting dependees of a non-existent node does not cause a crash and returns an empty set.
    /// </summary>
    [TestMethod]
    public void TestGetDependeesNonExistent()
    {
        DG dg = new();
        dg.AddDependency("D1", "B1");
        Assert.IsTrue(new HashSet<string> { }.SetEquals(dg.GetDependees("A1")));
    }

    /// <summary>
    /// This tests that getting dependents of a non-existent node does not cause a crash, and returns an empty set.
    /// </summary>
    [TestMethod]
    public void TestGetDependentsNonExistent()
    {
        DG dg = new();
        dg.AddDependency("B1", "A1");
        Assert.IsTrue(new HashSet<string> { }.SetEquals(dg.GetDependents("z9")));
    }

    /// <summary>
    /// This tests that checking for dependents of a non-existent node does not cause a crash.
    /// </summary>
    [TestMethod]
    public void TestHasDependentsNonExistent()
    {
        DG dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependents("test"));
    }

    /// <summary>
    /// This tests that checking for dependees of a non-existent node does not cause a crash.
    /// </summary>
    [TestMethod]
    public void TestHasDependeesNonExistent()
    {
        DG dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependees("nonexistent"));
    }

    /// <summary>
    /// This tests that when a dependency is added, HasDependees returns true for the dependent.
    /// </summary>
    [TestMethod]
    public void TestHasDependeesTrue()
    {
        DG dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsTrue(dg.HasDependees("B1"));
    }

    /// <summary>
    /// This tests that when a dependency is added, HasDependents returns true for the dependee.
    /// </summary>
    [TestMethod]
    public void TestHasDependentsTrue()
    {
        DG dg = new();
        dg.AddDependency("C1", "88");
        Assert.IsTrue(dg.HasDependents("C1"));
    }

    /// <summary>
    /// This tests that when a dependency is added, the dependee isn't mistakenly marked as having dependees.
    /// </summary>
    [TestMethod]
    public void TestHasDependentsFalse()
    {
        DG dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependents("B1"));
    }

    /// <summary>
    /// This tests that when a dependency is added, the dependent isn't mistakenly marked as having dependents.
    /// </summary>
    [TestMethod]
    public void TestHasDependeesFalse()
    {
        DG dg = new();
        dg.AddDependency("A1", "B1");
        Assert.IsFalse(dg.HasDependees("A1"));
    }

    /// <summary>
    /// This tests that adding a duplicate dependency does not increase the size of the dependency graph.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyDuplicate()
    {
        DG dg = new();
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
        DG dg = new();
        dg.AddDependency("b11", "b11");
        Assert.AreEqual(1, dg.Size);
    }

    /// <summary>
    /// This tests that adding a dependency of one string to itself displays tracks that it has dependees.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfHasDependees()
    {
        DG dg = new();
        dg.AddDependency("b11", "b11");
        Assert.IsTrue(dg.HasDependees("b11"));
    }

    /// <summary>
    /// This tests that adding a dependency of one string to itself displays tracks that it has dependents.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfHasDependents()
    {
        DG dg = new();
        dg.AddDependency("b11", "b11");
        Assert.IsTrue(dg.HasDependents("b11"));
    }

    /// <summary>
    /// This tests that adding a dependency of one string to itself does not break the list of dependents.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyToSelfCorrectDependents()
    {
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
        dg.AddDependency("A1", "B1");
        dg.RemoveDependency("A1", "Chrome");
    }

    /// <summary>
    /// Replacing dependents of an empty dependency graph should not crash, and in fact simply create the new dependencies.
    /// </summary>
    [TestMethod]
    public void TestReplaceDependentsFromEmpty()
    {
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
        dg.AddDependency("A1", "Y1");
        dg.AddDependency("A1", "354");
        dg.ReplaceDependents("A1", new HashSet<string> { });
        Assert.IsFalse(dg.HasDependents("A1"));
    }

    /// <summary>
    /// Replacing dependees of a non-existent dependent should not crash, and in fact simply create the new dependencies.
    /// Also ensuring that existing uninvolved dependencies are not changed.
    /// </summary>
    [TestMethod]
    public void TestReplaceDependeeFromNonExistantDependent()
    {
        DG dg = new();
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
        DG dg = new();
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
        DG dg = new();
        dg.AddDependency("F2", "Child");
        dg.AddDependency("AC1", "Child");
        dg.ReplaceDependees("Child", new HashSet<string> { });
        Assert.IsFalse(dg.HasDependees("Child"));
    }
}
