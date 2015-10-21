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
        //public Vector3 nextPos;
        private float scallingFactor = 0.5f;
        public bool isCollidedLeft = false;
        public bool isCollidedRight = false;
        public bool isCollidedUp = false;
        public bool isCollidedDown = false;

        public float nextPosType;
        private Texture2D texture;

        public Sphere(Model sphere,LabGame game)
        {
            this.game = game;
            model = sphere;
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.camera.View,
                
                //View = Matrix.LookAtRH(game.camera.pos, game.camera.pos+game.camera.pos_relative_to_player, Vector3.UnitY),
                Projection = game.camera.Projection,
                //Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 15000.0f),
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
            radius = model.CalculateBounds().Radius * scallingFactor + 0.6f;
            basicEffect.World = Matrix.Translation(new Vector3(model.CalculateBounds().Center.X,
                model.CalculateBounds().Center.Y, model.CalculateBounds().Center.Z)) * Matrix.Translation(pos);// * Matrix.Scaling(1f);
            effect = game.Content.Load<Effect>("modelShader");
            texture = game.Content.Load<Texture2D>("wood");

            //radius = model.Meshes[0].BoundingSphere.Radius * scallingFactor;
            
            frictionConstant = 0.2f;

        }

        public override void Draw(GameTime gametime)
        {
            //Matrix[] bones = new Matrix []{ Matrix.Translation(new Vector3(0, 0, 0)) };
            //model.Effects[0].
            //model.Draw(game.GraphicsDevice, world, view, projection);
            //Matrix newWorld = basicEffect.World * Matrix.Scaling(1, 1, -1);

            effect.Parameters["World"].SetValue(basicEffect.World);
            effect.Parameters["View"].SetValue(basicEffect.View);
            effect.Parameters["Projection"].SetValue(basicEffect.Projection);
            effect.Parameters["cameraPos"].SetValue(new Vector4(game.camera.pos.X, game.camera.pos.Y, game.camera.pos.Z, 1));
            effect.Parameters["lightPntPos"].SetValue(new Vector4(game.mazeDimension * MazeLandscape.CUBESCALE * 2 + 5000, 100000, 100, 1));
            effect.Parameters["worldInvTrp"].SetValue(Matrix.Transpose(Matrix.Invert(basicEffect.World)));
            effect.Parameters["tex"].SetResource(texture);
            effect.Techniques[0].Passes[0].Apply();
            model.Draw(game.GraphicsDevice, basicEffect.World, basicEffect.View, basicEffect.Projection, effect);
                //base.Draw(gametime);

        }


        public override void Update(GameTime gameTime)
        {

            //basicEffect.View = Matrix.LookAtRH(game.camera.pos, game.sphere.pos, Vector3.UnitY);
            basicEffect.View = game.camera.View;
            //projection = game.camera.Projection;
            Vector3 currentPos = prevPos;
            float prevXspeed = xSpeed;
            float prevZspeed = zSpeed;
            float xDir;
            float zDir;
            float xAmountIncrease;
            float zAmountIncrease;
            float increasePercent= 0.001f;


            currentPos = prevPos;


            xSpeed += (float)game.accelerometerReading.AccelerationX * 0.2f;
            xSpeed -= xSpeed * frictionConstant;
            
            xAmountIncrease = (float)game.accelerometerReading.AccelerationX * 0.2f*increasePercent;

            zSpeed += (float)game.accelerometerReading.AccelerationY * 0.2f;
            zSpeed -= zSpeed * frictionConstant;
            zAmountIncrease = (float)game.accelerometerReading.AccelerationY * 0.2f*increasePercent;


            /*while ((Math.Abs(prevXspeed) - Math.Abs(xSpeed)) >= xAmountIncrease &&
                (Math.Abs(prevZspeed) - Math.Abs(zSpeed)) >= zAmountIncrease)
            {
                currentPos = prevPos;
                CollisionDetection(currentPos);
                if ((Math.Abs(prevXspeed) - Math.Abs(xSpeed)) >= xAmountIncrease)
                { 
                    prevXspeed += xAmountIncrease;
                    prevXspeed -= prevXspeed * frictionConstant;
                    currentPos.X += prevXspeed;
                }
                if ((Math.Abs(prevZspeed) - Math.Abs(zSpeed)) >= zAmountIncrease)
                { 
                    prevZspeed += zAmountIncrease;
                    prevZspeed -= prevZspeed * frictionConstant;
                    currentPos.Z += prevZspeed;
                }
            }*/


            CollisionDetection(currentPos);
            


            if (isCollidedLeft)
            {
                //xSpeed = -xSpeed;
                xSpeed = Math.Abs(xSpeed);
                isCollidedLeft = false;
            }
            if (isCollidedRight)
            {
                xSpeed = -Math.Abs(xSpeed);
                isCollidedRight = false;
            }
            if (isCollidedUp)
            {
                //zSpeed = -zSpeed;
                zSpeed = -Math.Abs(zSpeed);
                isCollidedUp = false;
            }
            if (isCollidedDown)
            {
                zSpeed = Math.Abs(zSpeed);
                isCollidedDown = false;
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

        /*public void CollisionDetection(Vector3 currentPos)
        {
            //same orentation as array but flip 0,0 to bottom left
            //future step
            Vector3 futurePos = new Vector3(currentPos.X+xSpeed,0,currentPos.Z+zSpeed);
            //row = z, col = x
            //      front                z
            //left  sphere  right        | up increase
            //      back --->x right increase?
            float cubeSideLength = 2*MazeLandscape.CUBESCALE;
            float frontSphereRow=(futurePos.Z+ radius +MazeLandscape.CUBESCALE)/cubeSideLength;
            float frontSphereCol = (futurePos.X  + MazeLandscape.CUBESCALE)/cubeSideLength;

            float backSphereRow = (futurePos.Z - radius + MazeLandscape.CUBESCALE) / cubeSideLength;
            float backSphereCol = (futurePos.X + MazeLandscape.CUBESCALE)/cubeSideLength;

            float leftSphereRow = (futurePos.Z +MazeLandscape.CUBESCALE)/cubeSideLength;
            float leftSphereCol = (futurePos.X- radius +MazeLandscape.CUBESCALE)/cubeSideLength;

            float rightSphereRow = (futurePos.Z  + MazeLandscape.CUBESCALE)/cubeSideLength;
            float rightSphereCol = (futurePos.X + radius+ MazeLandscape.CUBESCALE)/cubeSideLength;
            float zMaX = MazeLandscape.CUBESCALE+(game.mazeDimension-1)*2*MazeLandscape.CUBESCALE;
            float xMax = MazeLandscape.CUBESCALE+(game.mazeDimension-1)*2*MazeLandscape.CUBESCALE;
            float xMin=-MazeLandscape.CUBESCALE;
            float zMin=-MazeLandscape.CUBESCALE;

            if ( futurePos.Z - radius <= zMin  ||
                futurePos.Z + radius >= zMaX || futurePos.Z <= zMin || futurePos.Z >= zMaX)
            {
                isCollidedZ = true;

            }

            if (futurePos.X - radius <= xMin || futurePos.X + radius >= xMax
                || futurePos.X <= xMin || futurePos.X >= xMax)
            {
                isCollidedX = true;
            }
            

        }*/


        
        public void CollisionDetection(Vector3 next)
        {
            Vector2 posInMaze, left, right, up, down, leftUp, rightUp, leftDown, rightDown;
            float leftPosInMaze, rightPosInMaze, upPosInMaze, downPosInMaze, leftUpInMaze, rightUpInMaze, LeftDownInMaze, RightDownInMaze;
            posInMaze = PositionInMaze(next);
            left = PositionInMaze(next + new Vector3(-radius, 0, 0));
            right = PositionInMaze(next + new Vector3(radius, 0, 0));
            up = PositionInMaze(next + new Vector3(0, 0, radius));
            down = PositionInMaze(next + new Vector3(0, 0, -radius));
            leftUp = PositionInMaze(next + new Vector3(-radius, 0, radius));
            rightUp = PositionInMaze(next + new Vector3(radius, 0, radius));
            leftDown = PositionInMaze(next + new Vector3(-radius, 0, -radius));
            rightDown = PositionInMaze(next + new Vector3(radius, 0, -radius));

            leftUpInMaze = game.mazeLandscape.maze.maze[(int)(leftUp.Y), (int)(leftUp.X)];
            rightUpInMaze = game.mazeLandscape.maze.maze[(int)(rightUp.Y), (int)(rightUp.X)];
            LeftDownInMaze = game.mazeLandscape.maze.maze[(int)(leftDown.Y), (int)(leftDown.X)];
            RightDownInMaze = game.mazeLandscape.maze.maze[(int)(rightDown.Y), (int)(rightDown.X)];

            nextPosType = game.mazeLandscape.maze.maze[(int)(posInMaze.Y), (int)(posInMaze.X)];

            leftPosInMaze = game.mazeLandscape.maze.maze[(int)(left.Y), (int)(left.X)];
            rightPosInMaze = game.mazeLandscape.maze.maze[(int)(right.Y), (int)(right.X)];
            upPosInMaze = game.mazeLandscape.maze.maze[(int)(up.Y), (int)(up.X)];
            downPosInMaze = game.mazeLandscape.maze.maze[(int)(down.Y), (int)(down.X)];

            if (leftPosInMaze == 1)
            {
                isCollidedLeft = true;
            }
            
            if (rightPosInMaze == 1)
            {
                isCollidedRight = true;
            }

            if (upPosInMaze == 1)
            {
                isCollidedUp = true;
            }
            if (downPosInMaze == 1)
            {
                isCollidedDown = true;
            }
            /*
            List<Vector2> posList = new List<Vector2>();
            Vector3 left = new Vector3();
            left.X = next.X - radius;
            left.Z = next.Z;
            Vector2 leftMaze = PositionInMaze(left);

            Vector3 right = new Vector3();
            right.X = next.X + radius;
            right.Z = next.Z;
            Vector2 rightMaze = PositionInMaze(right);

            Vector3 up = new Vector3();
            up.X = next.X;
            up.Z = next.Z - radius;
            Vector2 upMaze = PositionInMaze(up);

            Vector3 down = new Vector3();
            down.X = next.X;
            down.Z = next.Z + radius;
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
            }*/
        }
        

        public Vector2 PositionInMaze(Vector3 nextPos)
        {
            float cube_side = 2 * MazeLandscape.CUBESCALE;

            Vector2 newPos = new Vector2();

            newPos.X = (int)((nextPos.X+MazeLandscape.CUBESCALE) / cube_side);
            newPos.Y = (int)((nextPos.Z+MazeLandscape.CUBESCALE) / cube_side);

            if (newPos.X < 0)
            {
                newPos.X = 0;
            }

            if (newPos.Y < 0)
            {
                newPos.Y = 0;
            }

            if (newPos.X >= game.mazeDimension)
            {
                newPos.X = 49;
            }
            if (newPos.Y >= game.mazeDimension)
            {
                newPos.Y = 49;
            }


            return newPos;
        }
    }
}
