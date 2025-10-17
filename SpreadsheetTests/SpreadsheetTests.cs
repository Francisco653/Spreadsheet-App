// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>

using CS3500.Formulas;
using CS3500.Spreadsheets;

/// <summary>
/// Author:    Francisco Pinas
/// Partner:   N/A
/// Date:      09/26/2025
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
///    This file is the SpreadSheetTests Class, used to test the Spreadsheet class.
///    This class ensures that the Formula class is properly creating tokens,
///    and enforcing the rules for how a formula should be structured.
///    It now also contains tests for new FormulaClass functionality, ensuring that formulas are properly evaluated.
/// </summary>
namespace SpreadsheetTests;

/// <summary>
/// Author:    Francisco Pinas
/// Partner:   N/A
/// Date:      09/26/2025
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
///    This file is the SpreadSheetTests Class, used to test the Spreadsheet class.
///    This class ensures that the Formula class is properly creating tokens,
///    and enforcing the rules for how a formula should be structured.
///    It now also contains tests for new FormulaClass functionality, ensuring that formulas are properly evaluated.
/// </summary>
[TestClass]
public sealed class SpreadsheetTests
{
    // --- START OF BLACK BOX PRE-TESTS ---

    /// <summary>
    /// This tests makes sure that GetNames doesn't return Empty cells.
    /// </summary>
    [TestMethod]
    public void Test_GetNames_Empty()
    {
        Spreadsheet empty = new();
        Assert.AreEqual(0, empty.GetNamesOfAllNonemptyCells().Count());
    }

    /// <summary>
    /// This tests makes sure that GetNames returns the cell that has a value attached properly.
    /// </summary>
    [TestMethod]
    public void Test_GetName_OneCell()
    {
        Spreadsheet oneCell = new();
        oneCell.SetContentsOfCell("A1", "10");
        Assert.AreEqual("A1", oneCell.GetNamesOfAllNonemptyCells().First());
    }

    /// <summary>
    /// This tests makes sure that GetNames correctly handles lowercases and capitalizes them.
    /// </summary>
    [TestMethod]
    public void Test_GetName_OneCell_LowerCase()
    {
        Spreadsheet oneCell = new();
        oneCell.SetContentsOfCell("a1", "10");
        Assert.AreEqual("A1", oneCell.GetNamesOfAllNonemptyCells().First());
    }

    /// <summary>
    /// This tests makes sure that GetNames returns a list of cells that have values attached properly.
    /// API provided doesn't mention if the order returned matters, so I am assuming it doesn't (that is why AreEquivalent() is used).
    /// </summary>
    [TestMethod]
    public void Test_GetName_MultipleCells()
    {
        Spreadsheet multiCell = new();
        multiCell.SetContentsOfCell("A1", "10");
        multiCell.SetContentsOfCell("A2", "30");
        HashSet<string> names = ["A1", "A2"];
        CollectionAssert.AreEquivalent(names.ToList(), multiCell.GetNamesOfAllNonemptyCells().ToList());
    }

    /// <summary>
    /// This tests makes sure that GetNames returns nothing if a cell is set and then set again to hold an empty string.
    /// </summary>
    [TestMethod]
    public void Test_GetName_Removed()
    {
        Spreadsheet empty = new();
        empty.SetContentsOfCell("A1", "10");
        empty.SetContentsOfCell("A1", string.Empty);
        Assert.AreEqual(0, empty.GetNamesOfAllNonemptyCells().Count());
    }

    /// <summary>
    /// This tests makes sure that GetCellContents throws an InvalidNameException when given an invalid variable name to get.
    /// </summary>
    [TestMethod]
    public void Test_GetCellContents_InvalidVariable()
    {
        Spreadsheet empty = new();
        Assert.ThrowsExactly<InvalidNameException>(() => _ = empty.GetCellContents("1809ajsj"));
    }

    /// <summary>
    /// This tests makes sure that GetCellContent returns an empty string for an empty cell.
    /// </summary>
    [TestMethod]
    public void Test_GetCellContents_Empty()
    {
        Spreadsheet empty = new();
        Assert.AreEqual(string.Empty, empty.GetCellContents("A1"));
    }

    /// <summary>
    /// This tests makes sure that GetCellContent and SetCellContent works appropriately for doubles, including a lowercase variable.
    /// </summary>
    [TestMethod]
    public void Test_SetAndGet_Doubles()
    {
        Spreadsheet doubleSheet = new();
        doubleSheet.SetContentsOfCell("A1", "10");
        doubleSheet.SetContentsOfCell("a7", ".25");
        Assert.AreEqual(10D, doubleSheet.GetCellContents("A1"));
        Assert.AreEqual(.25, doubleSheet.GetCellContents("A7"));
    }

    /// <summary>
    /// This tests makes sure that GetCellContent and SetCellContent works appropriately for strings, including a lowercase variable.
    /// </summary>
    [TestMethod]
    public void Test_SetAndGet_Strings()
    {
        Spreadsheet doubleSheet = new();
        doubleSheet.SetContentsOfCell("A1", "32 + 19");
        doubleSheet.SetContentsOfCell("c6", "10 / (5 - 0 * 0)");
        Assert.AreEqual("32 + 19", doubleSheet.GetCellContents("a1"));
        Assert.AreEqual("10 / (5 - 0 * 0)", doubleSheet.GetCellContents("C6"));
    }

    /// <summary>
    /// This tests makes sure that GetCellContent and SetCellContent works appropriately for Formula Objects, including lowercase variables.
    /// This test compares using the canonical form of the formula provided by the Formula class in the ToString() method.
    /// </summary>
    [TestMethod]
    public void Test_SetAndGet_Formula()
    {
        Spreadsheet doubleSheet = new();
        doubleSheet.SetContentsOfCell("a1", "= 9 - 8");
        doubleSheet.SetContentsOfCell("F12", "= 5");
        Assert.AreEqual("9-8", doubleSheet.GetCellContents("a1").ToString());
        Assert.AreEqual("5", doubleSheet.GetCellContents("F12").ToString());
    }

    /// <summary>
    /// This tests makes sure that SetContentsOfCell throws an InvalidNameException when given a formula for an invalid cell.
    /// </summary>
    [TestMethod]
    public void Test_Set_Formula_InvalidVariable()
    {
        Spreadsheet doubleSheet = new();
        doubleSheet.SetContentsOfCell("a1", "=9 - 8");
        doubleSheet.SetContentsOfCell("F12", "= 5");
        Assert.ThrowsExactly<InvalidNameException>(() => _ = doubleSheet.SetContentsOfCell("not a variable!!!!", "=0"));
    }

    /// <summary>
    /// This tests makes sure that SetContentsOfCell throws an InvalidNameException when given a string for an invalid cell.
    /// </summary>
    [TestMethod]
    public void Test_Set_String_InvalidVariable()
    {
        Spreadsheet doubleSheet = new();
        doubleSheet.SetContentsOfCell("a1", "9 - 8");
        doubleSheet.SetContentsOfCell("F12", "ligma");
        Assert.ThrowsExactly<InvalidNameException>(() => _ = doubleSheet.SetContentsOfCell(string.Empty, "67"));
    }

    /// <summary>
    /// This tests makes sure that SetContentsOfCell throws an InvalidNameException when given a double for an invalid cell.
    /// </summary>
    [TestMethod]
    public void Test_Set_Double_InvalidVariable()
    {
        Spreadsheet doubleSheet = new();
        doubleSheet.SetContentsOfCell("a1", "= 9 - 8");
        doubleSheet.SetContentsOfCell("F12", "=5");
        Assert.ThrowsExactly<InvalidNameException>(() => _ = doubleSheet.SetContentsOfCell("45a", "271"));
    }

    /// <summary>
    /// This tests makes sure that a CircularException is thrown when a cell points to itself DIRECTLY.
    /// NOTE: This behavior is defined ONLY for formula, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Circular_Direct()
    {
        Spreadsheet selfReference = new();
        Assert.ThrowsExactly<CircularException>(() => _ = selfReference.SetContentsOfCell("A1", "= a1 * 2"));
    }

    /// <summary>
    /// This tests makes sure that a CircularException is thrown when a cell points to itself INDIRECTLY.
    /// NOTE: This behavior is defined ONLY for a formula argument, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Circular_Indirect()
    {
        Spreadsheet selfReference = new();
        selfReference.SetContentsOfCell("B1", "=      a1 - 5");
        Assert.ThrowsExactly<CircularException>(() => _ = selfReference.SetContentsOfCell("A1", " =    b1 * 2"));
    }

    // --- END OF BLACK BOX PRE-TESTS

    /// <summary>
    /// This tests makes sure that SetContentsOfCell returns only the changed cell itself when it has no dependents.
    /// NOTE: This behavior is defined ONLY for a formula argument, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Set_Return_NoDependents()
    {
        Spreadsheet noDependents = new();
        var dependencyList = noDependents.SetContentsOfCell("a1", "= 9 - 8");
        List<string> single = new();
        single.Add("A1");
        CollectionAssert.AreEquivalent(single, dependencyList.ToList());
    }

    /// <summary>
    /// This tests makes sure that SetContentsOfCell returns a correct list of updated cells (itself and dependents). Order doesn't matter.
    /// NOTE: This behavior is defined ONLY for a formula argument, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Set_Return_Dependents()
    {
        Spreadsheet hasDependents = new();
        var dependencyList = hasDependents.SetContentsOfCell("a1", " = a9 - j12 / BAD88 ");
        List<string> expectedList = new();

        // Variable names should be automatically capitalized.
        expectedList.Add("A9");
        expectedList.Add("J12");
        expectedList.Add("BAD88");
        expectedList.Add("A1");
        CollectionAssert.AreEquivalent(expectedList, dependencyList.ToList());
    }

    /// <summary>
    /// This tests makes sure that SetContentsOfCell returns a correct list of updated cells when old dependents are replaced. Order doesn't matter.
    /// NOTE: This behavior is defined ONLY for a formula argument, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Set_Return_New_Dependents()
    {
        Spreadsheet hasDependents = new();
        var dependencyList = hasDependents.SetContentsOfCell("a1", "= a9 - j12 / BAD88 ");
        List<string> expectedList = new();
        dependencyList = hasDependents.SetContentsOfCell("a1", "= b9");

        // Variable names should be automatically capitalized.
        expectedList.Add("B9");
        expectedList.Add("A1");
        CollectionAssert.AreEquivalent(expectedList, dependencyList.ToList());
    }

    /// <summary>
    /// This tests makes sure that SetContentsOfCell returns a correct list of updated cells when old dependents are replaced. Order doesn't matter.
    /// NOTE: This behavior is defined ONLY for a formula argument, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Set_Return_Lost_AllDependents()
    {
        Spreadsheet hasDependents = new();
        var dependencyList = hasDependents.SetContentsOfCell("a1", "= aAAs9 - j12 / L89 ");
        List<string> expectedList = new();
        dependencyList = hasDependents.SetContentsOfCell("a1", "98");
        expectedList.Add("A1");
        CollectionAssert.AreEquivalent(expectedList, dependencyList.ToList());
    }

    /// <summary>
    /// This tests makes sure that a CircularException is no longer thrown when a cell is updated to stop having a circular dependency.
    /// NOTE: This behavior is defined ONLY for a formula argument, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Circular_NoMore()
    {
        Spreadsheet selfReference = new();
        selfReference.SetContentsOfCell("B1", "= a1 - 5");
        Assert.ThrowsExactly<CircularException>(() => _ = selfReference.SetContentsOfCell("A1", "= b1 * 2"));
        List<string> expectedList = new();
        CollectionAssert.AreEquivalent(expectedList, selfReference.SetContentsOfCell("A1", "no circular dependencies here!!!").ToList());
    }

    /// <summary>
    /// This tests makes sure that GetCellContent returns an empty string when a cell that used to be defined is set as an empty string.
    /// </summary>
    [TestMethod]
    public void Test_GetCellContents_Removed()
    {
        Spreadsheet empty = new();
        empty.SetContentsOfCell("g3", "65");
        Assert.AreEqual(65D, empty.GetCellContents("G3"));
        empty.SetContentsOfCell("g3", string.Empty);
        Assert.AreEqual(string.Empty, empty.GetCellContents("g3"));
    }

    // Assignment 6 Black Box Testing (TTD)

    /// <summary>
    /// This test makes sure the new Spreadsheet default constructor works without crashing.
    /// </summary>
    [TestMethod]

    public void Test_SpreadsheetConstructor_Default()
    {
        Spreadsheet spreadsheet = new();
    }

    /// <summary>
    /// This test makes sure the new Spreadsheet string name constructor works without crashing.
    /// </summary>
    [TestMethod]

    public void Test_SpreadsheetConstructor_Named()
    {
        Spreadsheet spreadsheet = new("Shouldn't Crash");
    }

    [TestMethod]
    public void Test_IndexerSyntax_Get()
    {
        Spreadsheet spreadsheet = new();
        object test = spreadsheet[ "C1" ];
    }

    [TestMethod]
    public void Test_IndexerSyntax_Throws_InvalidNameException()
    {
        Spreadsheet spreadsheet = new();
        Assert.ThrowsExactly( object test = spreadsheet[ "12C1" ]);
    }
}
