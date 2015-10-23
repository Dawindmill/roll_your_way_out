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

        public Vector2 leftUpCorner2;
        public Vector3 inLeftUp;

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
            Vector3 nextPos = pos;
            float prevXspeed = xSpeed;
            float prevZspeed = zSpeed;
            float xDir;
            float zDir;
            float xAmountIncrease;
            float zAmountIncrease;
            float increasePercent= 0.001f;




            xSpeed += (float)game.accelerometerReading.AccelerationX * 0.2f;
            xSpeed -= xSpeed * frictionConstant;
            
            xAmountIncrease = (float)game.accelerometerReading.AccelerationX * 0.2f*increasePercent;

            zSpeed += (float)game.accelerometerReading.AccelerationY * 0.2f;
            zSpeed -= zSpeed * frictionConstant;
            zAmountIncrease = (float)game.accelerometerReading.AccelerationY * 0.2f*increasePercent;

            nextPos.X += xSpeed;
            nextPos.Z += zSpeed;

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


            CollisionDetection(nextPos);
            


            if (isCollidedLeft)
            {
                xSpeed = -xSpeed;
                xSpeed = Math.Abs(xSpeed);
                //xSpeed = 0;
                isCollidedLeft = false;
            }
            if (isCollidedRight)
            {
                xSpeed = -Math.Abs(xSpeed);
                //xSpeed = 0;
                isCollidedRight = false;
            }
            if (isCollidedUp)
            {
                zSpeed = -zSpeed;
                zSpeed = -Math.Abs(zSpeed);
                //zSpeed = 0;
                isCollidedUp = false;
            }
            if (isCollidedDown)
            {
                zSpeed = Math.Abs(zSpeed);
                //zSpeed = 0;
                isCollidedDown = false;
            }
            prevPos = pos;

            //pos = nextPos;
            if (nextPos.X < -MazeLandscape.CUBESCALE)
            {
                pos.X = -MazeLandscape.CUBESCALE;
                xSpeed = 0;
            }
            else if (nextPos.X > (game.mazeDimension - 1) * 2 * MazeLandscape.CUBESCALE)
            {
                pos.X = (game.mazeDimension - 1) * 2 * MazeLandscape.CUBESCALE;
                xSpeed = 0;
            }
            else
            {
                pos.X += xSpeed;
            }
            
            if (nextPos.Z < -MazeLandscape.CUBESCALE)
            {
                pos.Z = -MazeLandscape.CUBESCALE;
                zSpeed = 0;
            }
            else if (nextPos.Z > (game.mazeDimension - 1) * 2 * MazeLandscape.CUBESCALE)
            {
                pos.Z = (game.mazeDimension - 1) * 2 * MazeLandscape.CUBESCALE;
                zSpeed = 0;
            }
            else
            {
                pos.Z += zSpeed;
            }
            /*if (pos.X < 0)
            {
                pos.X = 0;
            }
            else if (pos.X > game.mazeDimension * MazeLandscape.CUBESCALE)
            {
                pos.X = game.mazeDimension * MazeLandscape.CUBESCALE;
            }
            else
            {
                pos.X += xSpeed;
            }

            if (pos.Z < 0)
            {
                pos.Z = 0;
            }
            else if (pos.Z > game.mazeDimension * MazeLandscape.CUBESCALE)
            {
                pos.Z = game.mazeDimension * MazeLandscape.CUBESCALE;
            }
            else
            {
                pos.Z += zSpeed;
            }*/

            //pos.X += xSpeed;
            //pos.Z += zSpeed;
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
            float leftPosInMaze, rightPosInMaze, upPosInMaze, downPosInMaze, leftUpInMaze, rightUpInMaze, leftDownInMaze, rightDownInMaze;

            Vector2 leftUpCorner, rightUpCorner, leftDownCorner, rightDownCorner, sphereCenter;
            float leftUpDistance, rightUpDistance, leftDownDistance, rightDownDistance;
            
            sphereCenter = new Vector2(next.X, next.Z);

            posInMaze = PositionInMaze(next);
            left = PositionInMaze(next + new Vector3(-radius, 0, 0));
            right = PositionInMaze(next + new Vector3(radius, 0, 0));
            up = PositionInMaze(next + new Vector3(0, 0, radius));
            down = PositionInMaze(next + new Vector3(0, 0, -radius));
            leftUp = PositionInMaze(next + new Vector3(-radius, 0, radius));
            rightUp = PositionInMaze(next + new Vector3(radius, 0, radius));
            leftDown = PositionInMaze(next + new Vector3(-radius, 0, -radius));
            rightDown = PositionInMaze(next + new Vector3(radius, 0, -radius));

            /*leftUpCorner = new Vector2((leftUp.X - 1) * MazeLandscape.CUBESCALE, (leftUp.Y + 1) * MazeLandscape.CUBESCALE);
            rightUpCorner = new Vector2((rightUp.X + 1) * MazeLandscape.CUBESCALE, (rightUp.Y + 1) * MazeLandscape.CUBESCALE);
            leftDownCorner = new Vector2((leftDown.X - 1) * MazeLandscape.CUBESCALE, (leftDown.Y - 1) * MazeLandscape.CUBESCALE);
            rightDownCorner = new Vector2((rightDown.X + 1) * MazeLandscape.CUBESCALE, (rightDown.Y + 1) * MazeLandscape.CUBESCALE);*/


            float size = MazeLandscape.CUBESCALE * 2;
            float halfSize = MazeLandscape.CUBESCALE;
            float posX = posInMaze.X * size;
            float posY = posInMaze.Y * size;

            leftUpCorner = new Vector2(posX - halfSize, posY + halfSize);
            leftUpCorner2 = new Vector2(posX - halfSize, posY + halfSize);
            rightUpCorner = new Vector2(posX + halfSize, posY + halfSize);
            leftDownCorner = new Vector2(posX - halfSize, posY - halfSize);
            rightDownCorner = new Vector2(posX + halfSize, posY - halfSize);

            leftUpDistance = distance(leftUpCorner, sphereCenter);
            rightUpDistance = distance(rightUpCorner, sphereCenter);
            leftDownDistance = distance(leftDownCorner, sphereCenter);
            rightDownDistance = distance(rightDownCorner, sphereCenter);

            float topSide, bottomSide, leftSide, rightSide;
            bottomSide = posInMaze.Y - 1 > 0 ? (posInMaze.Y - 1) : 0;
            topSide = posInMaze.Y + 1 < game.mazeDimension ? (posInMaze.Y + 1) : 49;
            leftSide = posInMaze.X - 1 > 0 ? (posInMaze.X - 1) : 0;
            rightSide = posInMaze.Y + 1 < game.mazeDimension ? (posInMaze.X + 1) : 49;

            leftUpInMaze = game.mazeLandscape.maze.maze[(int)(topSide), (int)(leftSide)];
            rightUpInMaze = game.mazeLandscape.maze.maze[(int)(topSide), (int)(rightSide)];
            leftDownInMaze = game.mazeLandscape.maze.maze[(int)(bottomSide), (int)(leftSide)];
            rightDownInMaze = game.mazeLandscape.maze.maze[(int)(bottomSide), (int)(rightSide)];

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

            /*if (leftUpDistance < radius && leftDownDistance < radius)
            {
                isCollidedLeft = true;
            }
            if (leftUpDistance < radius && rightUpDistance < radius)
            {
                isCollidedUp = true;
            }
            if (rightUpDistance < radius && rightDownDistance < radius)
            {
                isCollidedRight = true;
            }
            if (leftDownDistance < radius && rightDownDistance < radius)
            {
                isCollidedDown = true;
            }*/
            inLeftUp = new Vector3(leftSide, topSide, leftUpInMaze);
            float buffer = (float)(radius * 0.1);
            if (leftUpInMaze == 1 && leftUpDistance < radius - buffer)
            {
                isCollidedLeft = true;
                isCollidedUp = true;
            }
            if (rightUpInMaze == 1 && rightUpDistance < radius - buffer)
            {
                isCollidedRight = true;
                isCollidedUp = true;
            }
            if (rightDownInMaze == 1 && rightDownDistance < radius - buffer)
            {
                isCollidedRight = true;
                isCollidedDown = true;
            }
            if (leftDownInMaze == 1 && leftDownDistance < radius - buffer)
            {
                isCollidedLeft = true;
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

        public float distance(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
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
