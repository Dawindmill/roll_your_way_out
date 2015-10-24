using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Project
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
        public static byte WALL = 1;
        public static byte ROAD = 0;

        public Node startPoint;
        public Node destPoint;

        static int startAndDestDist;

        public List<Node> edgePoints = null;

        //size == dimension * dimension
        public RandomMaze(int dimension, int seed)
        {
            //atleast be half dimension apart
            startAndDestDist = dimension;
            astar = new AstarSearch();
            this.gridNeedToFill = (int)(dimension * dimension * roadPercentage);
            this.dimension=dimension;
            this.seed = seed;
            random = new Random(seed);
            random.Next(Int32.MaxValue);
            maze = new float[dimension,dimension];
            astar.initailize2DArrayToValue(maze, WALL);
            //setStartPointAndDestPoint(dimension);
            startPoint = null;
            destPoint = null;
        }

        public void setStartPointAndDestPoint()
        {
            //List<Node> edgePoints;
            List<Node> path = null;
            edgePoints = getEdgePoints(dimension);
            List<Node> edgePointsTemp = new List<Node>();
            

            while (path==null)
            {
                destPoint = null;
                if (edgePoints.Count == 0)
                {
                    break;
                }

                if (startPoint != null && destPoint != null)
                {
                    //reset to wall
                    maze[startPoint.y, startPoint.x] = WALL;
                    maze[destPoint.y, destPoint.x] = WALL;

                }
                startPoint = edgePoints.ElementAt(randomIndex(edgePoints.Count));
                edgePoints.Remove(startPoint);
                //while (true)
                //{
                    //edgePoints.Clear();
                    //edgePointsTemp.AddRange(edgePoints);
                    destPoint = generatePossibleDestPoint(edgePoints, startPoint);
                    if (destPoint == null)
                    {
                        return;
                    }

                    edgePointsTemp.Remove(destPoint);
                    maze[startPoint.y, startPoint.x] = ROAD;
                    maze[destPoint.y, destPoint.x] = ROAD;
                    path=astar.FindPath(astar.Float2DtoInt(maze), (int)WALL, startPoint.x, startPoint.y, destPoint.x, destPoint.y);
                    if (path == null)
                    {
                        maze[startPoint.y, startPoint.x] = WALL;
                        maze[destPoint.y, destPoint.x] = WALL;
                    }
                //Console.WriteLine("find possible start end.");
                //}
            }

            if (path == null)
            {
                //Console.WriteLine("maze has no start and dest point abort program ");
                //System.Environment.Exit(1);
                return;
            }

            //set path
            

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
                //Console.WriteLine("generatePossibleDestPoint => edgePoints empty.");
            }
            tempPoint  = edgePoints.ElementAt(randomIndex(edgePoints.Count));
            
            return new Node(tempPoint.x,tempPoint.y);
        }

        public Node generatePossibleDestPoint(List<Node> edgePoints, Node startPoint)
        {
            Node destPoint = null;
            Node tempDestPoint = null;
            if (edgePoints.Count == 0 || edgePoints == null)
            {
                //Console.WriteLine("generatePossibleDestPoint => no edge points can be chosen.");
                return destPoint;
            }

            while (true)
            {
                if (edgePoints.Count == 0 || edgePoints == null)
                {
                    break;
                    //Console.WriteLine("generatePossibleDestPoint => edgePoints is empty");
                }
                tempDestPoint = edgePoints.ElementAt(randomIndex(edgePoints.Count));
                edgePoints.Remove(tempDestPoint);
                if (Math.Sqrt(Math.Pow(Math.Abs(startPoint.x - tempDestPoint.x),2)+ Math.Pow(Math.Abs(startPoint.y - tempDestPoint.y),2)) >= startAndDestDist)
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
            //Console.WriteLine("----- Printing Current Maze -----");
            astar.print2Darray(maze);
        }

        //use dimention/2 points at the edges to draw map .
        //numberEdge points exceeded edge list will be ignored
        public void GenerateMazeWithRandomPoint(int numberEdgePoints,int dumbNumber,int smartNumber)
        {
            //path from begginning x y to ending x y
            updateMazeRoad(generateDumbPathToDestPoint(startX,startY,destX, destY, dumbNumber, smartNumber));

            //Console.WriteLine("dest X => "+destX+"dest y => "+destY);
 //           printMaze();
            Node otherStart = null;
            Node otherDest = null;
            while (numberEdgePoints > 0&&edgePoints.Count>1)
            {
                numberEdgePoints--;
                otherStart = GeneratePossibleDestPoint(edgePoints);
                otherDest = GeneratePossibleDestPoint(edgePoints);
                updateMazeRoad(generateDumbPathToDestPoint(otherStart.x, otherStart.y, otherDest.x, otherDest.y, dumbNumber, smartNumber));
            }
            
        }





        public void GenerateMazeWithDestBranch(int dumbNumber, int smartNumber)
        {


            //draw a path from source to 
            List<Node> roads = null;
            roads = generateDumbPathToDestPoint(startX, startY, destX, destY, dumbNumber, smartNumber);
            updateMazeRoad(roads);
            printMaze();
            List<Node> fourNeighbors;

            Node currentPoint = roads.First();

            int branchLength;


            while (roads.Count>0)
            {
                branchLength = randomIndex(dimension);
                currentPoint = roads.ElementAt(randomIndex(roads.Count));

                //dont need to remove current in this position is not been added yet.
                roads.Remove(currentPoint);

                if (astar.CalManhattenDistance(currentPoint.x, currentPoint.y, destX, destY) <= 2)
                {
                    continue;
                }

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
            List<Node> roads = new List<Node>();

            List<Node> fourNeighbors;

            //generae maze start at 1,1
            Node currentPoint = new Node(1, 1);


            Node tempNode = null;
            //Node tempCurrentPoint = null;

            maze[currentPoint.y, currentPoint.x]=ROAD;
            byte[,] visited = new byte[dimension,dimension];
            astar.initailize2DArrayToValue(visited,NOT_VISITED);
           
            while(true)
            {
                visited[currentPoint.y, currentPoint.x] = VISITED;
                //Console.WriteLine(currentPoint.x + ", " + currentPoint.y+" road len =>"+roads.Count);
                if (currentPoint.childNodes == null)
                {
                    //Console.WriteLine("seek children");
                    fourNeighbors = getFourNeighbors2(currentPoint.x, currentPoint.y, dimension, maze,visited);
                    currentPoint.childNodes = fourNeighbors;
                }
                
                if (currentPoint.childNodes == null ||currentPoint.childNodes.Count==0)
                {
                    //Console.WriteLine("no valid children");
                    if (roads.Count <= 1)
                    {
                        break;
                    }
                    tempNode = currentPoint;
                    
                    //dont need to remove current in this position is not been added yet.
                    roads.Remove(currentPoint);
                    currentPoint = roads.Last();
                    //currentPoint.childNodes.Remove(tempNode);
                    tempNode.freeNode();
                    
                    continue;
                }
                maze[currentPoint.y, currentPoint.x] = ROAD;
                //Console.WriteLine("remove current Point children");
                tempNode = currentPoint.childNodes.ElementAt(randomIndex(currentPoint.childNodes.Count));
                currentPoint.childNodes.Remove(tempNode);
                //Console.WriteLine("add current Point");
                roads.Add(currentPoint);
                currentPoint = tempNode;

            }

            
            //byte[,] visitedPositions= new byte[dimension,dimension];
            //astar.initailize2DArrayToValue(visitedPositions,NOT_VISITED);
            
            
        }
        //after every dumbTimes it will make one right movement closer to Dest
        public List<Node> generateDumbPathToDestPoint(int startX,int startY,int destX,int destY,int dumbTimes,int smartTimes)
        {
            float[,] visited = new float[dimension, dimension];
            astar.initailize2DArrayToValue(maze, WALL);
            Node tempNode = null;
            Node tempCurrentPoint =null;
            Node currentPoint = new Node(startX,startY);
            List<Node> dumbPath = new List<Node>();
            List<Node> fourNeighbors = new List<Node>();
            int currentDumbTimes = dumbTimes;
            int currentSmartTimes = 0;
            while(true)
            {
                //visited[currentPoint.y, currentPoint.x] = VISITED;
                //road == visited 
                visited[currentPoint.y, currentPoint.x] = ROAD;
                fourNeighbors = getFourNeighbors(currentPoint.x,currentPoint.y,dimension,maze);
                

                //need to go to previous node .if no neibours 
                if(currentPoint.x==destX&&currentPoint.y==destY)
                {
                    //Console.WriteLine("generateDumbPathToDestPoint => found dest");
                    dumbPath.Insert(0, currentPoint);
                    break;
                }

                if(fourNeighbors==null)
                {
                   // Console.WriteLine("generateDumbPathToDestPoint => neighbors null");
                    if(dumbPath.Count==0)
                    {
                     //   Console.WriteLine("generateDumbPathToDestPoint => dumbPath 0");
                        break;
                    }
                    tempCurrentPoint = currentPoint;
                    currentPoint=dumbPath.First();
                    dumbPath.Remove(currentPoint);
                    tempCurrentPoint.freeNode();
                    continue;
                }

                astar.FillManhattenDistance(fourNeighbors, destX, destY);

               
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
                //Console.WriteLine(" generateDumbPathToDestPoint => fail to find dumb path to goal point. ");
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

        public void updateMazeRoad(List<Node> path,float goal)
        {
            foreach (var node in path)
            {
                maze[node.y, node.x] = goal;
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
            //Console.WriteLine("----- Printing path in tempMaze -----");
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


        public List<Node> getFourNeighbors2(int startX, int startY, int dimension, float[,] maze,byte[,]visited)
        {
            List<Node> validNeighborNodes = new List<Node>();

            //top left right bottom
            int[] fourDirectionCol = { 0, -1, 1, 0 };
            int[] fourDirectionRow = { -1, 0, 0, 1 };
            int tempCol;
            int tempRow;
            int connectRoadCount=0;
            for (int i = 0; i < fourDirectionCol.Length; i++)
            {
                tempCol = startX + fourDirectionCol[i];
                tempRow = startY + fourDirectionRow[i];

                if (tempCol >= dimension-1 ||
                    tempCol < 1 ||
                    tempRow >= dimension-1 ||
                    tempRow < 1)
                {
                    continue;
                }
                //can
                if(maze[tempRow, tempCol] == ROAD)
                {
                    connectRoadCount+=1;
                    //input cell is connecting roads.
                    if (connectRoadCount >= 2)
                    {
                        return null;
                    }
                    continue;
                }

                if (visited[tempRow, tempCol] == VISITED)
                {
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
