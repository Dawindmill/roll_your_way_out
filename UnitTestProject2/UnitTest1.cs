using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RandomMaze maze; 
            maze = new RandomMaze(15, 9);
            //cube = new Cube();
            maze.GenerateMaze();

            maze.printMaze();

            maze.setStartPointAndDestPoint();

            maze.printMaze();
        }
    }
}
