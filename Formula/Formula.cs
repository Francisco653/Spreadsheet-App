// <copyright file="Formula.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;

namespace CS3500.Formulas;

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.  In general the expected behavior is that
///   the Lookup method will "know" about all variables in a formula
///   and return their appropriate value.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);

/// <summary>
/// Author:    Francisco Pinas
/// Partner:   N/A
/// Date:      09/19/2025
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
///    This file is the Formula class, which is used to represent, validate, and evaluate formula strings, rules down below.
/// <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
/// <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    // List of tokens in the formula
    private List<string> tokenList;

    // This string will spit back the formula in canonical form when ToString is called, it is initiated here to achieve O(1) time complexity.
    private string canonicalForm = string.Empty;

    // This hashset contains all the UNIQUE variables in the formula.
    private HashSet<string> uniqueVariables = new();

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        tokenList = GetTokens(formula);

        // Check each token using the rules from the assignment.
        FormulaIsValid(tokenList);

        // Normalizes the string for ToString method (again to achieve O(1) time complexity).
        canonicalForm = BuildCanonicalForm();
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
    ///   </remarks>
    ///   <para>
    ///     For example, if N is a method that converts all the letters in a string to upper case:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should enumerate "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should enumerate "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        foreach (string token in tokenList)
        {
            if (IsVar(token))
            {
                uniqueVariables.Add(token.ToUpper());
            }
        }

        return uniqueVariables;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All of the variables in the string will be normalized.  This
    ///     means capital letters.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + y1").ToString() should return "X1+Y1"
    ///       new("X1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This code should execute in O(1) time.
    ///   </para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString()
    {
        // canonicalForm is built in the constructor to achieve O(1) time complexity.
        return canonicalForm;
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
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }

    /// <summary>
    /// This method iterates through the list of tokens and checks if they are valid according to the rules specified in the assignment.
    /// </summary>
    /// <param name="formulaTokens"> These are the tokens of this formula (string types) that will be analyzed.</param>
    /// <returns> True if all the tokens (in relation to one another) are valid.</returns>
    private static bool FormulaIsValid(List<string> formulaTokens)
    {
        // This will hold the types of each token, useful for checking some of the formatting rules.
        List<string> types = [];

        // One token minimum rule
        if (formulaTokens.Count == 0)
        {
            throw new FormulaFormatException("The formula cannot be empty!");
        }

        // These ints are used for checking first token, and number of opening/closing parenthesis.
        int count = 0;
        int leftParenthesisCount = 0;
        int rightParenthesisCount = 0;

        // Here we start iterating through the tokens to check their validity.
        foreach (string token in formulaTokens)
        {
            // Check first token rule
            count++;
            if (count == 1 && !IsValidFirst(TokenType(token)))
            {
                throw new FormulaFormatException("First token must be a number, variable, or an open parenthesis!");
            }

            // Checks individual tokens to see if they are valid.
            if (TokenType(token) == "invalid")
            {
                throw new FormulaFormatException("Tokens must be a number, variable, opening/closing parenthesis, or operator!");
            }

            // Adds the token type now that it is verified, and from here more rule checks will occur.
            types.Add(TokenType(token));

            // Counting opening and closing parenthesis (left and right respectively) to check closing parenthesis and later the balanced parenthesis rules.
            if (types.Last() == "leftParenthesis")
            {
                leftParenthesisCount++;
            }

            if (types.Last() == "rightParenthesis")
            {
                rightParenthesisCount++;
                if (rightParenthesisCount > leftParenthesisCount)
                {
                    throw new FormulaFormatException("Closing parenthesis cannot at any point exceed opening parenthesis when read left to right!");
                }
            }

            // Parenthesis following rule checked here.
            if (count >= 2 && (types[types.Count - 2] == "operator" || types[types.Count - 2] == "leftParenthesis"))
            {
                // The conditions for the first token and tokens following an operator or opening parenthesis are the same.
                if (!IsValidFirst(TokenType(token)))
                {
                    throw new FormulaFormatException("A number, variable, or opening parenthesis must follow an operator or opening parenthesis!");
                }
            }

            // Extra following rule (Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.)
            if (count >= 2 && (types[types.Count - 2] == "number" || types[types.Count - 2] == "variable" || types[types.Count - 2] == "rightParenthesis"))
            {
                if (!(types.Last() == "operator" || types.Last() == "rightParenthesis"))
                {
                    throw new FormulaFormatException("An operator or a closing parenthesis must follow a variable, a number, or a closing parenthesis!");
                }
            }
        }

        // Now we check balanced parenthesis rule that ALL tokens have been iterated through.
        if (rightParenthesisCount > leftParenthesisCount || leftParenthesisCount > rightParenthesisCount)
        {
            throw new FormulaFormatException("There must be an equal amount of opening and closed parenthesis!");
        }

        // Now we check the last token rule (Technically, the leftParenthesis condition will never be triggered due to the balanced parenthesis rule, but it is here for clarity).
        if (types.Last() == "operator" || types.Last() == "leftParenthesis")
        {
            throw new FormulaFormatException("Last token must be a number, variable, or a closing parenthesis!");
        }

        return true;
    }

    /// <summary>
    /// Returns a string of the token's type (number, variable, operator, parenthesis, or invalid).
    /// </summary>
    /// <param name="token"> The token is each individual piece of a function.</param>
    /// <returns>Returns the type the token is as a string.</returns>
    private static string TokenType(string token)
    {
        if (Regex.IsMatch(token, @"^(\d+(\.\d*)?|\.\d+)([eE][-+]?\d+)?$"))
        {
            return "number"; // It's a number
        }
        else if (IsVar(token))
        {
            return "variable"; // It's a variable
        }
        else if (Regex.IsMatch(token, @"^[\+\-\*/]$"))
        {
            return "operator"; // It's an operator
        }
        else if (token == "(")
        {
            return "leftParenthesis"; // It's a left parenthesis
        }
        else if (token == ")")
        {
            return "rightParenthesis"; // It's a right parenthesis
        }
        else
        {
            return "invalid"; // It's not a valid token
        }
    }

    /// <summary>
    /// Returns true if the first token is valid (number, variable, or left parenthesis).
    /// </summary>
    /// <param name="tokenType">The type of the token to check for validity as the first token.</param>
    /// <returns>True if valid.</returns>
    private static bool IsValidFirst(string tokenType)
    {
        return tokenType == "number" || tokenType == "variable" || tokenType == "leftParenthesis";
    }

    /// <summary>
    /// This method normalizes the formula string to a canonical form during the construction of the Formula object.
    /// </summary>
    /// <returns> A new normalized formula (whitespace removed and variable letters capitalized.</returns>
    private string BuildCanonicalForm()
    {
        string normalized = string.Empty;
        foreach (string token in tokenList)
        {
            if (IsVar(token))
            {
                normalized += token.ToUpper();
            }
            else
            {
                if (TokenType(token) == "number")
                {
                    double num = double.Parse(token);

                    // This ensures decimals like 2.0000 are properly converted to just 2. However we have to be careful of the fact that integer values have a lower limit than doubles.
                    if (num == Math.Floor(num) && num <= int.MaxValue)
                    {
                        normalized += ((int)num).ToString();
                    }
                    else
                    {
                        normalized += num.ToString();
                    }
                }
                else
                {
                    normalized += token;
                }
            }
        }

        return normalized;
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
#pragma warning disable SA1201 // Operator is being overriden on purpose
    public static bool operator ==(Formula f1, Formula f2)
    {
        return f1.Equals(f2);
    }
#pragma warning restore SA1201 // Operator is being overriden on purpose

    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2)
    {
        return !(f1 == f2);
    }

    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is Formula otherFormula)
        {
            return this.ToString() == otherFormula.ToString();
        }

        return false;
    }

    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a Normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's (double) value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method should expect
    ///     variable names to be capitalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a formula error, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        Stack<double> valueStack = new();
        Stack<string> operatorStack = new();
        bool divideByZero;
        foreach (string token in tokenList)
        {
            // Handles numbers
            if (TokenType(token) == "number")
            {
                double tokenValue = double.Parse(token);
                divideByZero = NumVarCurrentTokenEvaluation(valueStack, operatorStack, tokenValue);
                if (divideByZero)
                {
                    return new FormulaError("Divide by Zero Error!");
                }
            }

            // Handles variables (NOTE: The delegate lookup method is used to get the value of the variable.
            // The delegate function is expected to throw an ArgumentException if the variable is undefined.
            else if (TokenType(token) == "variable")
            {
                double tokenValue = lookup(token.ToUpper());
                divideByZero = NumVarCurrentTokenEvaluation(valueStack, operatorStack, tokenValue);
                if (divideByZero)
                {
                    return new FormulaError("Divide by Zero Error!");
                }
            }

            // Handle + or - token
            else if (token == "+" || token == "-")
            {
                OperatorsTopOfStack(valueStack, operatorStack);
                operatorStack.Push(token);
            }

            // Handles if the token is * or /
            else if (token == "*" || token == "/")
            {
                operatorStack.Push(token);
            }

            // Handles left Parenthesis
            else if (TokenType(token) == "leftParenthesis")
            {
                operatorStack.Push(token);
            }

            // Handles right Parenthesis
            else if (TokenType(token) == "rightParenthesis")
            {
                OperatorsTopOfStack(valueStack, operatorStack);

                // The top of the operator stack should be a '('. So we pop it.
                operatorStack.Pop();

                // Making sure we aren't dividing by zero first.
                if (valueStack.Peek() == 0 && operatorStack.Peek() == "/")
                {
                    return new FormulaError("Divide by Zero Error!");
                }

                // Now we check for mulitiplication and division (+/-)
                OperatorsTopOfStack(valueStack, operatorStack);
            }
        }

        // Final computations for remaining value/operators
        if (operatorStack.Count == 0)
        {
            return valueStack.Pop();
        }
        else
        {
            OperatorsTopOfStack(valueStack, operatorStack);
        }

        return valueStack.Pop();
    }

    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be extremely small.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        int hash = 0;
        int count = 0;

        foreach (char character in canonicalForm)
        {
            count++;
            hash += count * character;

            if (character % 7 == 0)
            {
                hash *= -count;
            }
        }

        return hash;
    }

    /// <summary>
    /// This private helper methods handles when an operator is pending at the top of the operator stack.
    /// </summary>
    /// <param name="valueStack"> The stack that holds current values.</param>
    /// <param name="operatorStack"> The stack that holds current operators.</param>
    private static void OperatorsTopOfStack(Stack<double> valueStack, Stack<string> operatorStack)
    {
        bool notEmpty = operatorStack.Count > 0;

        // If we see another operator, we need to first evaluate the current operator.
        if ((notEmpty && operatorStack.Peek() == "+") || (notEmpty && operatorStack.Peek() == "-") || (notEmpty && operatorStack.Peek() == "*") || (notEmpty && operatorStack.Peek() == "/"))
        {
            double number1 = valueStack.Pop();
            double number2 = valueStack.Pop();
            string currentOperator = operatorStack.Pop();

            if (currentOperator == "+")
            {
                valueStack.Push(number2 + number1);
            }
            else if (currentOperator == "-")
            {
                valueStack.Push(number2 - number1);
            }
            else if (currentOperator == "*")
            {
                valueStack.Push(number2 * number1);
            }
            else
            {
                // We do not check for divide by zero error, that is handled in the evaluate method when necessary.
                valueStack.Push(number2 / number1);
            }
        }
    }

    /// <summary>
    /// This is a private helper method for the purposes of evaluating values and operators if the current token is a variable or number.
    /// </summary>
    /// <param name="valueStack"> The stack that holds current values.</param>
    /// <param name="operatorStack">The stack that holds current operators. </param>
    /// <param name="tokenValue">The value of the most current token.</param>
    /// <returns> Returns bool which then determines if a FormulaError object should be created. </returns>
    private static bool NumVarCurrentTokenEvaluation(Stack<double> valueStack, Stack<string> operatorStack, double tokenValue)
    {
        // This handles multiplication and division order of operations.
        if ((operatorStack.Count > 0 && operatorStack.Peek() == "*") || (operatorStack.Count > 0 && operatorStack.Peek() == "/"))
        {
            string op = operatorStack.Pop();
            double number = valueStack.Pop();
            if (op == "*")
            {
                valueStack.Push(number * tokenValue);
            }
            else
            {
                // Throw divide by zero error
                if (tokenValue == 0)
                {
                    return true;
                }

                valueStack.Push(number / tokenValue);
            }
        }

        // Add the number normally if we don't need to worry about multiplication or division
        else
        {
            valueStack.Push(tokenValue);
        }

        return false;
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
#pragma warning disable SA1402 // File may only contain a single type
public class FormulaError
#pragma warning restore SA1402 // File may only contain a single type
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
#pragma warning disable SA1402 // File may only contain a single type
public class FormulaFormatException : Exception
#pragma warning restore SA1402 // File may only contain a single type
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}
