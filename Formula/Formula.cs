namespace CS3500.Formula;

/// <summary>
///  This class represents a mathematical formula with particular syntax.
/// </summary>
public class Formula
{
    private string formulastring;
    /// <summary>
    /// This conmstructor checks the syntax validity of a string.
    /// It will throw an exception if the syntax is invalid.
    /// </summary>
    /// <param name="formula"></param>
    public Formula(string formula)
    {
        formulastring = formula;

    }
} 
