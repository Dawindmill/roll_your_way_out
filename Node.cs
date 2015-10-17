using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class Node
    {
        public int x;
        public int y;
        public Node parent;
        public List<Node> childNodes;
        public int manhattenDistanceToGoal;
        public static int NOT_CALCULATED=-1;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Node(int x, int y, Node parent)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
            manhattenDistanceToGoal = NOT_CALCULATED;
            childNodes = null;
        }

        public void addChildNodes(List<Node> childNodes)
        {
            this.childNodes = childNodes;
        }

        public void freeNode()
        {
            parent = null;
            childNodes = null;
        }

        public override string ToString()
        {
            return " Node x => " + x+" Node y => "+y ;
        }


        public void removeChildNode(Node childNode)
        {
            if (childNodes == null)
            {
                Console.WriteLine("removeChildNode => childNodes is empty.");
            }

            childNodes.Remove(childNode);
        }

        public Node LeastManhattenChildNode()
        {
            Node tempNode=null;

            if(childNodes==null)
            {
                Console.Write("LeastManhattenNode childNode is empty");
                return tempNode;
            }

            foreach (var oneNode in childNodes)
            {
                if (tempNode == null)
                {
                    tempNode = oneNode;
                    continue;
                }

                if (tempNode.manhattenDistanceToGoal <= oneNode.manhattenDistanceToGoal)
                {
                    continue;
                }
                else
                {
                    tempNode = oneNode;
                }
            
            }
            return tempNode;
        }
    }





}
