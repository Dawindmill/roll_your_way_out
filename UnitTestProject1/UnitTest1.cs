using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;

namespace Project
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            AstarSearch astar = new AstarSearch();
            List<Node> path = null;
            int srcX=1;
            int srcY=4;
            int destX =4;
            int destY =4;
            int limit=1;
            int[,] world ={
                          {0,0,0,0,0},
                          {1,0,1,1,0},
                          {0,0,1,0,0},
                          {0,1,1,1,0},
                          {0,0,1,1,0}
                          };
            path = astar.FindPath(world,limit,srcX,srcY,destX,destY);

            astar.printPath(path);

            Assert.IsNotNull(path);

        }

        [TestMethod]
        public void MazeTest()
        {
            AstarSearch astar = new AstarSearch();
            int dimension = 20;
            int dumbSmart = dimension / 10 * 2;
            int seed = 34534;
            List<Node> path = null;
            RandomMaze maze = new RandomMaze(dimension,seed,0.85f);
            path = maze.generateDumbPathToDestPoint(maze.destX,maze.destY,dumbSmart, dumbSmart);
            Console.WriteLine(" -- path count " + path.Count);
            maze.astar.printPath(path);
            maze.updateMazeRoad(path);
            maze.printMaze();
            Console.WriteLine("-----------------astar---------------------");
            astar.printPath(astar.FindPath(astar.Float2DtoInt(maze.maze),1,maze.startPoint.x,maze.startPoint.y,maze.destPoint.x,
                maze.destPoint.y));

        }

        [TestMethod]
        public void MazeGenerationTest()
        {
            int dimension = 100;
            int dumbSmart = dimension / 10 * 3;
            int seed = 3122;
            
            RandomMaze maze = new RandomMaze(dimension, seed, 0.50f);

            Node destPoint1 = maze.generatePossibleDestPoint(maze.edgePoints,maze.startPoint);

            Console.Write("destPoint 1 + " + destPoint1);

            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(0, 0, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(100, 100, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(5, 19, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(10, 19, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(19, 5, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(0,0, dumbSmart, dumbSmart));

            maze.GenerateMazeWithRandomPoint(20,1,1);
            maze.printMaze();
            
        }
        [TestMethod]
        public void MazeGenerationTest2()
        {
            int dimension = 100;
            int dumbSmart = dimension / 10 * 3;
            int seed = 3122;

            RandomMaze maze = new RandomMaze(dimension, seed, 0.50f);

            Node destPoint1 = maze.generatePossibleDestPoint(maze.edgePoints, maze.startPoint);

            Console.Write("destPoint 1 + " + destPoint1);

            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(0, 0, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(100, 100, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(5, 19, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(10, 19, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(19, 5, 1, 1));
            //maze.updateMazeRoad(maze.generateDumbPathToDestPoint(0,0, dumbSmart, dumbSmart));

            maze.GenerateMazeWithRandomPoint(50, 1, 1);
            maze.printMaze();
        }

        [TestMethod]
        public void MazeGenerationTest3()
        {
            int dimension = 100;
            int dumbSmart = dimension / 10 * 3;
            int seed = 3122;

            RandomMaze maze = new RandomMaze(dimension, seed, 0.50f);

            Node destPoint1 = maze.generatePossibleDestPoint(maze.edgePoints, maze.startPoint);


            maze.GenerateMazeWithDestBranch(1, 1);
            maze.printMaze();
        }

        [TestMethod]
        public void generalTest()
        {
            Console.WriteLine(" answer => " + (int)3.2/4);
        }


    }
}
