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
        private float xSpeed = 0;
        private float zSpeed = 0;
        private float angularVelocity;
        private float radius;
        private float xAngle = 0;
        private float zAngle = 0;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player", CreatePlayerModel);
            radius = 0.5f;
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

            xSpeed = (float)game.accelerometerReading.AccelerationX * 0.2f;
            zSpeed = (float)game.accelerometerReading.AccelerationY * 0.2f;
            pos.X += xSpeed;
            pos.Z += zSpeed;
            xAngle += xSpeed * radius;
            zAngle += zSpeed * radius;
            angularVelocity = (float)Math.Sqrt(xSpeed * xSpeed + zSpeed * zSpeed) * radius;

            // Keep within the boundaries.
            if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }
            if (pos.Z < game.boundaryFront) { pos.Z = game.boundaryFront; }
            if (pos.Z > game.boundaryBack) { pos.Z = game.boundaryBack; }

            basicEffect.World = Matrix.RotationAxis(Vector3.Normalize(new Vector3(pos.Z, pos.Y, -pos.X)), (float)Math.Sqrt(xAngle * xAngle + zAngle * zAngle)) * Matrix.Translation(pos);
        }
        public override void Draw(GameTime gametime)
        {
            if (myModel != null)
            {
                basicEffect.View = game.camera.View;
                effect.Parameters["World"].SetValue(basicEffect.World);
                effect.Parameters["View"].SetValue(basicEffect.View);
                effect.Parameters["Projection"].SetValue(basicEffect.Projection);
                effect.Parameters["cameraPos"].SetValue(new Vector4(game.camera.pos.X, game.camera.pos.Y, game.camera.pos.Z, 1));
                effect.Parameters["lightPntPos"].SetValue(new Vector4(50, 50, -50, 1));
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
            fire();
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            pos.X += (float)args.Delta.Translation.X / 100;
        }
    }
}