using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class MazeLandscape : GameObject
    {
        int dimension;
        int seed;
        public static String mazeModelName = "MazeLandscape";
        public static float CUBESCALE = 2.0f;
        public RandomMaze maze;
        public float entranceX;
        public float entranceZ;
        public VertexPositionNormalColor[] normalMaze;
        Cube cube;
        public MazeLandscape(LabGame game,int dimension,int seed )
        {
            this.seed = seed;
            this.dimension = dimension;
            this.game = game;
            maze = new RandomMaze(dimension, seed);
            cube = new Cube();
            maze.GenerateMaze();
            maze.setStartPointAndDestPoint();

            //display path.


            /*
                            List<Node> pathFromSrcToDest = maze.astar.FindPath(maze.astar.Float2DtoInt(maze.maze)
                , RandomMaze.WALL, maze.startPoint.x, maze.startPoint.y, maze.destPoint.x, maze.destPoint.y); 
            */
            //maze.updateMazeRoad(pathFromSrcToDest, Project.Cube.goal);

            type = GameObjectType.MazeLandscape;
            //if maze existed before just remove it from model
            game.assets.modelDict.Remove(mazeModelName);
            myModel = game.assets.GetModel(mazeModelName, CreateMazeLandscapeModel);
            //radius = 0.5f;
            //frictionConstant = 0.4f;
            pos = new SharpDX.Vector3(0, 0, 0);
            GetParamsFromModel();
            effect = game.Content.Load<Effect>("Phong");
            Debug.WriteLine("maze created");
            entranceX = maze.startPoint.x * CUBESCALE * 2;
            entranceZ = maze.startPoint.y * CUBESCALE * 2;
            normalMaze = myModel.shapeArray;
        }

        public void showPath()
        {
            //Cube cube = new cube();
            List<Node> pathFromSrcToDest = maze.astar.FindPath(maze.astar.Float2DtoInt(maze.maze)
                , RandomMaze.WALL, maze.startPoint.x, maze.startPoint.y, maze.destPoint.x, maze.destPoint.y);

            VertexPositionNormalColor[] wayOut = cube.GetMazeWayOutVertexWithCube(pathFromSrcToDest,CUBESCALE,Color.Red);
            VertexPositionNormalColor[] combinedWayOutAndMaze = new VertexPositionNormalColor[wayOut.Count() + myModel.shapeArray.Count()];
            Array.Copy(wayOut, combinedWayOutAndMaze, wayOut.Count());
            Array.Copy(normalMaze, 0, combinedWayOutAndMaze, wayOut.Count(), normalMaze.Count());
            myModel.shapeArray = combinedWayOutAndMaze;
            myModel.vertices = SharpDX.Toolkit.Graphics.Buffer.Vertex.New(game.GraphicsDevice, myModel.shapeArray);
            /*landScape = new VertexPositionNormalColor[terrain.Length + water.Length];

            Array.Copy(terrain, landScape, terrain.Length);
            Array.Copy(water, 0, landScape, terrain.Length, water.Length);
*/
        }

        public void closePath()
        {
            myModel.shapeArray = normalMaze;
            myModel.vertices = SharpDX.Toolkit.Graphics.Buffer.Vertex.New(game.GraphicsDevice, myModel.shapeArray);
        }

        public MyModel CreateMazeLandscapeModel()
        {
            return game.assets.CreateMazeLandscapeCube(dimension, seed, CUBESCALE,maze);
        }

        public override void Draw(GameTime gametime)
        {
           if (myModel != null)
            {
                //basicEffect.View = game.camera.View;
                effect.Parameters["World"].SetValue(basicEffect.World);
                effect.Parameters["View"].SetValue(basicEffect.View);
                effect.Parameters["Projection"].SetValue(basicEffect.Projection);
                effect.Parameters["cameraPos"].SetValue(new Vector4(game.camera.pos.X, game.camera.pos.Y, game.camera.pos.Z, 1));
                effect.Parameters["lightPntPos"].SetValue(new Vector4(dimension * CUBESCALE * 2 + 5000, 100000, 100, 1));
                effect.Parameters["worldInvTrp"].SetValue(Matrix.Transpose(Matrix.Invert(basicEffect.World)));
                effect.Techniques[0].Passes[0].Apply();
            }
            // Some objects such as the Enemy Controller have no model and thus will not be drawn
            base.Draw(gametime);

        }

       

        public override void Update(GameTime gameTime)
        {
        
            basicEffect.View = game.camera.View;
            basicEffect.Projection=game.camera.Projection;
        }

    }


}
