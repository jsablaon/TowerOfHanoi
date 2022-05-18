/************************************************************** 
*	Course:     PROG 260 
*	Instructor: Dennis Minium 
*	Term:       Fall 2019 
*
*	Programmer: John Sablaon
*	Assignment: Course Project
*	
*	Description:
*	Implement a towers class, user defined exceptions, unit test, program UI
* 
*	Revision    Date               Release Comment 
*	--------     ----------        ------------------------ 
*	1.0         10/27/2019         Initial Release 
*	1.1         11/17/2019         Implement phase 3,4,5
*	1.2         12/2/2019          Implement phase 6
*	1.3         12/4/2019          Implement phase 7
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TowerUtilities;
using static System.Console;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Threading;
using System.Runtime.DesignerServices;
using System.CodeDom;
using System.ComponentModel.Design;

class Program
{

    static void Main(string[] args)
    {
        string quit;
        string from;
        string to;
        string approach;
        MoveRecord tempRecord;
        const string CtrlZ = "\u001a";
        const string CtrlY = "\u0019";

        Queue<MoveRecord> moveRecord = new Queue<MoveRecord>();
        Stack<MoveRecord> zStack = new Stack<MoveRecord>(); // Ctrl+z = undo
        Stack<MoveRecord> yStack = new Stack<MoveRecord>(); // Ctrl+y = redo
    Start:
        try
        {
            do
            {
                Write("\nHow many disc in your tower (default is 3, max is 9): ");
                int.TryParse(ReadLine(), out int numOfDisc);
                Towers t = new Towers(numOfDisc);
                DisplayTowers(t);
                SelectOptionToSolve:
                Write("Options: " +
                    "\n- M - Solve the puzzle manually" +
                    "\n- A - Auto-solve " +
                    "\n- S - Auto-solve step-by-step" +
                    "\n\n\nChoose an approach: ");
                approach = ReadLine();
                if(approach.ToUpper() == "M")
                {
                    goto manualStart;
                }
                else if (approach.ToUpper() == "A")
                {
                    WriteLine("Auto-solve (Recursion Algo) is selected. Press enter when ready.");
                    ReadLine();
                    t.AutoSolve(1, 3, 2, numOfDisc, t);
                    WriteLine($"Number of moves: {t.NumberOfMoves}");

                    Write("\nWould you like to see a list of the moves you made? (Y or N): ");
                    string showMoveRecord = ReadKey().KeyChar.ToString().ToUpper();
                    if (showMoveRecord == "Y")
                    {
                        WriteLine("\n");
                        foreach (MoveRecord move in t.autosolveRecord)
                        {
                            WriteLine($"{move.MoveNumber}. Disc {move.Disc} moved from tower {move.From} to tower {move.To}");
                        }
                    }

                    WriteLine("\nWant to try again? (Y or N): ");
                    quit = ReadKey().KeyChar.ToString().ToUpper();
                    while (quit != "N")
                    {
                        if (quit == "Y") goto Start;
                        WriteLine("\nWant to try again? (Y or N): ");
                        quit = ReadKey().KeyChar.ToString().ToUpper();
                    }
                    goto Finish;
                }
                else if (approach.ToUpper() == "S")
                {
                    WriteLine("Auto-solve (Iterative Algo) step-by-step is selected. Press enter when ready.");
                    ReadLine();

                    t.AutoSolveIterative(t.NumberOfDiscs, t);

                    WriteLine($"Number of moves: {t.NumberOfMoves}");

                    Write("\nWould you like to see a list of the moves you made? (Y or N): ");
                    string showMoveRecord = ReadKey().KeyChar.ToString().ToUpper();
                    if (showMoveRecord == "Y")
                    {
                        WriteLine("\n");
                        foreach (MoveRecord move in t.autosolveIterativeRecord)
                        {
                            WriteLine($"{move.MoveNumber}. Disc {move.Disc} moved from tower {move.From} to tower {move.To}");
                        }
                    }

                    WriteLine("\nWant to try again? (Y or N): ");
                    quit = ReadKey().KeyChar.ToString().ToUpper();
                    while (quit != "N")
                    {
                        if (quit == "Y") goto Start;
                        WriteLine("\nWant to try again? (Y or N): ");
                        quit = ReadKey().KeyChar.ToString().ToUpper();
                    }
                    goto Finish;
                }
                else
                {
                    WriteLine($"{approach} is not a valid option");
                    goto SelectOptionToSolve;
                }
                manualStart:
                do
                {
                    WriteLine($"Move {t.NumberOfMoves + 1}:\n");
                Again:
                    try
                    {
                        do
                        {
                            Write("\nEnter 'from' tower number, 'Ctrl+z' to undo, 'Ctrl+y to redo, or 'q' to quit: ");
                            from = ReadKey().KeyChar.ToString().ToUpper();
                            if (from != "1" && from != "2" && from != "3" && from != "Q" && from != CtrlZ && from != CtrlY)
                            {
                                WriteLine("\r\n Invalid value. Valid values: 1, 2, 3, Ctrl+z, q");
                            }
                        }
                        while (from != "1" && from != "2" && from != "3" && from != "Q" && from != CtrlZ && from != CtrlY);
                        if (from == "Q") break;
                        if (from == CtrlZ && zStack.Count != 0)
                        {
                            tempRecord = zStack.Pop();
                            moveRecord.Enqueue(t.Move(tempRecord.To, tempRecord.From));
                        }
                        else if(from == CtrlY && yStack.Count != 0)
                        {
                            tempRecord = yStack.Pop();
                            zStack.Push(tempRecord);
                            moveRecord.Enqueue(t.Move(tempRecord.From, tempRecord.To));
                        }
                        else
                        {
                            do
                            {
                                Write("\nEnter 'to' tower number, or 'c' to cancel: ");
                                to = ReadKey().KeyChar.ToString().ToUpper();
                                if (to != "1" && to != "2" && to != "3" && to != "C")
                                {
                                    WriteLine("\r\n Invalid value. Valid values: 1, 2, 3, c");
                                }
                            }
                            while (to != "1" && to != "2" && to != "3" && to != "C");
                            if (to == "C")
                            {
                                WriteLine("\nMove cancelled.");
                                goto Again;
                            }
                            tempRecord = t.Move(int.Parse(from), int.Parse(to));
                        }
                        if(from == CtrlZ)
                        {
                            WriteLine("*****");
                            yStack.Push(tempRecord);
                        }
                        else
                        {
                            moveRecord.Enqueue(tempRecord);
                            zStack.Push(tempRecord);
                            yStack.Clear();
                        }
                    }
                    catch (InvalidMoveException e)
                    {
                        WriteLine($"\n\nInvalid move: {e.Message}");
                        goto Again;

                    }
                    DisplayTowers(t);
                    if(from == CtrlZ)
                    {
                        WriteLine($"Move {t.NumberOfMoves} complete by undo of move {t.NumberOfMoves - 1}. Disc restored from tower {tempRecord.To} to tower {tempRecord.From}\n");
                    }
                    else if(from == CtrlY)
                    {
                        WriteLine($"Move {t.NumberOfMoves} complete by redo of undo move {t.NumberOfMoves - 1}. Disc restored from tower {tempRecord.From} to tower {tempRecord.To}\n");
                    }
                    else
                    {
                        WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {tempRecord.From} to tower {tempRecord.To}\n");
                    }
                    
                }
                while (!t.IsComplete);
                
                if (from == "Q")
                {
                    WriteLine("\nBetter luck next time.");
                    WriteLine("Want to try again? (Y or N): ");
                    quit = ReadKey().KeyChar.ToString().ToUpper();
                }
                else
                {
                    Write($"It took you {t.NumberOfMoves} moves. Congrats! ");
                    if (t.NumberOfMoves == t.MinimumPossibleMoves)
                    {
                        WriteLine("That's the minimum!");
                    }
                    else
                    {
                        WriteLine($"Not bad, but it can be done in {t.MinimumPossibleMoves}.");
                    }
                    Write("\nWould you like to see a list of the moves you made? (Y or N): ");
                    string showMoveRecord = ReadKey().KeyChar.ToString().ToUpper();
                    if (showMoveRecord == "Y")
                    {
                        WriteLine("\n");
                        foreach (MoveRecord move in moveRecord)
                        {
                            WriteLine($"{move.MoveNumber}. Disc {move.Disc} moved from tower {move.From} to tower {move.To}");
                        }
                    }

                    WriteLine("\nWant to try again? (Y or N): ");
                    quit = ReadKey().KeyChar.ToString().ToUpper();
                }
                Clear();
            }
            while (quit.ToUpper() != "N");
            Finish:
            WriteLine("\nHave a great day!");
        }
        catch (InvalidHeightException e)
        {
            WriteLine($"\n\nTry again : {e.Message}");
            goto Start;
        }
        catch (Exception e)
        {
            WriteLine($"\n\nError : {e.Message}");
            goto Start;
        }
    }
}



