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
        static int CUBESCALE=100;
        public RandomMaze maze;
        public MazeLandscape(LabGame game,int dimension,int seed )
        {
            this.seed = seed;
            this.dimension = dimension;
            this.game = game;
            maze = new RandomMaze(dimension, seed);
            Cube cube = new Cube();
            maze.GenerateMaze();
            maze.setStartPointAndDestPoint();

            //display path.
            List<Node> pathFromSrcToDest = maze.astar.FindPath(maze.astar.Float2DtoInt(maze.maze)
                ,RandomMaze.WALL,maze.startPoint.x,maze.startPoint.y,maze.destPoint.x,maze.destPoint.y);
            maze.updateMazeRoad(pathFromSrcToDest, Project.Cube.goal);

            type = GameObjectType.MazeLandscape;
            myModel = game.assets.GetModel("MazeLandscape", CreateMazeLandscapeModel);
            //radius = 0.5f;
            //frictionConstant = 0.4f;
            pos = new SharpDX.Vector3(0, 0, 0);
            GetParamsFromModel();
            effect = game.Content.Load<Effect>("Phong");
            Debug.WriteLine("maze created");
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
                effect.Parameters["lightPntPos"].SetValue(new Vector4(50, 50, -50, 1));
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
