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
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TowersTests
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ConstructorShouldInitializeLeftTower()
        {
            //Arrage
            Towers testTower;
            int resultExpected = 9;

            //Act
            testTower = new Towers(9);

            //Assert
            Assert.AreEqual(resultExpected, testTower.NumberOfDiscs);
        }

        [TestMethod]
        public void ThrowInvalidHeightException_WhenParamIsInvalid()
        {
            //Arrange
            int testNum = 10;

            //Act - Assert
            Assert.ThrowsException<InvalidHeightException>(() => new Towers(testNum));
        }

    }

    [TestClass]
    public class Move_Should
    {
        [TestMethod]
        public void MoveDisksToDifferentTower()
        {
            //Arrange
            Towers towerUnderTest = new Towers(3);
            int numToTest;
            int numToTest2;
            int[][] towerArr;

            //Act
            towerUnderTest.Move(1, 3);
            towerUnderTest.Move(1, 2);
            towerArr = towerUnderTest.ToArray();
            numToTest = towerArr[2][0];
            numToTest2 = towerArr[1][0];

            //Assert
            Assert.AreEqual(1, numToTest);
            Assert.AreEqual(2, numToTest2);
        }

        [TestMethod]
        public void ReturnException_WhenLargerDiscIsMovedOnTopOfSmallerDisc()
        {
            //Arrange
            Towers towerUnderTest = new Towers(3);

            //Act
            towerUnderTest.Move(1, 3);

            //Assert
            Assert.ThrowsException<InvalidMoveException>(() => towerUnderTest.Move(1,3));
        }

        [TestMethod]
        public void ReturnIsCompleteTrue_WhenAllDiscIsOnRightTower()
        {
            //Arrange
            Towers towerUnderTest = new Towers(3);
            bool Isdone = false;

            //Act
            towerUnderTest.Move(1, 3);
            towerUnderTest.Move(1, 2);
            towerUnderTest.Move(3, 2);
            towerUnderTest.Move(1, 3);
            towerUnderTest.Move(2, 1);
            towerUnderTest.Move(2, 3);
            towerUnderTest.Move(1, 3);
            Isdone = towerUnderTest.IsComplete;

            //Arrange
            Assert.IsTrue(Isdone);
        }
    }

    [TestClass]
    public class ToArray_Should
    {
        [TestMethod]
        public void ConvertStackToArray_WhenCalled()
        {
            //Arrange
            Towers towerUnderTest = new Towers(3);
            int[][] towerArr;
            int val1;
            int val2;
            int val3;

            //Act
            towerUnderTest.Move(1, 3);
            towerUnderTest.Move(1, 2);
            towerArr = towerUnderTest.ToArray();

            val1 = towerArr[0][0];
            val2 = towerArr[1][0];
            val3 = towerArr[2][0];

            //Assert
            Assert.AreEqual(3, val1);
            Assert.AreEqual(2, val2);
            Assert.AreEqual(1, val3);
        }

        [TestMethod]
        public void ReturnCorrectVal_WhenAllDiscIsOnRightTower()
        {
            //Arrange 
            Towers towerUnderTest = new Towers(3);
            int [][] towerArr;

            //Act 
            towerUnderTest.Move(1, 3);
            towerUnderTest.Move(1, 2);
            towerUnderTest.Move(3, 2);
            towerUnderTest.Move(1, 3);
            towerUnderTest.Move(2, 1);
            towerUnderTest.Move(2, 3);
            towerUnderTest.Move(1, 3);
            towerArr = towerUnderTest.ToArray();

            //Assert
            Assert.AreEqual(1, towerArr[2][0]);
            Assert.AreEqual(2, towerArr[2][1]);
            Assert.AreEqual(3, towerArr[2][2]);

        }
    }

}
