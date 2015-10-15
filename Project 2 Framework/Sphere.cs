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

        Model model;
        Matrix view;
        Matrix world;
        Matrix projection;
        BoundingSphere modelBounds;

        public Sphere(Model sphere,LabGame game)
        {
            this.game = game;
            model = sphere;
            BasicEffect.EnableDefaultLighting(model,true);
            const float MaxModelSize = 10.0f;
            var scaling = MaxModelSize / modelBounds.Radius;
            //view = Matrix.LookAtRH(new Vector3(0, 0, MaxModelSize * 2.5f), new Vector3(0, 0, 0), Vector3.UnitY);
            //projection = Matrix.PerspectiveFovRH(0.9f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, MaxModelSize * 10.0f);
            //world = Matrix.Translation(-modelBounds.Center.X, -modelBounds.Center.Y, -modelBounds.Center.Z);
        
            view = game.camera.View;
            projection = game.camera.Projection;
            modelBounds = model.CalculateBounds();
            pos = new Vector3(-modelBounds.Center.X,-modelBounds.Center.Y,-modelBounds.Center.Z);
            world = Matrix.Translation(pos.X, pos.Y, pos.Z) * Matrix.Scaling(1f);
            //effect = game.Content.Load<Effect>("Phong");
        }

        public override void Draw(GameTime gametime)
        {
            model.Draw(game.GraphicsDevice, world, view, projection);
        }

        public override void Update(GameTime gameTime)
        {
            view = game.camera.View;
            projection = game.camera.Projection;

        }

    }
}
