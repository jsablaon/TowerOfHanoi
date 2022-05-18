/************************************************************** 
*	Course:     PROG 260 
*	Instructor: Dennis Minium 
*	Term:       Fall 2019 
*
*	Programmer: John Sablaon
*	Assignment: Course Project - Checkpoint 1 - Implement Phase 1 & 2 
*	
*	Description:
*	Implement a towers class, user defined exceptions, unit test, program UI
* 
*	Revision    Date               Release Comment 
*	--------     ----------        ------------------------ 
*	1.0         10/27/2019         Initial Release 
*	
* 
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InvalidHeightException : Exception
{
    public InvalidHeightException()
    {
    }

    public InvalidHeightException(string message) : base(message)
    {
    }

    public InvalidHeightException(string message, Exception inner) : base(message, inner)
    {
    }

}


