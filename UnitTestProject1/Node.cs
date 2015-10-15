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
        static int NOT_CALCULATED=-1;
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

    }



}
