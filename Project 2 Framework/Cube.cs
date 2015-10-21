using Project;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project
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
        public static float wall=1;
        public static float road=0;
        public static float goal = 2;
        public static int CUBE_INITAIL_SIDE_LENGTH = 2;
        public int numberWallAndFloor;


        //assume square maze
        public VertexPositionNormalColor[] GetMazeVertexWithCube(float[,] maze,float scale)
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
                        tempCube = TranslatePolygonInXY(tempCube, new Vector3((float)scale * 2 * col, 0.0f, (float)scale * 2 * row));
                        for (int i = 0; i < tempCube.Length; i++)
                        {
                            mazeCubes[countIndex++] = tempCube[i];
                        }
                    }
                    /*
                    if (maze[row, col] == goal)
                    {
                        tempRoad = GenerateScaledPolygon(GetUnitCubeFloor(Color.Yellow), scale);
                        tempRoad = TranslatePolygonInXY(tempRoad, new Vector3((float)scale * 2 * col, 0.0f, (float)scale * 2 * row));
                        for (int i = 0; i < tempRoad.Length; i++)
                        {
                            mazeCubes[countIndex++] = tempRoad[i];
                        }
                    }*/
                    
                    if (maze[row, col] == road || maze[row, col] == goal)
                    {
                        tempRoad = (VertexPositionNormalColor[])sampleRoad.Clone();
                        tempRoad = TranslatePolygonInXY(tempRoad, new Vector3((float)scale * 2 * col, 0.0f, (float)scale * 2 * row));
                        for (int i = 0; i < tempRoad.Length; i++)
                        {
                            mazeCubes[countIndex++] = tempRoad[i];
                        }
                    }
                }
            }




            //know how many vertex are for wall and floor 
            numberWallAndFloor = countIndex;

            return mazeCubes;    
        }

        public VertexPositionNormalColor[] GetMazeWayOutVertexWithCube(List<Node>path, float scale,Color color)
        {
            //VertexPositionNormalColor[] sampleCube = GenerateScaledPolygon(GetUnitCube(), scale);
            VertexPositionNormalColor[] sampleRoad = GenerateScaledPolygon(GetUnitCubeFloor(Color.Green), scale);
            //int cubeNumVertex = sampleCube.Length;
            VertexPositionNormalColor[] mazeCubes = new VertexPositionNormalColor[path.Count*sampleRoad.Count()];
            VertexPositionNormalColor[] tempRoad;
            int countIndex = 0;
            foreach (var position in path)
            {
                        tempRoad = GenerateScaledPolygon(GetUnitCubeFloor(color), scale);
                        tempRoad = TranslatePolygonInXY(tempRoad, new Vector3((float)scale * 2 * position.x, 0.0f, (float)scale * 2 * position.y));
                        for (int i = 0; i < tempRoad.Length; i++)
                        {
                            mazeCubes[countIndex++] = tempRoad[i];
                        }
            }
            return mazeCubes;  
        }
        public VertexPositionNormalColor[] GenerateScaledPolygon(VertexPositionNormalColor[] unitCube, float scale)
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
                new VertexPositionNormalColor(frontBottomLeft, new Vector3(0, 1, 0), color),
                new VertexPositionNormalColor(backBottomLeft, new Vector3(0, 1, 0), color),
                new VertexPositionNormalColor(backBottomRight, new Vector3(0, 1, 0), color),
                new VertexPositionNormalColor(frontBottomLeft, new Vector3(0, 1, 0), color),
                new VertexPositionNormalColor(backBottomRight, new Vector3(0, 1, 0), color),
                new VertexPositionNormalColor(frontBottomRight, new Vector3(0, 1, 0), color),
            };
        }
        
        public VertexPositionNormalColor[] GetUnitCube()
        {
            Vector3 upNormal = new Vector3(0, 1, 0);
            Vector3 downNormal = new Vector3(0, -1, 0);
            Vector3 frontNormal = new Vector3(0, 0, -1);
            Vector3 backNormal = new Vector3(0, 0, 1);
            Vector3 leftNormal = new Vector3(-1, 0, 0);
            Vector3 rightNormal = new Vector3(1, 0, 0);
            return new VertexPositionNormalColor[]{
                /*new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, Color.Purple), // Front
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
                new VertexPositionNormalColor(backTopRight, backTopRightNormal, Color.DarkOrange),*/

                new VertexPositionNormalColor(frontBottomLeft, frontNormal, Color.Purple), // Front
                new VertexPositionNormalColor(frontTopLeft, frontNormal, Color.Purple),
                new VertexPositionNormalColor(frontTopRight, frontNormal, Color.Purple),
                new VertexPositionNormalColor(frontBottomLeft, frontNormal, Color.Purple),
                new VertexPositionNormalColor(frontTopRight, frontNormal, Color.Purple),
                new VertexPositionNormalColor(frontBottomRight, frontNormal, Color.Purple),
                new VertexPositionNormalColor(backBottomLeft, backNormal, Color.Orange), // BACK
                new VertexPositionNormalColor(backTopRight, backNormal, Color.Orange),
                new VertexPositionNormalColor(backTopLeft, backNormal, Color.Orange),
                new VertexPositionNormalColor(backBottomLeft, backNormal, Color.Orange),
                new VertexPositionNormalColor(backBottomRight, backNormal, Color.Orange),
                new VertexPositionNormalColor(backTopRight, backNormal, Color.Orange),
                new VertexPositionNormalColor(frontTopLeft, upNormal, Color.OrangeRed), // Top
                new VertexPositionNormalColor(backTopLeft, upNormal, Color.OrangeRed),
                new VertexPositionNormalColor(backTopRight, upNormal, Color.OrangeRed),
                new VertexPositionNormalColor(frontTopLeft, upNormal, Color.OrangeRed),
                new VertexPositionNormalColor(backTopRight, upNormal, Color.OrangeRed),
                new VertexPositionNormalColor(frontTopRight, upNormal, Color.OrangeRed),
                new VertexPositionNormalColor(frontBottomLeft, downNormal, Color.Blue), // Bottom
                new VertexPositionNormalColor(backBottomRight, downNormal, Color.Blue),
                new VertexPositionNormalColor(backBottomLeft, downNormal, Color.Blue),
                new VertexPositionNormalColor(frontBottomLeft, downNormal, Color.Blue),
                new VertexPositionNormalColor(frontBottomRight, downNormal, Color.Blue),
                new VertexPositionNormalColor(backBottomRight, downNormal, Color.Blue),
                new VertexPositionNormalColor(frontBottomLeft, leftNormal, Color.DarkOrange), // Left
                new VertexPositionNormalColor(backBottomLeft, leftNormal, Color.DarkOrange),
                new VertexPositionNormalColor(backTopLeft, leftNormal, Color.DarkOrange),
                new VertexPositionNormalColor(frontBottomLeft, leftNormal, Color.DarkOrange),
                new VertexPositionNormalColor(backTopLeft, leftNormal, Color.DarkOrange),
                new VertexPositionNormalColor(frontTopLeft, leftNormal, Color.DarkOrange),
                new VertexPositionNormalColor(frontBottomRight, rightNormal, Color.DarkOrange), // Right
                new VertexPositionNormalColor(backTopRight, rightNormal, Color.DarkOrange),
                new VertexPositionNormalColor(backBottomRight, rightNormal, Color.DarkOrange),
                new VertexPositionNormalColor(frontBottomRight, rightNormal, Color.DarkOrange),
                new VertexPositionNormalColor(frontTopRight, rightNormal, Color.DarkOrange),
                new VertexPositionNormalColor(backTopRight, rightNormal, Color.DarkOrange),
            };
        }
        
    }
}
