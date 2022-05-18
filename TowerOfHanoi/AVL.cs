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
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfHanoi
{
    class AVL<TKey, TValue> where TKey: IComparable
    {
        internal class Node
        {
            public int Key;
            public Towers TowerValue;
            public Node Left;
            public Node Right;
        }

        internal Node Root;

        public AVL()
        {
            Root = null;
        }

        public void Insert(int pKey, Towers pTower)
        {
            Root = Insert(Root, pKey, pTower);
        }

        private Node Insert(Node node, int pKey, Towers pTower)
        {
            if (node == null)
            {
                node = new Node();
                node.Key = pKey;
                node.TowerValue = pTower;
            }
            else if (pKey.CompareTo(node.Key) < 0)
            {
                node.Left = Insert(node.Right, pKey, pTower);
                node = BalanceTree(node);
            }
            return node;
        }

        private Node BalanceTree(Node node)
        {
            int balanceFactor = GetBalanceFactor(node);
            if (balanceFactor > 1)
            {
                if (GetBalanceFactor(node.Left) > 0)
                {
                    node = RotateRight(node);
                }
                else
                {
                    node = RotateLeftRight(node);
                }
            }
            else if (balanceFactor < -1)
            {
                if (GetBalanceFactor(node.Right) > 0)
                {
                    node = RotateRightLeft(node);
                }
                else
                {
                    node = RotateLeft(node);
                }
            }
            return node;
        }

        private Node RotateLeft(Node curr)
        {
            Node pivot = curr.Right;
            curr.Right = pivot.Left;
            pivot.Left = curr;
            return pivot;
        }

        private Node RotateLeftRight(Node curr)
        {
            Node pivot = curr.Left;
            curr.Left = RotateLeft(pivot);
            curr = RotateRight(curr);
            return curr;
        }

        private Node RotateRightLeft(Node curr)
        {
            Node pivot = curr.Right;
            curr.Right = RotateRight(pivot);
            curr = RotateLeft(curr);
            return curr;
        }

        private Node RotateRight(Node curr)
        {
            Node pivot = curr.Left;
            curr.Left = pivot.Right;
            pivot.Right = curr;
            return pivot;
        }

        private int GetBalanceFactor(Node node)
        {
            int l = GetHeight(node.Left);
            int r = GetHeight(node.Right);
            int balance = l - r;
            return balance;
        }

        private int GetHeight(Node node)
        {
            int height = 0;
            if (node != null)
            {
                int l = GetHeight(node.Left);
                int r = GetHeight(node.Right);
                int m = l > r ? l : r;
                height = m + 1;
            }
            return height;
        }

        public void Traverse()
        {
            Traverse(Root);
        }

        public delegate void DisplayTower(TKey val);

        public void DisplayAll(DisplayTower displayMethod)
        {

        }

        private void Traverse(Node node)
        {
            if (node == null) return;
            Traverse(node.Left);
            //WriteLine(node.Key + " " + node.value);
            Traverse(node.Right);
        }

    }
}
