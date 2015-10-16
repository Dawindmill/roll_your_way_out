using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1
{
    class Cube
    {
        Vector3 frontNormal = new Vector3(0, 0, 1);

        Vector3 frontBottomLeft = new Vector3(-1.0f, -1.0f, -1.0f);
        Vector3 frontTopLeft = new Vector3(-1.0f, 1.0f, -1.0f);
        Vector3 frontTopRight = new Vector3(1.0f, 1.0f, -1.0f);
        Vector3 frontBottomRight = new Vector3(1.0f, -1.0f, -1.0f);
        Vector3 backBottomLeft = new Vector3(-1.0f, -1.0f, 1.0f);
        Vector3 backBottomRight = new Vector3(1.0f, -1.0f, 1.0f);
        Vector3 backTopLeft = new Vector3(-1.0f, 1.0f, 1.0f);
        Vector3 backTopRight = new Vector3(1.0f, 1.0f, 1.0f);

        Vector3 frontBottomLeftNormal = new Vector3(-0.333f, -0.333f, -0.333f);
        Vector3 frontTopLeftNormal = new Vector3(-0.333f, 0.333f, -0.333f);
        Vector3 frontTopRightNormal = new Vector3(0.333f, 0.333f, -0.333f);
        Vector3 frontBottomRightNormal = new Vector3(0.333f, -0.333f, -0.333f);
        Vector3 backBottomLeftNormal = new Vector3(-0.333f, -0.333f, 0.333f);
        Vector3 backBottomRightNormal = new Vector3(0.333f, -0.333f, 0.333f);
        Vector3 backTopLeftNormal = new Vector3(-0.333f, 0.333f, 0.333f);
        Vector3 backTopRightNormal = new Vector3(0.333f, 0.333f, 0.333f);
        float wall=1;
        float road=0;
        float goal = 2;


        //assume square maze
        public VertexPositionNormalColor[] DrawMazeWithCube(float[,] maze,int scale)
        {
            VertexPositionNormalColor[] sampleCube = GenerateScaledPolygon(GetUnitCube(), scale);
            VertexPositionNormalColor[] sampleRoad = GenerateScaledPolygon(GetUnitCubeFloor(Color.Green), scale);
            int dimension = maze.GetLength(0);
            int cubeNumVertex = sampleCube.Length;
            VertexPositionNormalColor[] mazeCubes = new VertexPositionNormalColor[dimension * dimension*cubeNumVertex];
            VertexPositionNormalColor[] tempCube;
            VertexPositionNormalColor[] tempRoad;
            int countIndex = 0;
            for (int row = 0; row < dimension; row++)
            {
                for (int col = 0; col < dimension; col++)
                { 
                    if(maze[row,col]==wall)
                    {
                        tempCube = (VertexPositionNormalColor[])sampleCube.Clone();
                        tempCube = TranslatePolygonInXY(tempCube, new Vector3((float)scale * 2 * col, (float)scale * 2 * row, 0.0f));
                        for (int i = 0; i < tempCube.Length; i++)
                        {
                            mazeCubes[countIndex++] = tempCube[i];
                        }
                    }
                    if (maze[row, col] == goal)
                    {
                        tempRoad = GenerateScaledPolygon(GetUnitCubeFloor(Color.Purple), scale);
                        tempRoad = TranslatePolygonInXY(tempRoad, new Vector3((float)scale * 2 * col, (float)scale * 2 * row, 0.0f));
                        for (int i = 0; i < tempRoad.Length; i++)
                        {
                            mazeCubes[countIndex++] = tempRoad[i];
                        }
                    }

                    if (maze[row, col] == road)
                    {
                        tempRoad = (VertexPositionNormalColor[])sampleRoad.Clone();
                        tempRoad = TranslatePolygonInXY(tempRoad, new Vector3((float)scale * 2 * col, (float)scale * 2 * row, 0.0f));
                        for (int i = 0; i < tempRoad.Length; i++)
                        {
                            mazeCubes[countIndex++] = tempRoad[i];
                        }
                    }
                }
            }
            return mazeCubes;    
        }

        public VertexPositionNormalColor[] GenerateScaledPolygon(VertexPositionNormalColor[] unitCube, int scale)
        {
            for (int i = 0; i < unitCube.Length; i++)
            {
                unitCube[i].Position = unitCube[i].Position * scale;
            }

            return unitCube;
        }

        public VertexPositionNormalColor[] TranslatePolygonInXY(VertexPositionNormalColor[] cube,Vector3 translation)
        {
            for (int i = 0; i < cube.Length; i++)
            {
                cube[i].Position = cube[i].Position+translation;

            }

            return cube;
        }

        public VertexPositionNormalColor[] GetUnitCubeFloor(Color color)
        { 
         return new VertexPositionNormalColor[]{
             new VertexPositionNormalColor(frontTopRight, new Vector3(0,0,-1), color),
            
                    new VertexPositionNormalColor(frontTopLeft, new Vector3(0,0,-1),color),
                     new VertexPositionNormalColor(frontBottomLeft, new Vector3(0,0,-1), color), // Front
                    

                      new VertexPositionNormalColor(frontBottomRight, new Vector3(0,0,-1), color),
                   
                    new VertexPositionNormalColor(frontTopRight, new Vector3(0,0,-1), color),
                     new VertexPositionNormalColor(frontBottomLeft, new Vector3(0,0,-1), color),
                   };
        }
        
        public VertexPositionNormalColor[] GetUnitCube()
        {
            return new VertexPositionNormalColor[]{
             new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, Color.Purple), // Front
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, Color.Purple),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, Color.Purple),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, Color.Purple),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, Color.Purple),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, Color.Purple),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, Color.Orange), // BACK
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, Color.Orange),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, Color.Orange),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, Color.Orange),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, Color.Orange),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, Color.Orange),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, Color.OrangeRed), // Top
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, Color.Blue), // Bottom
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, Color.Blue),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, Color.Blue),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, Color.Blue),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, Color.Blue),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, Color.Blue),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, Color.DarkOrange), // Left
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, Color.DarkOrange), // Right
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, Color.DarkOrange),};
        }
        
    }
}
