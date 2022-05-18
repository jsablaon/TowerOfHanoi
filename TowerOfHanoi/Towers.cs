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
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using static TowerUtilities;


public class Towers
{
    public Queue<MoveRecord> autosolveRecord = new Queue<MoveRecord>();
    public Queue<MoveRecord> autosolveIterativeRecord = new Queue<MoveRecord>();
    public int NumberOfDiscs { get; internal set; }
    public int NumberOfMoves { get; internal set; }
    public bool IsComplete = false;
    public int MinimumPossibleMoves { get; internal set; }

    List<Stack<int>> poles = new List<Stack<int>>
    {
        new Stack<int>(),
        new Stack<int>(),
        new Stack<int>()
    };

    public int[][] jaggedArr;

    public Towers(int pNumberOfDiscs = 3) // defaults to 3 if not specified
    {
        if (pNumberOfDiscs < 1 || pNumberOfDiscs > 9)
        {
            throw new InvalidHeightException("Invalid number of discs. Input a number between 1 - 9.");
        }

        for (int i = pNumberOfDiscs; i > 0; i--)
        {
            poles[0].Push(i);
            NumberOfDiscs++;
        }

        MinimumPossibleMoves = (int)Math.Pow(2, pNumberOfDiscs) - 1;
    }
    public MoveRecord Move(int from, int to)
    {
        if (from < 1 || from > 3 || to < 1 || to > 3)
        {
            throw new InvalidMoveException("Towers do not exist. Must only be tower 1 through tower 3.");
        }
        if (from == to)
        {
            throw new InvalidMoveException("Move cancelled");
        }
        if (poles[from - 1].Count == 0)
        {
            throw new InvalidMoveException($"Tower {from} is empty.");
        }
        int currDisc = poles[from - 1].Peek();
        if (poles[to - 1].Count != 0)
            if (poles[from - 1].Peek() > poles[to - 1].Peek())
                throw new InvalidMoveException($"Top disc from {from} is larger that top disc on tower {to}");

        poles[to - 1].Push(poles[from - 1].Pop());
        ++NumberOfMoves;

        if (poles[0].Count == 0 && poles[1].Count == 0 && poles[2].Count == NumberOfDiscs) IsComplete = true;

        return new MoveRecord(NumberOfMoves, currDisc, from, to);

    }

    public int[][] ToArray()
    {
        jaggedArr = new int[3][];
        for (int i = 0; i < jaggedArr.Length; i++)
        {
            jaggedArr[i] = poles[i].ToArray();
        }
        return jaggedArr;
    }


    public void AutoSolve(int source, int dest, int aux, int numOfDisc, Towers t)
    {
        if (numOfDisc > 1)
        {
            AutoSolve(source, aux, dest, numOfDisc - 1, t);
        }

        autosolveRecord.Enqueue(Move(source, dest));
        System.Threading.Thread.Sleep(300);

        DisplayTowers(t);
        Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {source} to tower {dest}\n");

        if (numOfDisc > 1)
        {
            AutoSolve(aux, dest, source, numOfDisc - 1, t);
        }
    }

    private bool IsValidMove(int from, int to)
    {
        bool isValid;
        from = from - 1;
        to = to - 1;
        if(poles[from].Any() && poles[to].Any())
        {
            if (poles[from].Peek() > poles[to].Peek())
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }
        }
        else
        {
            if(poles[from].Count == 0)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            
        }

        return isValid;
    }

    private bool Check()
    {
        if (poles[2].Count == NumberOfDiscs)
        {
            IsComplete = true;
        }
        else
        {
            IsComplete = false;
        }
        return IsComplete;
    }
    public void AutoSolveIterative(int numberOfDiscs, Towers t)
    {
        int duration = 1;
        while(!t.IsComplete)
        {
            if (numberOfDiscs % 2 == 0)
            {
                if (IsValidMove(1, 2))
                {
                    autosolveIterativeRecord.Enqueue(t.Move(1, 2));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {1} to tower {2}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                else
                {
                    autosolveIterativeRecord.Enqueue(t.Move(2, 1));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {2} to tower {1}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                IsComplete = Check();
                if (IsComplete) return;
                Console.WriteLine("Press any key to see the next move");
                Console.ReadLine();

                if (IsValidMove(1, 3))
                {
                    autosolveIterativeRecord.Enqueue(t.Move(1, 3));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {1} to tower {3}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                else
                {
                    autosolveIterativeRecord.Enqueue(t.Move(3, 1));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {3} to tower {1}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                IsComplete = Check();
                if (IsComplete) return;
                Console.WriteLine("Press any key to see the next move");
                Console.ReadLine();

                if (IsValidMove(2, 3))
                {
                    autosolveIterativeRecord.Enqueue(t.Move(2, 3));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {2} to tower {3}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                else
                {
                    autosolveIterativeRecord.Enqueue(t.Move(3, 2));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {3} to tower {2}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                IsComplete = Check();
                if (IsComplete) return;
                Console.WriteLine("Press any key to see the next move");
                Console.ReadLine();

            }
            else
            {
                if (IsValidMove(1, 3))
                {
                    autosolveIterativeRecord.Enqueue(t.Move(1, 3));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {1} to tower {3}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                else
                {
                    autosolveIterativeRecord.Enqueue(t.Move(3, 1));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {3} to tower {1}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                IsComplete = Check();
                if (IsComplete) return;
                Console.WriteLine("Press any key to see the next move");
                Console.ReadLine();

                if (IsValidMove(1, 2))
                {
                    autosolveIterativeRecord.Enqueue(t.Move(1, 2));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {1} to tower {2}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                else
                {
                    autosolveIterativeRecord.Enqueue(t.Move(2, 1));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {2} to tower {1}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                IsComplete = Check();
                if (IsComplete) return;
                Console.WriteLine("Press any key to see the next move");
                Console.ReadLine();

                if (IsValidMove(2, 3))
                {
                    autosolveIterativeRecord.Enqueue(t.Move(2, 3));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {2} to tower {3}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                else
                {
                    autosolveIterativeRecord.Enqueue(t.Move(3, 2));
                    DisplayTowers(t);
                    Console.WriteLine($"Move {t.NumberOfMoves} complete. Successfully moved disc from tower {3} to tower {2}\n");
                    System.Threading.Thread.Sleep(duration);
                }
                IsComplete = Check();
                if (IsComplete) return;
                Console.WriteLine("Press any key to see the next move");
                Console.ReadLine();
            }
        }
    }
}
public class MoveRecord
{
    public int MoveNumber { get; internal set; }
    public int Disc { get; set; }
    public int From;
    public int To;
    public Towers TowerState { get; internal set; }
    public MoveRecord(int moveNumber, int disc, int from, int to/*, Towers tower*/)
    {
        MoveNumber = moveNumber;
        Disc = disc;
        From = from;
        To = to;
        //TowerState = tower;
    }
}

