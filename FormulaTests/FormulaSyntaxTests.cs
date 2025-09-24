// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

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
///    This file is the FormulaTester Class, used to test the Formula class.
///    This class ensures that the Formula class is properly creating tokens,
///    and enforcing the rules for how a formula should be structured.
///    It now also contains tests for new FormulaClass functionality, ensuring that formulas are properly evaluated.
/// </summary>
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
    /// This method uses closing parenthesis properly, no error should be thrown.
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
    /// Should throw a formula format exception error as random characters cannot follow a closing parenthesis.
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

    // --- ASSIGNMENT 4 PRETESTS ---

    /// <summary>
    /// This tests ensures that two formula objects with only visual syntax differences are considered equal.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEquals_True()
    {
        Assert.IsTrue(new Formula("a1 + b2 - c3 * 2E+5 - 0.25").Equals(new Formula("A1+B2-C3 * 2e5 -.25")));
    }

    /// <summary>
    /// This test ensures that two formula objects with different variables are not considered equal.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEquals_False()
    {
        Assert.IsFalse(new Formula("a1 + b2 - c3").Equals(new Formula("A1+B2-C4")));
    }

    /// <summary>
    /// Comparing to a null object should return false, not crash the program.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEquals_False_Null()
    {
        Assert.IsFalse(new Formula("a1 + b2 - c3").Equals(null));
    }

    /// <summary>
    /// Comparing to a non-formula object should return false, not crash the program.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEquals_False_Other_Type()
    {
        double notAFormula = 3.14;
        Assert.IsFalse(new Formula("a1 + b2 - c3").Equals(notAFormula));
    }

    /// <summary>
    /// This tests ensures that two formula objects with identical syntax are equal.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEquals_Same()
    {
        Assert.IsTrue(new Formula("a1 + b2 - c3 * 2E+5 - 0.25").Equals(new Formula("A1+B2-C3 * 2e5 -.25")));
    }

    /// <summary>
    /// This test simply checks that getting the hash code of a valid formula does not crash the program.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetHashCode_No_Crashes()
    {
        new Formula("41 + 72 - 83").GetHashCode();
    }

    /// <summary>
    /// This tests ensures that two formula objects with only visual syntax differences have the same hash code.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetHashCode_True()
    {
        Assert.AreEqual(new Formula("a1 + b2 - c3 * 2E+5 - 0.25").GetHashCode(), new Formula("A1+B2-C3 * 2e5 -.25").GetHashCode());
    }

    /// <summary>
    /// Distinct formulas should (almost) always have distinct hash codes.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetHashCode_False()
    {
        Assert.AreNotEqual(new Formula("a1 + b2 - c3").GetHashCode(), new Formula("A1+B2-C4").GetHashCode());
    }

    /// <summary>
    /// This tests ensures that two formula objects with only visual syntax differences are considered equal.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEqualOperator_True()
    {
        Assert.IsTrue(new Formula("a1 + b2 - c3 * 2E+5 - 0.25") == new Formula("A1+B2-C3 * 2e5 -.25"));
    }

    /// <summary>
    /// This test ensures that two formula objects with different variables are not considered equal.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEqualOperator_False()
    {
        Assert.IsFalse(new Formula("a1 + b2 - c3") == new Formula("A1+B2-C4"));
    }

    /// <summary>
    /// This tests ensures that two formula objects with only visual syntax differences are considered equal.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNotEqualOperator_False()
    {
        Assert.IsFalse(new Formula("a1 + b2 - c3 * 2E+5 - 0.25") != new Formula("A1+B2-C3 * 2e5 -.25"));
    }

    /// <summary>
    /// This test ensures that two formula objects with different variables are not considered equal.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNotEqualOperator_True()
    {
        Assert.IsTrue(new Formula("a1 + b2 - c3") != new Formula("A1+B2-C4"));
    }

    /// <summary>
    /// This methods ensures that evaluating a formula with variables works properly when provided a valid but very simple lookup function.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Variable()
    {
        Formula f = new Formula("A1 * B1 + 3");
        double TestLookup(string name)
        {
            if (name == "A1")
            {
                return 2;
            }
            else if (name == "B1")
            {
                return 3;
            }
            else
            {
                throw new ArgumentException("I don't know that variable");
            }
        }

        Assert.AreEqual(9D, f.Evaluate(TestLookup));
    }

    /// <summary>
    /// This methods ensures that evaluating a formula with variables works properly when provided a valid but very simple lookup lambda function.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Variable_Lambda()
    {
        Formula f = new ("A1 + B1 - D1 + 1");
        Assert.AreEqual(6D, f.Evaluate((name) => 5));
    }

    /// <summary>
    /// This methods ensures that evaluating a formula without variables works properly.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_NoVariable_Lambda()
    {
        Formula f = new ("10 + 5 * 0 - 10");

        // The value returned is a double not an int.
        Assert.AreEqual(0D, f.Evaluate((name) => 5));
    }

    /// <summary>
    /// This methods ensures that evaluating a formula without variables works properly when provided a valid but very simple lookup function.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_NoVariable()
    {
        Formula f = new Formula(" 6/2  + 4");
        double TestLookup(string name)
        {
            if (name == "A1")
            {
                return 2;
            }
            else if (name == "B1")
            {
                return 3;
            }
            else
            {
                throw new ArgumentException("I don't know that variable");
            }
        }

        // The return value is a double, not an int.
        Assert.AreEqual(7D, f.Evaluate(TestLookup));
    }

    // --- END OF ASSIGNMENT 4 PRETESTS, START OF ASSIGNMENT 4 WHITE BOX TESTS ---

    /// <summary>
    /// This test makes sure that the equal operator returns true for two pointers pointing to the same object.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEqualOperator_Self()
    {
        Formula f1 = new Formula("a5 / a4");
        Formula f2 = f1;
        Assert.IsTrue(f2 == f1);
        Assert.IsTrue(f1 == f2);
    }

    /// <summary>
    /// This test makes sure that the equal operator returns false when a formula is changed.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEqualOperator_Updated()
    {
        Formula f1 = new Formula("a5 / a4");
        Formula f2 = new Formula("A5 /     A4");
        Assert.IsTrue(f2 == f1);
        f2 = new Formula(" 67 + 68 + 69");
        Assert.IsFalse(f1 == f2);
    }

    /// <summary>
    /// This tests ensures that two formula objects are equal even after creating a new but identical object.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEquals_Updated_Same()
    {
        Formula f1 = new ("a1 + b2 - c3 * 2E+5 - 0.25");
        Formula f2 = new ("A1+B2-C3 * 2e5 -.25");
        Assert.IsTrue(f2.Equals(f1));
        f1 = new ("a1 + b2 - c3 * 2E+5 - 0.25");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    /// This tests ensures that two formula objects are not equal after an object is replaced.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEquals_Updated_Different()
    {
        Formula f1 = new ("a1 + b2 - c3 * 2E+5 - 0.25");
        Formula f2 = new ("A1+B2-C3 * 2e5 -.25");
        Assert.IsTrue(f2.Equals(f1));
        f1 = new ("55 + 10 - f90");
        Assert.IsFalse(f1.Equals(f2));
    }

    /// <summary>
    /// This test makes sure that the not equal operator returns false for two pointers pointing to the same object.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNotEqualOperator_Self()
    {
        Formula f1 = new Formula("a5 / a4");
        Formula f2 = f1;
        Assert.IsFalse(f2 != f1);
        Assert.IsFalse(f1 != f2);
    }

    /// <summary>
    /// This test makes sure that the not equal operator returns true when a formula is changed.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNotEqualOperator_Updated()
    {
        Formula f1 = new Formula("a5 / a4");
        Formula f2 = new Formula("A5 /     A4");
        Assert.IsFalse(f2 != f1);
        f2 = new Formula(" 77 + 88 - A9");
        Assert.IsTrue(f1 != f2);
    }

    /// <summary>
    /// Making sure that GetHashCode() returns a unique hash code for two different formulas that are anagrams of each other.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetHashCode_Anagram()
    {
        Assert.AreNotEqual(new Formula("a1 + b2 - c3").GetHashCode(), new Formula("C3 - B2 + a1").GetHashCode());
    }

    /// <summary>
    /// Making sure that GetHashCode() returns a unique hash code when a formula variable is updated with a new object.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGetHashCode_Update()
    {
        Formula f1 = new Formula("81 / b32 - 03");
        int oldHash = f1.GetHashCode();
        f1 = new Formula("a11 + b22 * c44");
        int newHash = f1.GetHashCode();
        Assert.AreNotEqual(oldHash, newHash);
    }

    /// <summary>
    /// This tests make sure that a simple parenthesis case encompassing the whole formula evaluates properly.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Simple_Parenthesis()
    {
        Formula f = new ("(a7 + cdgfhaja1 - Dig65 - 1)");
        Assert.AreEqual(4D, f.Evaluate((name) => 5));
    }

    /// <summary>
    /// This tests make sure that a having multiple parenthesis in a formula still properly evaluates.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Multiple_Parenthesis()
    {
        Formula f = new ("(5 * 11) / (15 - 4)");
        Assert.AreEqual(5D, f.Evaluate((name) => -36572));
    }

    /// <summary>
    /// This tests make sure that a having nested parenthesis in a formula still properly evaluates.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Nested_Parenthesis()
    {
        Formula f = new ("( 1 * (5 * 11) / 1)  / (((15 - 4)))");
        Assert.AreEqual(5D, f.Evaluate((name) => -36572));
    }

    /// <summary>
    /// This tests make sure that a FormulaError object is returned when dividing by zero inside a parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_By_Zero_In_Parenthesis()
    {
        Formula f = new ("(1+A5 - 85 / 0)");
        Assert.IsTrue(f.Evaluate(name => 1000) is FormulaError);
    }

    /// <summary>
    /// This tests make sure that a FormulaError object is returned when dividing a number by zero.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_Number_By_Zero()
    {
        Formula f = new ("10 / 0 - 6");
        Assert.IsTrue(f.Evaluate(name => 7) is FormulaError);
    }

    /// <summary>
    /// This tests make sure that a FormulaError object is returned when dividing a variable by zero.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_Variable_By_Zero()
    {
        Formula f = new ("Y10 / 0 - 6");
        Assert.IsTrue(f.Evaluate(name => 9) is FormulaError);
    }

    /// <summary>
    /// This tests make sure that a FormulaError object is returned when dividing by zero last.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_By_Zero_Last()
    {
        Formula f = new ("672 / 0");
        Assert.IsTrue(f.Evaluate(name => -9) is FormulaError);
    }

    /// <summary>
    /// This tests make sure that a FormulaError object is returned when dividing by variable that equals zero.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_By_Variable_Zero()
    {
        Formula f = new ("672 / A8");
        Assert.IsTrue(f.Evaluate(name => 0) is FormulaError);
    }

    /// <summary>
    /// This tests make sure that a the formula is evaluated properly when dividing by a defined variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_By_Variable_Normal()
    {
        Formula f = new ("98 + A7 / D65 ");
        double TestLookup(string name)
        {
            if (name == "A7")
            {
                return 6;
            }
            else if (name == "D65")
            {
                return 3;
            }
            else
            {
                throw new ArgumentException("I don't know that variable");
            }
        }

        Assert.AreEqual(100D, f.Evaluate(TestLookup));
    }

    /// <summary>
    /// This tests make sure that a the formula is evaluated properly adding inside and outside the right parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Right_Parenthesis_Addition()
    {
        Formula f = new ("(10 + d5) + 6");
        double TestLookup(string name)
        {
            if (name == "A7")
            {
                return 6;
            }
            else if (name == "D65")
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        Assert.AreEqual(16D, f.Evaluate(TestLookup));
    }

    /// <summary>
    /// This tests make sure that a the formula is evaluated properly subtracting inside and outside the right parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Right_Parenthesis_Subtraction()
    {
        Formula f = new ("(10 - D5) - 6");
        double TestLookup(string name)
        {
            if (name == "A7")
            {
                return 6;
            }
            else if (name == "D5")
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        Assert.AreEqual(0D, f.Evaluate(TestLookup));
    }

    /// <summary>
    /// This tests make sure that a the formula is evaluated properly multiplying inside and outside the right parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Right_Parenthesis_Multiplication()
    {
        Formula f = new ("(10 * A7) * 2");
        double TestLookup(string name)
        {
            if (name == "A7")
            {
                return 6;
            }
            else if (name == "D5")
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        Assert.AreEqual(120D, f.Evaluate(TestLookup));
    }

    /// <summary>
    /// This tests make sure that a the formula is evaluated properly dividing inside and outside the right parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Right_Parenthesis_Division()
    {
        Formula f = new ("(10 / A7) / 2");
        double TestLookup(string name)
        {
            if (name == "A7")
            {
                return 5;
            }
            else if (name == "D5")
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        Assert.AreEqual(1D, f.Evaluate(TestLookup));
    }

    /// <summary>
    /// This tests make sure that a the formula is evaluated properly dividing by something inside a parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_By_Parenthesis()
    {
        Formula f = new ("A7 / (10 * 1)");
        double TestLookup(string name)
        {
            if (name == "A7")
            {
                return 20;
            }
            else if (name == "D5")
            {
                return 14;
            }
            else
            {
                return 0;
            }
        }

        Assert.AreEqual(2D, f.Evaluate(TestLookup));
    }

    /// <summary>
    /// This tests make sure that a the formula is evaluated properly dividing by zero in a parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEvaluate_Divide_Inside_Parenthesis_By_Zero()
    {
        Formula f = new ("A7 / (0)");
        double TestLookup(string name)
        {
            if (name == "A7")
            {
                return 20;
            }
            else if (name == "D5")
            {
                return 14;
            }
            else
            {
                return 0;
            }
        }

        Assert.IsTrue(f.Evaluate(TestLookup) is FormulaError);
    }
}
