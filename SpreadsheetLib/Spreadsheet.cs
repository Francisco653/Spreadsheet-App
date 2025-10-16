// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;
using DG = CS3500.DependencyGraph.DependencyGraph;
using FormulaType = CS3500.Formulas.Formula;

namespace CS3500.Spreadsheets;

/// <summary>
///   <para>
///     Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name
public class CircularException : Exception
#pragma warning restore SA1649 // File name should match first type name
#pragma warning restore SA1402 // File may only contain a single type
{
}

/// <summary>
///   <para>
///     Thrown to indicate that a name parameter was invalid. Lowercase variables are auto-capitalized.
///   </para>
/// </summary>
#pragma warning disable SA1402 // File may only contain a single type
public class InvalidNameException : Exception
#pragma warning restore SA1402 // File may only contain a single type
{
}

/// <summary>
/// <para>
///   Thrown to indicate that a read or write attempt has failed with
///   an expected error message informing the user of what went wrong.
/// </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpreadsheetReadWriteException"/> class.
    ///   <para>
    ///     Creates the exception with a message defining what went wrong.
    ///   </para>
    /// </summary>
    /// <param name="msg"> An informative message to the user. </param>
    public SpreadsheetReadWriteException(string msg)
    : base(msg)
    {
    }
}

/// <summary>
///   <para>
///     A Spreadsheet object represents the state of a simple spreadsheet.  A
///     spreadsheet object consists of a (theoretically) infinite number of named cells
///     that contain "data" (see below).
///   </para>
///   <para>
///     Cell Names Definition: A valid cell name is defined as:
///   </para>
///   <list type="number">
///      <item> starts with one or more characters</item>
///      <item> completes with one or more numbers (0-9)</item>
///   </list>
///   <para>
///     Examples: "A0", "A1", "A01", "ABC123".
///   </para>
///   <para>
///     Capitalization: Cell names are case insensitive so "a1" and "A1" are the same.  To the "outside world"
///     capital letters should be shown. (i.e., User types in "=a1", the spreadsheet would show "=A1".
///   </para>
///   <para>
///     Cells: A spreadsheet must be able to return the content of any request cell.  (This
///     means that a spreadsheet represents an infinite number of "implied" cells.)  Cells should thus have
///     a name, a contents (and eventually a value).  The distinction between content and value is important.
///   </para>
///   <para>
///     By analogy, the contents of a cell in Excel is what is displayed on the editing line when the cell is selected.
///     The value is what is shown in the spreadsheet grid.
///   </para>
///   <para>
///     Cell Contents: The contents of a cell can be one of:
///   </para>
///   <list type="number">
///     <item> A string </item>
///     <item> A double </item>
///     <item> A Formula object</item>
///   </list>
///   <para>
///     Empty Cells: A cell that has not been assigned a value is considered an Empty cell and should contain
///     the empty string.  For a new spreadsheet, every cell is empty.  Hint: because there are an infinite
///     number of cells, the "empty" string is IMPLIED, not stored.
///   </para>
///   <para>
///     Cell Value: The value of a cell can be one of (1) a string, (2) a double, or (3) a FormulaError.
///     For this assignment, the value of a cell is not computed/stored/etc.
///   </para>
///   <para>
///     Circular Dependencies:  Spreadsheets are never allowed to contain a combination of Formulas that
///     establish a circular dependency.  A circular dependency exists when a cell depends on itself.
///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
///     A1 depends on B1, which depends on C1, which depends on A1.  If the user attempts to create a circular
///     dependency, the contents of the spreadsheet remain unchanged (i.e., stay the same as before the action).
///   </para>
/// </summary>
public class Spreadsheet
{
    // Needed to copy this from formula class as all the methods that check if variables are valid are private within the class.
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    // All variable names will be uppercase for sake of normality
    private Dictionary<string, object> cellDictionary = new();
    private DG cellDependencies = new();
    private string name;

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// If a name isn't provided, we use name "default".
    /// </summary>
    public Spreadsheet()
    {
        // TODO: Implement Default Constructor
        name = "default";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// </summary>
    /// <param name="name"> Name of the cell.</param>
    public Spreadsheet(string name)
    {
        // TODO: Implement string constructor
        this.name = name;
    }

    /// <summary>
    /// Gets a value indicating whether the spreadsheet has been changed before saving or loading.
    /// </summary>
    public bool Changed { get; private set; }

    /// <summary>
    ///   <para>
    ///     Shortcut syntax to for getting the value of the cell
    ///     using the [] operator.
    ///   </para>
    ///   <para>
    ///     See: <see cref="GetCellValue(string)"/>.
    ///   </para>
    ///   <para>
    ///     Example Usage:
    ///   </para>
    ///   <code>
    ///      sheet.SetContentsOfCell( "A1", "=5+5" );
    ///
    ///      sheet["A1"] == 10;
    ///      // vs.
    ///      sheet.GetCellValue("A1") == 10;
    ///   </code>
    /// </summary>
    /// <param name="cellName"> Any valid cell name. </param>
    /// <returns>
    ///   Returns the value of a cell.  Note: If the cell is a formula, the value should
    ///   already have been computed.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If the name parameter is invalid, throw an InvalidNameException.
    /// </exception>
    public object this[ string cellName ]
    {
        // TODO: Implement Indexer Syntax Override
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///   <para>
    ///     Writes the contents of this spreadsheet to the named file using a JSON format.
    ///     If the file already exists, overwrite it.
    ///   </para>
    ///   <para>
    ///     The output JSON should look like the following.
    ///   </para>
    ///   <para>
    ///     For example, consider a spreadsheet that contains a cell "A1"
    ///     with contents being the double 5.0, and a cell "B3" with contents
    ///     being the Formula("A1+2"), and a cell "C4" with the contents "hello".
    ///   </para>
    ///   <para>
    ///      This method would produce the following JSON string:
    ///   </para>
    ///   <code>
    ///   {
    ///     "Cells": {
    ///       "A1": {
    ///         "StringForm": "5"
    ///       },
    ///       "B3": {
    ///         "StringForm": "=A1+2"
    ///       },
    ///       "C4": {
    ///         "StringForm": "hello"
    ///       }
    ///     }
    ///   }
    ///   </code>
    ///   <para>
    ///     You can achieve this by making sure your data structure is a dictionary
    ///     and that the contained objects (Cells) have property named "StringForm"
    ///     (if this name does not match your existing code, use the JsonPropertyName
    ///     attribute).
    ///   </para>
    ///   <para>
    ///     There can be 0 cells in the dictionary, resulting in { "Cells" : {} }.
    ///   </para>
    ///   <para>
    ///     Further, when writing the value of each cell...
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       If the contents is a string, the value of StringForm is that string
    ///     </item>
    ///     <item>
    ///       If the contents is a double d, the value of StringForm is d.ToString()
    ///     </item>
    ///     <item>
    ///       If the contents is a Formula f, the value of StringForm is "=" + f.ToString()
    ///     </item>
    ///   </list>
    ///   <para>
    ///     After saving the file, the spreadsheet is no longer "changed".
    ///   </para>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file,
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save(string filename)
    {
        // TODO: Implement per documentation
        throw new NotImplementedException();
    }

    /// <summary>
    ///   <para>
    ///     Read the data (JSON) from the file and instantiate the current
    ///     spreadsheet.  See <see cref="Save(string)"/> for expected format.
    ///   </para>
    ///   <para>
    ///     Note: First deletes any current data in the spreadsheet.
    ///   </para>
    ///   <para>
    ///     Loading a spreadsheet should set changed to false.  External
    ///     programs should alert the user before loading over a changed sheet.
    ///   </para>
    /// </summary>
    /// <param name="filename"> The saved file name including the path. </param>
    /// <exception cref="SpreadsheetReadWriteException"> When the file cannot be opened or the json is bad.</exception>
    public void Load(string filename)
    {
        // TODO: Implement per documentation
        throw new NotImplementedException();
    }

    /// <summary>
    ///   Provides a copy of the names of all of the cells in the spreadsheet
    ///   that contain information (i.e., not empty cells).
    /// </summary>
    /// <returns>
    ///   A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        return cellDictionary.Keys.ToHashSet();
    }

    /// <summary>
    ///   Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    ///   The contents as either a string, a double, or a Formula.
    ///   See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        // Check for invalid name
        CheckName(name);

        // By default return an empty string for an undefined cell.
        if (! cellDictionary.ContainsKey(name.ToUpper()))
        {
            return string.Empty;
        }

        return cellDictionary[name.ToUpper()];
    }

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="cellName"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value's type should be either a string, a double, or a CS3500.Formula.FormulaError.
    ///   If the cell contents are a formula, the value should have already been computed
    ///   at this point.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue(string cellName)
    {
        // TODO: Implement GetCellValue per documentation
        throw new NotImplementedException();
    }

    /// <summary>
    ///   <para>
    ///       Sets the contents of the named cell to the appropriate object
    ///       based on the string in <paramref name="content"/>.
    ///   </para>
    ///   <para>
    ///       First, if the <paramref name="content"/> parses as a double, the contents of the named
    ///       cell becomes that double.
    ///   </para>
    ///   <para>
    ///       Otherwise, if the <paramref name="content"/> begins with the character '=', an attempt is made
    ///       to parse the remainder of content into a Formula.
    ///   </para>
    ///   <para>
    ///       There are then three possible outcomes when a formula is detected:
    ///   </para>
    ///
    ///   <list type="number">
    ///     <item>
    ///       If the remainder of content cannot be parsed into a Formula, a
    ///       FormulaFormatException is thrown.
    ///     </item>
    ///     <item>
    ///       If changing the contents of the named cell to be f
    ///       would cause a circular dependency, a CircularException is thrown,
    ///       and no change is made to the spreadsheet.
    ///     </item>
    ///     <item>
    ///       Otherwise, the contents of the named cell becomes f.
    ///     </item>
    ///   </list>
    ///   <para>
    ///     Finally, if the content is a string that is not a double and does not
    ///     begin with an "=" (equal sign), save the content as a string.
    ///   </para>
    ///   <para>
    ///     On successfully changing the contents of a cell, the spreadsheet will be <see cref="Changed"/>.
    ///   </para>
    /// </summary>
    /// <param name="cellName"> The cell name that is being changed.</param>
    /// <param name="content"> The new content of the cell.</param>
    /// <returns>
    ///   <para>
    ///     This method returns a list of all cells that have been updated
    ///     (of course including the cell that was just updated).
    ///   </para>
    ///   <para>
    ///     For example, if cellName is "A1", and B1 contains A1*2, and C1 contains B1+A1, the
    ///     list containing [A1, B1, C1] is returned.
    ///   </para>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the cellName parameter contains an invalid name, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   If changing the contents of the named cell to be the formula would
    ///   cause a circular dependency, throw a CircularException.
    ///   (NOTE: No change is made to the spreadsheet.)
    /// </exception>
    public IList<string> SetContentsOfCell(string cellName, string content)
    {
        // TODO: Implement per documentation
        throw new NotImplementedException();
    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar(string token)
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    /// This private helper calls IsVar to check variable name validity, and throws an error if invalid.
    /// </summary>
    /// <param name="name"> The variable name given.
    /// </param>
    /// <exception cref="InvalidNameException"> If the name is invalid, throw an InvalidNameException.
    /// </exception>
    private static void CheckName(string name)
    {
        if (!IsVar(name))
        {
            throw new InvalidNameException();
        }
    }

    /// <summary>
    ///  Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new content of the cell. </param>
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other cells whose value depends, directly
    ///     or indirectly, on the named cell.
    ///   </para>
    ///   <para>
    ///     The order must correspond to a valid dependency ordering for recomputing
    ///     all of the cells, i.e., if you re-evaluate each cell in the order of the list,
    ///     the overall spreadsheet will be correctly updated.
    ///   </para>
    ///   <para>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
    ///   </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        // We check if the given variable name is valid.
        CheckName(name);

        // Since the cell being set is not a formula now, we need to remove any potential dependencies that may exist.
        cellDependencies.ReplaceDependents(name.ToUpper(), []);
        return UpdateCell(name.ToUpper(), number);
    }

    /// <summary>
    ///   The contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {
        // We check if the given variable name is valid.
        CheckName(name);

        // Since the cell being set is not a formula now, we need to remove any potential dependencies that may exist.
        cellDependencies.ReplaceDependents(name.ToUpper(), []);
        return UpdateCell(name.ToUpper(), text);
    }

    /// <summary>
    ///   Set the contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException.
    ///   </para>
    ///   <para>
    ///     No change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, FormulaType formula)
    {
        // We check if the given variable name is valid.
        CheckName(name);

        // Here we check for other variables in formula. If we find any, then we need to update our dependencies.
        var dependents = formula.GetVariables();
        cellDependencies.ReplaceDependents(name.ToUpper(), dependents);

        // Checks for circular exceptions
        GetCellsToRecalculate(name.ToUpper());

        // Now we can update cell as per usual.
        return UpdateCell(name.ToUpper(), formula);
    }

    /// <summary>
    /// This private helper takes a variable name and either updates it in cellDictionary or creates a new key for the newly defined variable.
    /// These cell keys are then given an object value (double, string, or formula) to hold.
    /// </summary>
    /// <param name="name"> The name of the cell to be made or updated.
    /// </param>
    /// <param name="value"> The vakue for the cell (key) to hold.
    /// </param>
    /// <returns> Returns a list of all cells dependent on the cell just updated.
    /// </returns>
    private IList<string> UpdateCell(string name, object value)
    {
        // Need to account for the cell being set to an empty string, which effectively removes that cell.
        if (value is string && (string) value == string.Empty )
        {
            cellDictionary.Remove(name);
            return [];
        }

        if (cellDictionary.ContainsKey(name))
        {
            cellDictionary[name] = value;
            return GetDirectDependents(name).ToList();
        }
        else
        {
            cellDictionary.Add(name, value);
            return GetDirectDependents(name).ToList();
        }
    }

    /// <summary>
    ///   Returns an enumeration, without duplicates, of the names of all cells whose
    ///   values depend directly on the value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all cells
    ///     that contain formulas containing name.
    ///   </para>
    ///   <para>For example, suppose that: </para>
    ///   <list type="bullet">
    ///      <item>A1 contains 3</item>
    ///      <item>B1 contains the formula A1 * A1</item>
    ///      <item>C1 contains the formula B1 + A1</item>
    ///      <item>D1 contains the formula B1 - C1</item>
    ///   </list>
    ///   <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        return cellDependencies.GetDependents(name.ToUpper());
    }

    /// <summary>
    ///   <para>
    ///     This method is implemented for you, but makes use of your GetDirectDependents.
    ///   </para>
    ///   <para>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated, assuming that the contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    ///   <para>
    ///     For example, suppose that:
    ///   </para>
    ///   <list type="number">
    ///     <item>
    ///       A1 contains 5
    ///     </item>
    ///     <item>
    ///       B1 contains the formula A1 + 2.
    ///     </item>
    ///     <item>
    ///       C1 contains the formula A1 + B1.
    ///     </item>
    ///     <item>
    ///       D1 contains the formula A1 * 7.
    ///     </item>
    ///     <item>
    ///       E1 contains 15
    ///     </item>
    ///   </list>
    ///   <para>
    ///     If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    ///     and they must be recalculated in an order which has A1 first, and B1 before C1
    ///     (there are multiple such valid orders).
    ///     The method will produce one of those enumerations.
    ///   </para>
    ///   <para>
    ///      PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    ///      IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    ///   </para>
    /// </summary>
    /// <param name="name"> The name of the cell.  Requires that name be a valid cell name.</param>
    /// <returns>
    ///    Returns an enumeration of the names of all cells whose values must
    ///    be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        LinkedList<string> changed = new();
        HashSet<string> visited = [];
        Visit(name, name, visited, changed);
        return changed;
    }

    /// <summary>
    ///   A helper for the GetCellsToRecalculate method.
    ///   <para>
    ///   This method takes a starting point, and a current point to examine. It adds the current point to a list of visited then iterates through each dependent of the current point.
    ///   If at any moment the dependents inside current point lead back to the start point, a circular error exception is thrown.
    ///   Otherwise, the current point is added to the list of changed values.
    ///   Every non-visited point gets its own recursion of the visit method, repeating the mentioned steps until all points are contained in the visited Set.
    ///   </para>
    ///   <returns>
    ///   Returns a list of all points that need to be recalculated due to a change in their dependee.
    ///   </returns>
    ///   <exception cref="CircularException">
    ///   Throws a CircularException if a circular dependency is found.
    ///   </exception>
    /// </summary>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        visited.Add(name);
        foreach (string dependent in GetDirectDependents(name))
        {
            if (dependent.Equals(start))
            {
                throw new CircularException();
            }
            else if (!visited.Contains(dependent))
            {
                Visit(start, dependent, visited, changed);
            }
        }

        changed.AddFirst(name);
    }
}
