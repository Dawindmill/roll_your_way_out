using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    class AstarSearch
    {
        static int VISITED = 1;
        static int NOT_VISITED = 0;
        AstarSearch()
        {
        }
        //return the list of nodes which formed the parth from src to dest. 
        //in world bigger or equal to limit means can't move to there 
        //manhatten distance used in here 
        public List<Node> FindPath(int[,] world, int limit, int srcX, int srcY, int destX, int destY)
        {
            // int[row,col];
            int worldDimension = world.Length;
            //1 means visited . 0 means not visited 
            byte[,] visitedPositions = new byte[worldDimension, worldDimension];
            Node headNode = new Node(srcX,srcY,null);
            headNode.manhattenDistanceToGoal = CalManhattenDistance(headNode.x, headNode.y, destX, destY);
            Node tempNode = headNode;
            List<Node> neighborNodes = null;
            List<Node> path =null;
            while (true)
            {
                if (tempNode == null||(tempNode.x==destX&&tempNode.y==destY))
                {
                    break;
                }

                
            }
            return path;
        }
        //less manhatten distance == closer
        public int CalManhattenDistance(int srcX,int srcY, int destX, int destY)
        {
            return Math.Abs(srcX - destX) + Math.Abs(srcY - destY);
        }

        public List<Node> FillManhattenDistance(List<Node> nodes,int destX,int destY)
        {
            if (nodes == null)
            {
                Console.Write("FillManhattenDistance => input List<MNode> is empty . ");
                return null;
            }
            foreach(var oneNode in nodes)
            {
                oneNode.manhattenDistanceToGoal = CalManhattenDistance(oneNode.x, oneNode.y, destX, destY);
            }
            return nodes;
        }
        public List<Node> GetValidNeighborNodes(Node start, int limit, int worldDimention, int[,] world, byte[,] visisted)
        {
            List<Node> validNeighborNodes = new List<Node>();

            //0,0 is from top left 
            //x = col , y =row ;
            int startX = start.x;
            int startY = start.y;
            int[] eightDirectionCol = { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
            int[] eightDirectionRow = { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
            int tempCol;
            int tempRow;
            for (int i = 0; i < eightDirectionCol.Length; i++)
            {
                tempCol = startX + eightDirectionCol[i];
                tempRow = startY + eightDirectionRow[i];
                if(tempCol>=worldDimention||
                    tempCol<0||
                    tempRow>=worldDimention||
                    tempRow<0||
                    visisted[tempRow,tempCol]==VISITED||
                    world[tempRow,tempCol]>=limit)
                {
                    continue;
                }
                //public Node(int x, int y, Node parent)
                validNeighborNodes.Add(new Node(tempCol,tempRow,start));
            }
            if (validNeighborNodes.Count == 0)
            {
                return null;
            }
            return validNeighborNodes;
        }


    }
}
