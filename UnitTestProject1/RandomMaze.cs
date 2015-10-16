using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class RandomMaze
    {
        Random random;
        int seed;
        int dimension;

        //what percentage of the map will be filled with roads 
        float roadPercentage;
        int gridNeedToFill;
        public float[,] maze;
        public AstarSearch astar;
        int startX;
        int startY;

        public int destX;
        public int destY;

        static byte NOT_VISITED = 0;
        static byte VISITED = 1;
        //initially all WALL and knock down wall 
        static byte WALL = 1;
        static byte ROAD = 0;

        public Node startPoint;
        public Node destPoint;

        static int startAndDestDist;

        public List<Node> edgePoints = null;

        //size == dimension * dimension
        public RandomMaze(int dimension, int seed,float roadPercentage)
        {
            //atleast be half dimension apart
            startAndDestDist = dimension / 2;
            astar = new AstarSearch();
            this.gridNeedToFill = (int)(dimension * dimension * roadPercentage);
            this.dimension=dimension;
            this.roadPercentage = roadPercentage;
            this.seed = seed;
            random = new Random(seed);
            random.Next(Int32.MaxValue);
            maze = new float[dimension,dimension];
            astar.initailize2DArrayToValue(maze, WALL);
            setStartPointAndDestPoint(dimension);
        }

        public void setStartPointAndDestPoint(int dimension)
        {
            //List<Node> edgePoints;
            edgePoints = getEdgePoints(dimension);
            startPoint = null;
            destPoint = null;
          
            startPoint = edgePoints.ElementAt(random.Next() % edgePoints.Count);
            Console.WriteLine("startPoint => " + startPoint);

            edgePoints.Remove(startPoint);
            destPoint=generatePossibleDestPoint(edgePoints,startPoint);
            Console.WriteLine("destPoint => " + destPoint);

            startX = startPoint.x;
            startY = startPoint.y;
            destX = destPoint.x;
            destY = destPoint.y;
        }

        public Node GeneratePossibleDestPoint(List<Node> edgePoints)
        {
            Node tempPoint = null;
            if (edgePoints == null || edgePoints.Count == 0)
            {
                Console.WriteLine("generatePossibleDestPoint => edgePoints empty.");
            }
            tempPoint  = edgePoints.ElementAt(randomIndex(edgePoints.Count));
            edgePoints.Remove(tempPoint);
            return new Node(tempPoint.x,tempPoint.y);
        }

        public Node generatePossibleDestPoint(List<Node> edgePoints, Node startPoint)
        {
            Node destPoint = null;
            Node tempDestPoint = null;
            if (edgePoints.Count == 0 || edgePoints == null)
            {
                Console.WriteLine("generatePossibleDestPoint => no edge points can be chosen.");
                return destPoint;
            }

            while (true)
            {
                if (edgePoints.Count == 0 || edgePoints == null)
                {
                    Console.WriteLine("generatePossibleDestPoint => edgePoints is empty");
                }
                tempDestPoint = edgePoints.ElementAt(randomIndex(edgePoints.Count));
                edgePoints.Remove(tempDestPoint);
                if (Math.Abs(startPoint.x - tempDestPoint.x) >= startAndDestDist ||
                    Math.Abs(startPoint.y - tempDestPoint.y)>=startAndDestDist)
                {
                    destPoint = tempDestPoint;
                    break;
                }
            }

            return destPoint;
        }

        public int randomIndex(int count)
        {
            return random.Next() % count;
        }

        public void printMaze()
        {
            Console.WriteLine("----- Printing Current Maze -----");
            astar.print2Darray(maze);
        }

        //use dimention/2 points at the edges to draw map .
        //numberEdge points exceeded edge list will be ignored
        public void GenerateMazeWithRandomPoint(int numberEdgePoints,int dumbNumber,int smartNumber)
        {
            //path from begginning x y to ending x y
            generateDumbPathToDestPoint(destX, destY, dumbNumber, smartNumber);
            Node otherDest = null;
            while (numberEdgePoints > 0&&edgePoints.Count>0)
            {
                numberEdgePoints--;
                otherDest = GeneratePossibleDestPoint(edgePoints);
                updateMazeRoad(generateDumbPathToDestPoint(otherDest.x, otherDest.y, dumbNumber, smartNumber));
            }
            
        }

        public void GenerateMazeWithDestBranch(int dumbNumber, int smartNumber)
        {


            //draw a path from source to 
            List<Node> roads = null;
            roads = generateDumbPathToDestPoint(destX, destY, dumbNumber, smartNumber);
            List<Node> fourNeighbors;

            Node currentPoint = roads.First();

            int branchLength;


            while (roads.Count>0)
            {
                branchLength = randomIndex(dimension);
                currentPoint = roads.ElementAt(randomIndex(roads.Count));
                //dont need to remove current in this position is not been added yet.
                roads.Remove(currentPoint);

                while (branchLength > 0)
                {
                    branchLength--;
                    maze[currentPoint.y, currentPoint.x] = ROAD;

                    fourNeighbors = getFourNeighbors(currentPoint.x, currentPoint.y, dimension, maze);

                    if (fourNeighbors == null)
                    {
                        
                        break;
                    }
                    currentPoint = fourNeighbors.ElementAt(randomIndex(fourNeighbors.Count));
                }
                currentPoint.freeNode();
            }
        }

        //assign a random length then pop random node from queue ? 

        public void GenerateMaze()
        {
            //some point at the edges
            //may change to queue later, may try not just pop out first one but randome one.

            //List<Node> queue;
            int dumbSmartNum = dimension / 10 * 2;
            Console.WriteLine("ndumbSmartNum - > " + dumbSmartNum);
            if (dumbSmartNum < 0)
            {
                Console.WriteLine("GenerateMaze => dumbSmart num < 0 impossible to generate maze.");
                System.Environment.Exit(1);
            }

            //draw a path from source to 
            List<Node> roads = null;
            roads=generateDumbPathToDestPoint(destX,destY,dumbSmartNum, dumbSmartNum);
            List<Node> fourNeighbors;

            Node tempNode = null;
            Node tempCurrentPoint = null;
            Node currentPoint = roads.First();

            int numberRoadsLeft = (int)(roadPercentage*(dimension * dimension));
            Console.WriteLine("numberRoadsLeft left  before minus-> " + numberRoadsLeft);
            numberRoadsLeft = numberRoadsLeft - roads.Count;
            //start from destX again . and it haven't got children yet
            numberRoadsLeft += 1;

            Console.WriteLine("Number roads left -> "+numberRoadsLeft);
           
            while(numberRoadsLeft>0)
            {
                maze[currentPoint.y, currentPoint.x] = ROAD;
                
                fourNeighbors = getFourNeighbors(currentPoint.x, currentPoint.y, dimension, maze);

                if (fourNeighbors == null)
                {
                    Console.WriteLine("GenerateMaze => neighbors null");
                    if (roads.Count == 0)
                    {
                        Console.WriteLine("GenerateMaze => dumbPath 0");
                        break;
                    }
                    tempCurrentPoint = currentPoint;
                    currentPoint = roads.ElementAt(randomIndex(roads.Count));
                    //dont need to remove current in this position is not been added yet.
                    roads.Remove(tempCurrentPoint);
                    tempCurrentPoint.freeNode();
                    continue;
                }

                currentPoint.childNodes = fourNeighbors;
                roads.Insert(0, currentPoint);
                numberRoadsLeft--;
                currentPoint = currentPoint.childNodes.ElementAt(randomIndex(currentPoint.childNodes.Count));

            }

            
            //byte[,] visitedPositions= new byte[dimension,dimension];
            //astar.initailize2DArrayToValue(visitedPositions,NOT_VISITED);
            
            
        }
        //after every dumbTimes it will make one right movement closer to Dest
        public List<Node> generateDumbPathToDestPoint(int destX,int destY,int dumbTimes,int smartTimes)
        {
            float[,] maze = new float[dimension, dimension];
            astar.initailize2DArrayToValue(maze, WALL);
            Node tempNode = null;
            Node tempCurrentPoint =null;
            Node currentPoint = new Node(startX,startY,null);
            List<Node> dumbPath = new List<Node>();
            List<Node> fourNeighbors = new List<Node>();
            int currentDumbTimes = dumbTimes;
            int currentSmartTimes = 0;
            while(true)
            {
                Console.WriteLine("generateDumbPathToDestPoint => current.x = "+
                    currentPoint.x+
                    " current.y = "+
                    currentPoint.y);
                //visited[currentPoint.y, currentPoint.x] = VISITED;
                maze[currentPoint.y, currentPoint.x] = ROAD;
                fourNeighbors = getFourNeighbors(currentPoint.x,currentPoint.y,dimension,maze);
                astar.FillManhattenDistance(fourNeighbors, destX, destY);
                //need to go to previous node .if no neibours 
                if(currentPoint.x==destX&&currentPoint.y==destY)
                {
                    Console.WriteLine("generateDumbPathToDestPoint => found dest");
                    dumbPath.Insert(0, currentPoint);
                    break;
                }

                if(fourNeighbors==null)
                {
                    Console.WriteLine("generateDumbPathToDestPoint => neighbors null");
                    if(dumbPath.Count==0)
                    {
                        Console.WriteLine("generateDumbPathToDestPoint => dumbPath 0");
                        break;
                    }
                    tempCurrentPoint = currentPoint;
                    currentPoint=dumbPath.First();
                    dumbPath.Remove(currentPoint);
                    tempCurrentPoint.freeNode();
                    continue;
                }
                //addind dumb path

                if(currentSmartTimes==0&&currentDumbTimes==0)
                {
                    currentSmartTimes = smartTimes;
                    currentDumbTimes=dumbTimes;
                }
                if (currentSmartTimes != 0)
                {
                    currentSmartTimes--;
                    currentPoint.childNodes = fourNeighbors;
                    dumbPath.Insert(0, currentPoint);
                    tempNode = currentPoint.LeastManhattenChildNode();
                    currentPoint.childNodes.Remove(tempNode);
                    currentPoint = tempNode;
                    if (currentSmartTimes == 0)
                    {
                        currentDumbTimes = dumbTimes;
                    }
                    continue;
                
                }

                if(currentDumbTimes!=0)
                {
                    currentDumbTimes--;
                    currentPoint.childNodes = fourNeighbors;
                    dumbPath.Insert(0,currentPoint);
                    tempNode = currentPoint.childNodes.ElementAt(randomIndex(currentPoint.childNodes.Count));
                    currentPoint.childNodes.Remove(tempNode);
                    currentPoint = tempNode;
                    if (currentDumbTimes == 0)
                    {
                        currentSmartTimes = smartTimes;
                    }
                    continue;
                }
                
                
                
            }

            if(dumbPath.Count==0||dumbPath==null)
            {
                Console.WriteLine(" generateDumbPathToDestPoint => fail to find dumb path to goal point. ");
            }


            return dumbPath;

        }

        public void updateMazeRoad(List<Node> path)
        {
            foreach (var node in path)
            {
                maze[node.y, node.x] = ROAD;
            }
        }

        public void printPathInMaze(List<Node> path)
        { 
            float[,] tempMaze = new float[dimension,dimension];
            astar.initailize2DArrayToValue(tempMaze, WALL);
            foreach (var node in path)
            {
                tempMaze[node.y, node.x] = ROAD;
            }
            Console.WriteLine("----- Printing path in tempMaze -----");
            astar.print2Darray(tempMaze);
        }

        public List<Node> getFourNeighbors(int startX,int startY,int dimension, float[,]visited)
        {
            List<Node> validNeighborNodes = new List<Node>();

            //top left right bottom
            int[] fourDirectionCol = { 0, -1, 1, 0 };
            int[] fourDirectionRow = { -1, 0, 0, 1 };
            int tempCol;
            int tempRow;
            for (int i = 0; i < fourDirectionCol.Length; i++)
            {
                tempCol = startX + fourDirectionCol[i];
                tempRow = startY + fourDirectionRow[i];

                if (tempCol >= dimension ||
                    tempCol < 0 ||
                    tempRow >= dimension ||
                    tempRow < 0 ||
                    visited[tempRow, tempCol] ==ROAD)
                {
//                    Console.WriteLine(" getFourNeighbors - tempCol => " +
//                        tempCol +
//                        " tempRow => " +
//                        tempRow + " worldDim => " +
//                        dimension);
                    continue;
                }
                //public Node(int x, int y, Node parent)
                //Console.WriteLine("here");
                validNeighborNodes.Add(new Node(tempCol, tempRow));
            }
            if (validNeighborNodes.Count == 0)
            {
                return null;
            }
            return validNeighborNodes;
        }

        public List<Node> getEdgePoints(int dimension)
        {
            List<Node> edgePoints = new List<Node>();

            for(int row=0;row<dimension;row++)
            {
                for(int col=0;col<dimension;col++)
                {
                    if ((row != 0 || row != dimension - 1)&&(col==0||col==dimension-1))
                    {
                        edgePoints.Add(new Node(col,row));
                    }

                    if (row == 0 || row == dimension - 1)
                    {
                        edgePoints.Add(new Node(col, row));
                    }
                }
            }

            return edgePoints;
        
        }

        //TODO
        //public float[,] scaleTo4Point(float[,] maze)
    }
}
