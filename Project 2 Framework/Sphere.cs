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



            CheckFourDimension(nextPos);


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
            float offset = 0.5f;
            List<Vector2> posList = new List<Vector2>();
            Vector3 left = new Vector3();
            left.X = next.X - radius - offset;
            left.Z = next.Z;
            Vector2 leftMaze = PositionInMaze(left);

            Vector3 right = new Vector3();
            right.X = next.X + radius + offset;
            right.Z = next.Z;
            Vector2 rightMaze = PositionInMaze(right);

            Vector3 up = new Vector3();
            up.X = next.X;
            up.Z = next.Z - radius - offset;
            Vector2 upMaze = PositionInMaze(up);

            Vector3 down = new Vector3();
            down.X = next.X;
            down.Z = next.Z + radius + offset;
            Vector2 downMaze = PositionInMaze(down);


            float[,] maze = this.game.mazeLandscape.maze.maze;

            //System.Diagnostics.Debug.WriteLine("Left:" + left.X + " " + left.Y);
            //System.Diagnostics.Debug.WriteLine("Right:" + right.X + " " + right.Y);

            if (maze[(int)leftMaze.Y, (int)(leftMaze.X)] == 1 ||
                maze[(int)rightMaze.Y, (int)(rightMaze.X)] == 1)
            {
                isCollidedX = true;

            }

            if (maze[(int)(upMaze.Y), (int)upMaze.X] == 1 ||
                maze[(int)(downMaze.Y), (int)downMaze.X] == 1)
            {
                isCollidedZ = true;
            }
        }

        public Vector2 PositionInMaze(Vector3 nextPos)
        {
            float cube_side = 2 * MazeLandscape.CUBESCALE;

            Vector2 newPos = new Vector2();

            newPos.X = (int)nextPos.X / cube_side;
            newPos.Y = (int)nextPos.Z / cube_side;

            //if (newPos.X < 0)
            //{
            //    newPos.X = 0;
            //}

            //if (newPos.Y < 0)
            //{
            //    newPos.Y = 0;
            //}


            return newPos;
        }

        public Vector3 FindClosetWall(Vector3 currentPostion)
        {
           Vector3[,] positionList = this.game.mazeLandscape.positionList;
            float minDistance = positionList.Length * 2 * MazeLandscape.CUBESCALE;
            Vector3 closestWall = new Vector3();
            Vector2 positionInMaze = PositionInMaze(currentPostion);

            int dimension = this.game.mazeLandscape.maze.dimension;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (!(i == positionInMaze.X && j == positionInMaze.Y) 
                        && DistanceBetweenTwoPoint(positionList[i, j], currentPostion) < minDistance){
                        minDistance = DistanceBetweenTwoPoint(positionList[i, j], currentPostion);
                        closestWall = positionList[i, j];
                    }
                }
            }

            return closestWall;
        }

        public void CheckFourDimension(Vector3 currentPosition)
        {
            Vector3 closestWall = FindClosetWall(currentPosition);
            float halfCubeSize = MazeLandscape.CUBESCALE;
            if (closestWall.Y == 1)
            {
                //Left && Right
                if ((currentPosition.X - radius < closestWall.X + halfCubeSize) ^
                        (currentPosition.X + radius > closestWall.X - halfCubeSize))
                {
                //if (currentPosition.X - radius < closestWall.X + halfCubeSize)
                //{
                    isCollidedX = true;
                }

                //Top && Bottom;
                //if((currentPosition.Z  - radius < closestWall.Z + halfCubeSize) ^ 
                //        (currentPosition.Z + radius > closestWall.Z - halfCubeSize))
                //{
                //    isCollidedZ = true;
                //}
            }



        }

        public float DistanceBetweenTwoPoint(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Z - p2.Z), 2));
        }
        
    }
}
