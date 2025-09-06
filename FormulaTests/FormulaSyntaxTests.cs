// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CS3500.Formula;
/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNoTokens_Invalid()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula(string.Empty));
    }

    /// <summary>
    /// This tests that having one singular valid token does not throw any errors.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOneToken_Valid()
    {
        new Formula("1");
    }

    // --- Tests for Valid Token Rule ---
    /// <summary>
    /// This method should throw an exception for an invalid variable,
    /// which in this case is a number BEFORE a variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Invalid_Variable_Uppercase()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("1A"));
    }
    /// <summary>
    /// This method should also throw an exception for an invalid variable,
    /// which in this case is a number BEFORE a variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Invalid_Variable_Lowercase()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("1c"));
    }

    /// <summary>
    /// This method should also throw an exception for an invalid variable,
    /// as a number is sandwiched between two variable characters. 
    /// EDIT: Actually the given getTokens method splits the variable where it stops being valid. So here, we get [c12453, h6]. Still should give an error, but for a different reason now.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Invalid_Variable_Scrambled()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("c12453h6"));
    }
    /// <summary>
    /// This method should run with no exceptions, as variables are defined by letter(s).
    /// followed by number(s). 
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Variable_Uppercase()
    {
        new Formula("A1");
    }

    /// <summary>
    /// This methods should throw an error as letters only are not valid for a variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Invalid_Variable_Letters_Only()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("fnjsnajow"));
    }

    /// <summary>
    /// This method should run with no exceptions, as variables are defined by letter(s).
    /// followed by number(s).
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Variable_Lowercase()
    {
        new Formula("a1");
    }

    /// <summary>
    /// This method should run with no errors, however strange a bunch of zeroes seems.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Zeroes()
    {
        new Formula("000000000000000000");
    }

    /// <summary>
    /// This method should run with no errors, however strange a bunch of zeroes seems.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Decimals()
    {
        new Formula(" 12345.6789 ");
    }

    /// <summary>
    /// This method should run with no errors,  it is valid operations.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Operations()
    {
        new Formula("AB12345 + 23 * (8 / 2) - 10");
    }
    /// <summary>
    /// The program does allow for decimals points to be the start of a number
    /// For instance, both "1+ 0.25" and "1+ .25" are equally valid.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Decimal_only()
    {
        new Formula("1 + .25");
    }


    /// <summary>
    /// This method should run with no errors, multiple spaces should just seperate tokens.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Extra_Spaces()
    {
        new Formula("12345              + 23 * (8 /    2) -   10");
    }

    /// <summary>
    /// Formula constructor does not support factorials, so this should throw an error.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Invalid_Operation()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("5!"));
    }

    /// <summary>
    /// Formula constructor does not support factorials, so this should throw an error.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Invalid_Character()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("@69"));
    }

    /// <summary>
    /// This method should run with no errors, as scientific notation is supported.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Scientific_Lowercase()
    {
        new Formula("0+ 2e5");
    }

    /// <summary>
    /// This method should run with no errors, as negative scientific notation is supported.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Scientific_Lowercase_Negative()
    {
        new Formula("0 + 2e-5");
    }

    /// <summary>
    /// This method should run with no errors, as uppercase scientific notation is supported.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Scientific_Uppercase()
    {
        new Formula("0 + 2E5");
    }

    /// <summary>
    /// This method should run with no errors, as uppercase negative scientific notation is supported.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Scientific_Uppercase_Negative()
    {
        new Formula("0 + 2E-5");
    }

    /// <summary>
    /// This method should run with no errors, as uppercase positive scientific notation is supported.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Scientific_Uppercase_Positive()
    {
        new Formula("0 + 2E+5");
    }

    /// <summary>
    /// This method should run with no errors, as lowercase positive scientific notation is supported.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToken_Valid_Scientific_Lowercase_Positive()
    {
        new Formula("0 + 2e+5");
    }

    // --- Tests for Closing Parenthesis Rule

    /// <summary>
    /// This method is testing to make sure a FormulaFormatException is thrown 
    /// when there is a valid pair of parenthesis, but an extra mismatched closing. 
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestClosing_Pairs()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(10) +  2)"));
    }

    /// <summary>
    /// This method is testing to make sure a FormulaFormatException is thrown 
    /// when there is one mismatched closing parenthesis. 
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestClosing_No_Pair()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("10)"));
    }

    /// <summary>
    /// This method is testing to make sure a FormulaFormatException is thrown 
    /// when a closing parenthesis comes before the opening.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestClosing_First()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("0) + (10"));
    }
    /// <summary>
    /// This method uses closing parenthesis properly, no error should be thrown
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestClosing_Proper()
    {
        new Formula("(abc123) +  (def456)");
    }


    // --- Tests for Balanced Parentheses Rule

    /// <summary>
    /// This method is testing to make sure a FormulaFormatException is thrown 
    /// when there is a valid pair of parenthesis, but also an invalid pair. 
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalanced_Pairs()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(10) - (5) +  2)"));
    }

    /// <summary>
    /// This method is testing to make sure a FormulaFormatException is thrown 
    /// when there only one open bracket with no closing pair.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalanced_NoPair()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(10"));
    }

    /// <summary>
    /// This method makes sure that a balanced nested parenthesis equation works properly.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalanced_Proper_Nested()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(18 + (10) * 20 ) - (5) +  2)"));
    }
    /// <summary>
    /// This method makes sure that a simple balanced parenthesis equation works properly.
    /// </summary>
    /// 
    [TestMethod]
    public void FormulaConstructor_TestBalanced_Proper_Simple()
    {
        new Formula("(a0 + 5) * 2");
    }


    // --- Tests for First Token Rule

    /// <summary>
    /// This method ensures that starting with an opening bracket works normally.
    /// </summary>
    /// 
    [TestMethod]
    public void FormulaConstructor_TestFirst_OpenParenthesis()
    {
        new Formula("(1289043.3683 / 9)");
    }

    /// <summary>
    /// This method ensures that starting with a number works normally.
    /// </summary>
    /// 
    [TestMethod]
    public void FormulaConstructor_TestFirst_Number()
    {
        new Formula("12 + a2");
    }

    /// <summary>
    /// This method should throw an error as it uses a closing bracket as the first character.
    /// </summary>
    /// 
    [TestMethod]
    public void FormulaConstructor_TestFirst_ClosedParenthesis()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula(") - 1289043.3683 / 9)"));
    }

    /// <summary>
    /// This method should throw an error as it uses an operator as the first character.
    /// </summary>

    [TestMethod]
    public void FormulaConstructor_TestFirst_Operator()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("+ 1289043.3683 / 9)"));
    }

    /// <summary>
    /// This method should throw an error as it uses a random character as the first character.
    /// </summary>
    /// 
    [TestMethod]
    public void FormulaConstructor_TestFirst_RandomCharacter()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("$ + 1289043.3683 / 9)"));
    }

    /// <summary>
    /// Starting with a decimal should work, as .25 is a number token, despite starting with a period. 
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirst_DecimalPoint()
    {
        new Formula(".25");
    }

    /// <summary>
    /// Should allow the first token to be a variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirst_Variable()
    {
        new Formula("abc123 + easy0");
    }

    // --- Tests for  Last Token Rule ---

    /// <summary>
    /// Should throw an error as an opening bracket is not allowed at the end.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLast_Open_Parenthesis()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("abc123 + 0 - ("));
    }

    /// <summary>
    /// Should run fine as a closing bracket is allowed at the end.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLast_Closed_Parenthesis()
    {
        new Formula("abc123 + (0)");
    }

    /// <summary>
    /// Should throw an error as an operator is not allowed at the end.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLast_Operator()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("abc123 + 0 -"));
    }

    /// <summary>
    /// Should run fine as a variable is allowed at the end of a formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLast_Variable()
    {
        new Formula("10 + ACDC42");
    }

    /// <summary>
    /// Should run fine as a number is allowed at the end of a formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLast_Number()
    {
        new Formula("10 + 42");
    }

    /// <summary>
    /// Should throw a formula format exception error as only relevant characters are allowed at the end of a formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLast_Random_Character()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("GGs0 + 42 - &"));
    }

    // --- Tests for Parentheses/Operator Following Rule ---

    /// <summary>
    /// Should run fine as numbers are allowed following an opening parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFollowing_Number()
    {
        new Formula("(1245641673283 + 37189298 * 238)");
    }

    /// <summary>
    /// Should run fine as variables are allowed following an opening parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFollowing_Variable()
    {
        new Formula("(ABC1245641673283 + 37189298 * 238)");
    }

    /// <summary>
    /// Should run fine as opening parenthesis are allowed following an opening parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFollowing_Open_Parenthesis()
    {
        new Formula("((((((ABC1245641673283 + 37189298 * 238))))))");
    }

    /// <summary>
    /// Should throw a formula format exception error as a closing parenthesis cannot follow an opening parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFollowing_Closed_Parenthesis()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("() + 2"));
    }

    /// <summary>
    /// Should throw a formula format exception error as operators cannot follow an opening parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFollowing_Operator()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(+ 1)"));
    }

    /// <summary>
    /// Should throw a formula format exception error as random characters cannot follow an opening parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFollowing_Random_Character()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(? - 1)"));
    }


    // --- Tests for Extra Following Rule ---

    /// <summary>
    /// Should throw a formula format exception error as numbers cannot follow variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Variable_Number()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("Abc123  2"));
    }

    /// <summary>
    /// Should throw a formula format exception error as variables cannot follow variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Variable_Variable()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("Abc123  z52"));
    }

    /// <summary>
    /// Should throw a formula format exception error as variables cannot follow variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Variable_Open_Parenthesis()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("Abc123  (48) "));
    }

    /// <summary>
    /// Should throw a formula format exception error as variables cannot follow variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Variable_Random_Character()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("Abc123  & def456 "));
    }

    /// <summary>
    /// Should run without error as closing parenthesis can follow variables.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Variable_Closing_Parenthesis()
    {
        new Formula("( 89 * Abc123)");
    }

    /// <summary>
    /// Should run without error as operators can follow variables.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Variable_Operator()
    {
        new Formula("agyDBJH23819 / 0");
    }

    /// <summary>
    /// Should throw a formula format exception error as random characters cannot follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Number_Random()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("1 } 2"));
    }

    /// <summary>
    /// Should throw a formula format exception error as numbers cannot follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Number_Number()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("123  2"));
    }

    /// <summary>
    /// Should throw a formula format exception error as open parenthesis cannot follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Number_Open_Parenthesis()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("123 (10 + 2) "));
    }

    /// <summary>
    /// Should throw a formula format exception error as a variable cannot follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Number_Variable()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("123 a5 "));
    }

    /// <summary>
    /// Should throw a formula format exception error as a random character cannot follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Number_Random_Character()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("123 ? 6 "));
    }

    /// <summary>
    /// Should run without issue as an operator can follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Number_Operator()
    {
        new Formula("123 * 6 ");
    }

    /// <summary>
    /// Should run without issue as an closing parenthesis can follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Number_Closing_Parenthesis()
    {
        new Formula("( a55 +123 * 6)");
    }

    /// <summary>
    /// Should throw a formula format exception error as random characters cannot follow a closing parenthesis
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Closing_Parenthesis_Random()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(1)?"));
    }

    /// <summary>
    /// Should throw a formula format exception error as numbers cannot follow a closing parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Closing_Parenthesis_Number()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(1748) 2"));
    }

    /// <summary>
    /// Should throw a formula format exception error as open parenthesis cannot follow a closing parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Closing_Parenthesis_Open_Parenthesis()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(10 + 2)(5) "));
    }

    /// <summary>
    /// Should throw a formula format exception error as a variable cannot follow a closing parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Closing_Parenthesis_Variable()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(123) a5 "));
    }

    /// <summary>
    /// Should throw a formula format exception error as a random character cannot follow a closing parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Closing_Parenthesis_Random_Character()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("123 ? 6 "));
    }

    /// <summary>
    /// Should run without issue as an operator can follow a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Closing_Parenthesis_Operator()
    {
        new Formula("(a55 / 10) - 2 ");
    }

    /// <summary>
    /// Should run without issue as an closing parenthesis can follow a closing parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtra_Closing_Parenthesis_Closing_Parenthesis()
    {
        new Formula("((55 +123 * 6))");
    }

    // --- Tests for toString() ---

    /// <summary>
    /// Testing that creating an acceptable formula and then calling toString does not crash the program.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToString_No_Crashes()
    {
        new Formula("((55 +123 * 6))").ToString();
    }

    /// <summary>
    /// Testing that the formula is normalized properly in a very complex case (variables are capitalized and white space removed).
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToString_Outputs_Complex()
    {
        Assert.AreEqual("((XABCD55+123*WWW6))", new Formula("((xaBCD55 +     123 * www6))").ToString());
    }
    /// <summary>
    /// Testing that the formula is normalized properly in a very simple case (variables are capitalized and white space removed).
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToString_Outputs_Simple()
    {
        Assert.AreEqual("ABC123+G2", new Formula("aBC123 + g2").ToString());
    }

    /// <summary>
    /// Making sure ToString() doesn't somehow prevent a crash from an invalid formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToString_Crashes()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(123) a5 ").ToString());
    }

    /// <summary>
    /// Making sure ToString() still crashes, as empty formulas are not valid.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestToString_Empty()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula(string.Empty).ToString());
    }

    // --- Tests for getVariables() ---

    /// <summary>
    /// This tests that get variables returns three unique variables from a simple formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetVariables_Simple()
    {
        Assert.IsTrue(new HashSet<string> { "A1", "B2", "C3" }.SetEquals(new Formula("A1 + B2 - C3").GetVariables()));
    }

    /// <summary>
    /// This tests that get variables returns three unique variables from a complex formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetVariables_Complex()
    {
        Assert.IsTrue(new HashSet<string> { "AGHY1", "B212", "C3", "EF43" }.SetEquals(new Formula("aGhY1   +   b212 - C3/2 + 100 * 10 - eF43").GetVariables()));
    }

    /// <summary>
    /// This tests that get variables returns three unique variables from formula with repeated variables.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetVariables_Repeats()
    {
        Assert.IsTrue(new HashSet<string> { "AGHY1", "B2" }.SetEquals(new Formula("aGhY1   +   AGHY1 - AgHy1 + 100 * 10 - AGHy1 + b2").GetVariables()));
    }

    /// <summary>
    /// Making sure GetVariables() still crashes, as empty formulas are not valid.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetVariables_Empty()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula(string.Empty).GetVariables());
    }

    /// <summary>
    /// Making sure GetVariables() doesn't somehow prevent a crash from an invalid formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_GetVariables_Crashes()
    {
        Assert.ThrowsExactly<FormulaFormatException>(() => _ = new Formula("(123) a5 ").GetVariables());
    }

}
