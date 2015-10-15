using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;

namespace UnitTestProject1
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


    }
}
