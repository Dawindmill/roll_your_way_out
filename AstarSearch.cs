using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class AstarSearch
    {
        static byte VISITED = 1;
        static byte NOT_VISITED = 0;
        public AstarSearch()
        {
        }

        public void initailize2DArrayToValue<T>(T[,] array, T value)
        {
            int rowDim=array.GetLength(0);
            int colDim=array.GetLength(1);
            for (int row = 0; row < rowDim; row++)
            {
                for (int col = 0; col < colDim; col++)
                {
                    array[row, col] = value;
                }
            }
        }
        public void print2Darray<T>(T[,] array)
        {
            int rowDim = array.GetLength(0);
            int colDim = array.GetLength(1);
            for (int row = 0; row < rowDim; row++)
            {
                for (int col = 0; col < colDim; col++)
                {
                    Console.Write("  " + array[row, col] + " ");
                }
                Console.WriteLine();
            }

        }

        public int[,] Float2DtoInt(float[,] array)
        {
            int rowDim = array.GetLength(0);
            int colDim = array.GetLength(1);
            int[,] result =new int[rowDim,colDim];
            for (int row = 0; row < rowDim; row++)
            {
                for (int col = 0; col < colDim; col++)
                {
                    result[row, col] =(int) array[row, col];
                }
            }
            return result;
        }

        public void printPath (List<Node>path)
        {
            foreach (var tempNode in path)
            {
                Console.Write(" | " + tempNode + " | ");
            }
            Console.WriteLine();
        }
        //return the list of nodes which formed the parth from src to dest. 
        //in world bigger or equal to limit means can't move to there 
        //manhatten distance used in here 
        public List<Node> FindPath(int[,] world, int limit, int srcX, int srcY, int destX, int destY)
        {
            // int[row,col];
            //row size = > world.GetLength(0)
            int worldDimension = world.GetLength(0);
            //1 means visited . 0 means not visited 
            byte[,] visitedPositions = new byte[worldDimension, worldDimension];

            //initialize visit array to not visited 
            initailize2DArrayToValue(visitedPositions,NOT_VISITED);

            Node headNode = new Node(srcX,srcY,null);
            headNode.manhattenDistanceToGoal = CalManhattenDistance(headNode.x, headNode.y, destX, destY);
            Node tempNode = headNode;
            Node removeNode = null;
            List<Node> neighborNodes = null;
            List<Node> path =null;

            //visited head
            
            while (true)
            {
                if (tempNode == null)
                {
                    path=null;
                    break;
                }

                visitedPositions[tempNode.y, tempNode.x] = VISITED;
                //Console.WriteLine("==== tempNode.y =>" + tempNode.y + " tempNode.x => " + tempNode.x + "====");
                    
                if(tempNode.x==destX&&tempNode.y==destY)
                {
                    path = BackTrackBuildPath(tempNode);
                    break;
                }

                if (tempNode.childNodes == null)
                {
                    neighborNodes = GetValidNeighborNodes(tempNode, limit, worldDimension, world, visitedPositions);
                    tempNode.childNodes = neighborNodes;
                }


                if (tempNode.childNodes == null||tempNode.childNodes.Count==0)
                {
                    
                    removeNode = tempNode;
                    tempNode = tempNode.parent;

                    if (tempNode==null)
                    {
                        break;
                    }

                    tempNode.removeChildNode(removeNode);
                    removeNode.freeNode();
                    continue;
                }

                FillManhattenDistance(tempNode.childNodes, destX, destY);

                //Console.WriteLine("childnodes Len => " + tempNode.childNodes.Count);
                tempNode = tempNode.LeastManhattenChildNode();
            }
            return path;
        }

        public List<Node> BackTrackBuildPath(Node goalNode)
        {
            int head = 0;
            List<Node> path = new List<Node>();
            Node tempNode = goalNode;
            while(tempNode.parent!=null)
            {
                path.Insert(head, tempNode);
                tempNode = tempNode.parent;
                
            }
            //last node;
            path.Insert(head,tempNode);
            

            if (path.Count == 0)
            {
                //Console.Write("BackTrackBuildPath => No Path Found.");
                return null;
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
                //Console.WriteLine("FillManhattenDistance => input List<MNode> is empty . ");
                return null;
            }
            foreach(var oneNode in nodes)
            {
                oneNode.manhattenDistanceToGoal = CalManhattenDistance(oneNode.x, oneNode.y, destX, destY);
            }
            return nodes;
        }
        //can use annother array said added[,] to track which nodes are already added as other nodes's cildren
        //so avoid adding using nodes ..
        public List<Node> GetValidNeighborNodes(Node start, int limit, int worldDimention, int[,] world, byte[,] visisted)
        {
            List<Node> validNeighborNodes = new List<Node>();

            //0,0 is from top left 
            //x = col , y =row ;
            int startX = start.x;
            int startY = start.y;
            //int[] eightDirectionCol = { -1, 0, 1, -1, 1, -1, 0, 1 };
            //int[] eightDirectionRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
            //search 4 direction instead of 8
            int[] eightDirectionCol = { 0, -1, 1, 0 };
            int[] eightDirectionRow = { -1, 0, 0, 1 };
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
                //Console.WriteLine("here");
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
