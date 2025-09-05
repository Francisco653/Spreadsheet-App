namespace CS3500.Formula;

/// <summary>
///  This class represents a mathematical formula with particular syntax.
/// </summary>
public class Formula
{
    private string formulastring;
    private Array tokenList;
    /// <summary>
    /// This conmstructor checks the syntax validity of a string.
    /// It will throw an exception if the syntax is invalid.
    /// </summary>
    /// <param name="formula"></param>
    public Formula(string formula)
    {
        formulastring = formula;
        if (verification())
        {
            return true;
        }

        else
        {
            throw new ArgumentException("Invalid formula syntax");
        }
        

    /// <summary>
    /// This method checks the syntax validity of a the given formula string.
    /// </summary>
    /// <returns> Returns true if the formula string is valid. </returns>
    private bool verification()
    {
        return true;
    }
} 
