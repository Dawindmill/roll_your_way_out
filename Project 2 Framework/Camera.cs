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
        private Vector3 pos_relative_to_player;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(LabGame game) {
            pos = new Vector3(0, 5, -5);
            pos_relative_to_player = new Vector3(0, 5, -5);
            View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update()
        {
            //pos = game.player.pos + pos_relative_to_player;
            View = Matrix.LookAtLH(pos, game.player.pos, Vector3.UnitY);
        }
    }
}
