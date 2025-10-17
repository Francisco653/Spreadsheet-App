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
        hasDependents.SetContentsOfCell("b33", "= a1 +10");
        hasDependents.SetContentsOfCell("c45", "= b33 + 10");
        var dependencyList = hasDependents.SetContentsOfCell("a1", " = a9 - j12 / BAD88 ");
        List<string> expectedList = new();

        // Variable names should be automatically capitalized.
        expectedList.Add("A1");
        expectedList.Add("B33");
        expectedList.Add("C45");
        CollectionAssert.AreEquivalent(expectedList, dependencyList.ToList());
    }

    /// <summary>
    /// This tests makes sure that SetContentsOfCell returns a correct list of updated cells when old dependents are replaced. Order doesn't matter.
    /// NOTE: This behavior is defined ONLY for a formula argument, not double or string.
    /// </summary>
    [TestMethod]
    public void Test_Set_Return_New_Dependees()
    {
        Spreadsheet hasDependents = new();
        var dependencyList = hasDependents.SetContentsOfCell("a1", "= a9 - j12 / BAD88 ");
        List<string> expectedList = new();
        dependencyList = hasDependents.SetContentsOfCell("a1", "= b9");

        // Variable names should be automatically capitalized.
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
        hasDependents.SetContentsOfCell("f12", "= a1 * 123467 +b1");
        var dependencyList = hasDependents.SetContentsOfCell("a1", "= aAAs9 - j12 / L89 ");
        List<string> expectedList = new();
        hasDependents.SetContentsOfCell("f12", "98");
        dependencyList = hasDependents.SetContentsOfCell("a1", "= aAAs9 - j12 / L89 ");
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
        expectedList.Add("A1");
        expectedList.Add("B1");
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

    /// <summary>
    /// This makes sure that the spreadsheet is properly marked as changed when a cell is updated.
    /// </summary>
    [TestMethod]
    public void Test_Set_Changed_True()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell("m10", "hello");
        Assert.IsTrue(spreadsheet.Changed);
    }

    /// <summary>
    /// This makes sure that we get 0 from an unimplemented cell with spreadsheet using indexer syntax.
    /// </summary>
    [TestMethod]
    public void Test_IndexerSyntax_Get_Empty()
    {
        Spreadsheet spreadsheet = new();
        object test = spreadsheet[ "C1" ];
        Assert.AreEqual(0D, test);
    }

    /// <summary>
    /// This makes sure that we can get values from spreadsheet using indexer syntax.
    /// </summary>
    [TestMethod]
    public void Test_IndexerSyntax_Get()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell("C1", "45");
        object test = spreadsheet[ "C1" ];
        Assert.AreEqual("45", test);
    }

    /// <summary>
    /// This ensures that Indexer syntax also throws an invalid name exception (same as GetCellValue would).
    /// </summary>
    [TestMethod]
    public void Test_IndexerSyntax_Throws_InvalidNameException()
    {
        Spreadsheet spreadsheet = new();
        Assert.ThrowsExactly<InvalidNameException>(() => _ = spreadsheet["1782"]);
    }

    /// <summary>
    /// This makes sure that we can get values from spreadsheet using indexer syntax.
    /// </summary>
    [TestMethod]
    public void Test_IndexerSyntax_Set()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet[ "f6" ] = "10";
        Assert.AreEqual(10D, spreadsheet.GetCellValue("F6"));
    }

    /// <summary>
    /// This tests ensures that a SpreadsheetReadWriteException is thrown if saving to a filepath that is invalid.
    /// </summary>
    [TestMethod]
    public void Test_Save_SpreadsheetReadWriteException()
    {
        Spreadsheet spreadsheet = new();
        string invalid = "Z://whole lot of nonsense?!?!.txt";
        Assert.ThrowsExactly<SpreadsheetReadWriteException>(() => spreadsheet.Save(invalid));
    }

    /// <summary>
    /// This tests ensures that a a proper Json file is saved for a spreadsheet object.
    /// </summary>
    [TestMethod]
    public void Test_Save_ProperFile()
    {
        Spreadsheet spreadsheetSaved = new("save");
        string valid = "valid_file.txt";
        spreadsheetSaved.SetContentsOfCell("A3", "= 95");
        string properJson = """
             {
              "Cells": {
                "A3": {
                  "StringForm": "= 95"
                }
              }
            }
            """;
        spreadsheetSaved.Save(valid);
        Assert.AreEqual(properJson, File.ReadAllText("valid_file.txt"));
    }

    /// <summary>
    /// This tests ensures that immediately after saving, the spreadsheet is marked as unchanged.
    /// </summary>
    [TestMethod]
    public void Test_Save_Changed_False()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell("y7", "85.01");
        spreadsheet.Save("test.txt");
        Assert.IsFalse(spreadsheet.Changed);
    }

    /// <summary>
    /// This ensures that a SpreadsheetReadWriteException if trying to load from an invalid point.
    /// </summary>
    [TestMethod]
    public void Test_Load_SpreadsheetReadWriteException()
    {
        Spreadsheet spreadsheet = new();
        Assert.ThrowsExactly<SpreadsheetReadWriteException>(() => spreadsheet.Load("."));
    }

    /// <summary>
    /// This test is making sure that a proper json file is loaded in and creats cells appropriately.
    /// </summary>
    [TestMethod]
    public void Test_Load_ProperFile_Changed_False()
    {
        Spreadsheet spreadsheetLoad = new();
#pragma warning disable SA1118 // JSON Files do span multiple lines
        File.WriteAllText("Loadtest.txt", /*lang=json,strict*/ """
             {
              "Cells": {
                "A2": {
                  "StringForm": "100"
                }
              }
            }
            """);
#pragma warning restore SA1118 // Parameter should not span multiple lines'
        spreadsheetLoad.Load("Loadtest.txt");
        Assert.AreEqual("100", spreadsheetLoad.GetCellContents("A2"));
        Assert.AreEqual(100D, spreadsheetLoad.GetCellValue("A2"));
        Assert.IsFalse(spreadsheetLoad.Changed);
    }

    // -- END OF BLACK BOX TESTS FOR ASSIGNMENT 6

    /// <summary>
    /// This test makes sure that getValue works properly when a cell is defined and another cell depends upon it.
    /// </summary>
    [TestMethod]
    public void Test_GetValue_Variables_Defined()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell("B2", "= 10 +5");
        spreadsheet.SetContentsOfCell("A1", "= 10 + b2");
        Assert.AreEqual(25D, spreadsheet.GetCellValue("A1"));
    }

    /// <summary>
    /// This test makes sure that getValue works properly when a cell is defined and another cell depends upon it.
    /// </summary>
    [TestMethod]
    public void Test_GetValue_Variables_Undefined()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell("A1", "= 10 + b2");
        Assert.IsTrue(spreadsheet.GetCellValue("A1") is FormulaError);
    }

    /// <summary>
    /// This test makes sure that getValue works properly when a cell is defined and another cell depends upon it.
    /// </summary>
    [TestMethod]
    public void Test_GetValue_Multiple_Formulas()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell("B2", "= 10 +5");
        spreadsheet.SetContentsOfCell("A1", "= 10 + b2");
        spreadsheet.SetContentsOfCell("C5", "= 4* A1");
        Assert.AreEqual(100D, spreadsheet.GetCellValue("c5"));
    }
}
