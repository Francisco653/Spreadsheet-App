```
Author:     Francisco Pinas
Partner:    None
Start Date: September 4th, 2025
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  Francisco653
Repo:       https://github.com/uofu-cs3500-20-fall2025/spreadsheet-Franinja1
Commit Date: September 19th, 2025
Solution:   Spreadsheet
Copyright:  CS 3500 and Francisco Pinas - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of verifying the validity of formulas, ensuring that they match all syntax expectations. It also is capable of tracking dependencies between cells in a spreadsheet. 
It can now evaluate formulas and compare them to one another.


# Time Expenditures:

    1. Assignment One:   Predicted Hours:          8        Actual Hours:   7
    2. Assignment Two:   Predicted Hours:           12        Actual Hours:    5
    3. Assignment Three:   Predicted Hours:           8        Actual Hours:    4.5
    4. Assignment Four :   Predicted Hours:           5        Actual Hours:    5.5
    5. Assignment Five :   Predicted Hours:           7        Actual Hours:    6
    6. Assignment Six :   Predicted Hours:           8        Actual Hours:    4.5 (so far)

# Examples of Good Software Practice (GSP)
    1. DRY - I updated my Evaluate method in Formula to use two private helpers for repeated processes, which drastically reduced code length and increased readability.
    2. Code Reuse - I properly reused my RemoveDependency and AddDepencency methods when calling ReplaceDependency.
    3. Seperation of concerns - Within my Evaluate method in Formula class, I acknowledge and document that the lookup method used is to be defined elsewhere, and not the concern of Evaluate to define.
    4. DRY - Copied IsVar from formula class to check variable name validity in Spreadsheet.
    5. Code Reuse - I reused the GetCellsToRecalculate method to check for circular dependencies.

# What I've learned about Time Management

Most the assignments I have worked I started the day of or the day before. I have been able to get it away with it so far but it is very uncomfortable having to
work multiple hours in one sitting to finish an assignment. Compared to how I worked in CS2420 though, the time I do spend working is managed differently.
I spend a significant amount of time reading the assignment specifications and writing down API expectations. I spend a lot of time setting up files and 
creating my black box tests. It is a few hours of prep but it has been paying off very well compared to my old style of just programming as I am reading
the assignment, and making tests only after fully implementing everything.
    