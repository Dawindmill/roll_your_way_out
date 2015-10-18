using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Sphere:GameObject
    {

        public Model model;
        //Matrix view;
        //Matrix world;
        //Matrix projection;
        //BoundingSphere modelBounds;
        public float xSpeed = 0;
        public float zSpeed = 0;
        public float angularVelocity;
        public float radius;
        public float xAngle = 0;
        public float zAngle = 0;
        public float xAngularVelocity;
        public float zAngularVelocity;
        private float frictionConstant;
        public Vector3 prevPos;
        public Vector3 nextPos;
        private float scallingFactor = 0.5f;
        public bool isCollidedX = false;
        public bool isCollidedZ = false;

        public Sphere(Model sphere,LabGame game)
        {
            this.game = game;
            model = sphere;
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.camera.View,
                Projection = game.camera.Projection,
                World = Matrix.Identity,
                //Texture = myModel.Texture,
                TextureEnabled = true,
                VertexColorEnabled = false
            };
            BasicEffect.EnableDefaultLighting(model,true);
            //const float MaxModelSize = 10.0f;
            //var scaling = MaxModelSize / modelBounds.Radius;
            //var scaling = MaxModelSize / model.Meshes[0].BoundingSphere.Radius;
            //modelBounds = model.CalculateBounds();
            //view = Matrix.LookAtRH(new Vector3(0, 0, MaxModelSize * 2.5f), new Vector3(0, 0, 0), Vector3.UnitY);
            //projection = Matrix.PerspectiveFovRH(0.9f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, MaxModelSize * 10.0f);
            //world = Matrix.Translation(-modelBounds.Center.X, -modelBounds.Center.Y, -modelBounds.Center.Z) * Matrix.Scaling(0.5f);
            //basicEffect.View = game.camera.View;
            //basicEffect.Projection = game.camera.Projection;
            //modelBounds = model.CalculateBounds();
            //pos = model.BoundingSphere.Center;
          //  pos = new Vector3(-model.BoundingSphere.Center.X,-model.BoundingSphere.Center.Y,-model.BoundingSphere.Center.Z);
            //pos = new Vector3(game.mazeLandscape.maze.startPoint.x, 3, game.mazeLandscape.maze.startPoint.y);
            pos = new Vector3(game.mazeLandscape.entranceX, 0, game.mazeLandscape.entranceZ);
            basicEffect.World = Matrix.Translation(pos);// * Matrix.Scaling(1f);
            //effect = game.Content.Load<Effect>("Phong");

            radius = model.Meshes[0].BoundingSphere.Radius * scallingFactor;
            frictionConstant = 0.4f;

        }

        public override void Draw(GameTime gametime)
        {
            //Matrix[] bones = new Matrix []{ Matrix.Translation(new Vector3(0, 0, 0)) };
            //model.Effects[0].
            //model.Draw(game.GraphicsDevice, world, view, projection);

            model.Draw(game.GraphicsDevice, basicEffect.World, basicEffect.View, basicEffect.Projection);
                //base.Draw(gametime);

        }

        public override void Update(GameTime gameTime)
        {
            basicEffect.View = game.camera.View;
            //projection = game.camera.Projection;

            nextPos = prevPos;


            xSpeed += (float)game.accelerometerReading.AccelerationX * 0.2f;
            xSpeed -= xSpeed * frictionConstant;

            zSpeed += (float)game.accelerometerReading.AccelerationY * 0.2f;
            zSpeed -= zSpeed * frictionConstant;

            nextPos.X += xSpeed;
            nextPos.Z += zSpeed;



            CollisionDetection(nextPos);


            if (isCollidedX)
            {
                //xSpeed = -xSpeed;
                xSpeed = 0;
                isCollidedX = false;
            }
            if (isCollidedZ)
            {
                //zSpeed = -zSpeed;
                zSpeed = 0;
                isCollidedZ = false;
            }

            prevPos = pos;

            pos.X += xSpeed;
            pos.Z += zSpeed;
            //xAngle += xSpeed * radius;
            //zAngle += zSpeed * radius;
            xAngularVelocity = xSpeed / radius;
            zAngularVelocity = zSpeed / radius;
            //xAngle = pos.X / radius;
            //zAngle = pos.Z / radius;
            xAngle += xAngularVelocity;
            zAngle += zAngularVelocity;
            //basicEffect.World = basicEffect.World * Matrix.Translation(-prevPos) * Matrix.RotationX(zAngularVelocity) * Matrix.RotationAxis(new Vector3(0, 0, -1), xAngularVelocity) * Matrix.Translation(pos);
            basicEffect.World = basicEffect.World * Matrix.Translation(-prevPos) * Matrix.RotationZ(xAngularVelocity) * Matrix.RotationAxis(new Vector3(-1, 0, 0), zAngularVelocity) * Matrix.Translation(pos);
        }

        public void CollisionDetection(Vector3 next)
        {
            Vector2 pos = PositionInMaze(next);

            float[,] maze = this.game.mazeLandscape.maze.maze;

            if (maze[(int)(pos.X - radius), (int)pos.Y] == 1 ||
                maze[(int)(pos.X + radius), (int)pos.Y] == 1)
            {
                isCollidedX = true;
                System.Diagnostics.Debug.WriteLine((int)pos.X + " " + (int)pos.Y);

            }

            if (maze[(int)pos.X, (int)(pos.Y - radius)] == 1 ||
                maze[(int)pos.X, (int)(pos.Y + radius)] == 1)
            {
                isCollidedZ = true;
            }
        }


        public Vector2 PositionInMaze(Vector3 pos)
        {
            float cube_side = 2 * MazeLandscape.CUBESCALE;

            Vector2 newPos = new Vector2();
            newPos.X = (int)pos.X / cube_side;
            newPos.Y = (int)pos.Z / cube_side;
            return newPos;
        }
    }
}
