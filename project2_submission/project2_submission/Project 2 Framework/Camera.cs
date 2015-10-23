using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Project
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public LabGame game;
        public Vector3 pos;
        public Vector3 oldPos;
        public Vector3 pos_relative_to_player;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(LabGame game) {
            pos_relative_to_player = new Vector3(0, 15, -10);
            //pos = new Vector3(game.mazeLandscape.maze.startPoint.x, 0, game.mazeLandscape.maze.startPoint.y) + pos_relative_to_player;
           // pos = new Vector3(5000,5000,12000);
            //View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
            //View = Matrix.LookAtLH(pos, new Vector3(5000, 5000, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 15000.0f);
            this.game = game;
        }

        public void setStartingPosView()
        {
            //pos = new Vector3(game.mazeLandscape.maze.startPoint.x, 0, game.mazeLandscape.maze.startPoint.y) + pos_relative_to_player;
            pos = game.sphere.pos + pos_relative_to_player;
            View = Matrix.LookAtLH(pos, game.sphere.pos, Vector3.UnitY);
 
        }

        // If the screen is resized, the projection matrix will change
        public void Update()
        {
            pos = game.sphere.pos + pos_relative_to_player;
            View = Matrix.LookAtLH(pos, game.sphere.pos, Vector3.UnitY);
        }
    }
}
