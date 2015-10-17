using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    // Player class.
    public class Player : GameObject
    {
        //private float speed = 0.006f;
        private float projectileSpeed = 20;
        public float xSpeed = 0;
        public float zSpeed = 0;
        public float angularVelocity;
        public float radius;
        public float xAngle = 0;
        public float zAngle = 0;
        public float xAngularVelocity;
        public float zAngularVelocity;
        private float frictionConstant;
        private Vector3 prevPos;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player", CreatePlayerModel);
            radius = 0.5f;
            frictionConstant = 0.4f;
            pos = new SharpDX.Vector3(0, 0, 0);
            GetParamsFromModel();
            effect = game.Content.Load<Effect>("Phong");
        }

        public MyModel CreatePlayerModel()
        {
            return game.assets.CreateTexturedCube("player.png", 0.7f);
        }

        // Method to create projectile texture to give to newly created projectiles.
        private MyModel CreatePlayerProjectileModel()
        {
            return game.assets.CreateTexturedCube("player projectile.png", new Vector3(0.3f, 0.2f, 0.25f));
        }

        // Shoot a projectile.
        private void fire()
        {
            game.Add(new Projectile(game,
                game.assets.GetModel("player projectile", CreatePlayerProjectileModel),
                pos,
                new Vector3(0, projectileSpeed, 0),
                GameObjectType.Enemy
            ));
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            if (game.keyboardState.IsKeyDown(Keys.Space)) { fire(); }

            // TASK 1: Determine velocity based on accelerometer reading
            //pos.X += (float)game.accelerometerReading.AccelerationX;
            //pos.Z += (float)game.accelerometerReading.AccelerationY;
            prevPos = pos;

            xSpeed += (float)game.accelerometerReading.AccelerationX * 0.2f;
            xSpeed -= xSpeed * frictionConstant;
            zSpeed += (float)game.accelerometerReading.AccelerationY * 0.2f;
            zSpeed -= zSpeed * frictionConstant;
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
            //angularVelocity = (float)Math.Sqrt(xSpeed * xSpeed + zSpeed * zSpeed) * radius;

            // Keep within the boundaries.
            //if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            //if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }
            //if (pos.Z < game.boundaryFront) { pos.Z = game.boundaryFront; }
            //if (pos.Z > game.boundaryBack) { pos.Z = game.boundaryBack; }
            /*if (xSpeed * zSpeed <= 0)
            {
                //basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(zSpeed, 0, -xSpeed)), angularVelocity) * Matrix.Translation(pos);
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(zSpeed, 0, -xSpeed)), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }
            else
            {
                //basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-zSpeed, 0, xSpeed)), angularVelocity) * Matrix.Translation(pos);
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-zSpeed, 0, xSpeed)), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }*/
            /*if (xAngle >= 0 && zAngle >= 0)
            {
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(Math.Abs(zSpeed), 0, -Math.Abs(xSpeed))), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }
            else if (xAngle >= 0 && zAngle < 0)
            {
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-Math.Abs(zSpeed), 0, -Math.Abs(xSpeed))), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }
            else if (xAngle < 0 && zAngle >= 0)
            {
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(Math.Abs(zSpeed), 0, Math.Abs(xSpeed))), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }
            else
            {
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-Math.Abs(zSpeed), 0, Math.Abs(xSpeed))), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }*/
            /*if (pos.X * pos.Z <= 0)
            {
                //basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(zSpeed, 0, -xSpeed)), angularVelocity) * Matrix.Translation(pos);
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(pos.Z, 0, -pos.X)), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }
            else
            {
                //basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-zSpeed, 0, xSpeed)), angularVelocity) * Matrix.Translation(pos);
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-pos.Z, 0, pos.X)), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }*/
            /*if (xSpeed * zSpeed <= 0)
            {
                //basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(zSpeed, 0, -xSpeed)), angularVelocity) * Matrix.Translation(pos);
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(zSpeed, 0, -xSpeed)), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }
            else
            {
                //basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-zSpeed, 0, xSpeed)), angularVelocity) * Matrix.Translation(pos);
                basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(-zSpeed, 0, xSpeed)), -(float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
            }*/
            /*if (xSpeed * zSpeed <= 0)
            {
                basicEffect.World = Matrix.RotationX(zAngle) * Matrix.RotationZ(-xAngle) * Matrix.Translation(pos);
            }
            else
            {
                basicEffect.World = Matrix.RotationX(-zAngle) * Matrix.RotationZ(xAngle) * Matrix.Translation(pos);
            }*/
            //basicEffect.World = Matrix.RotationX(zAngle) * Matrix.RotationAxis(new Vector3(0, 0, -1), xAngle) * Matrix.Translation(pos);
            basicEffect.World = basicEffect.World * Matrix.Translation(-prevPos) * Matrix.RotationX(zAngularVelocity) * Matrix.RotationAxis(new Vector3(0, 0, -1), xAngularVelocity) * Matrix.Translation(pos);
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

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.Exit();
        }

        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            //fire();
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            pos.X += (float)args.Delta.Translation.X / 100;
        }
    }
}