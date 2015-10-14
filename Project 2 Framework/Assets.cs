using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Project
{
    using SharpDX.Toolkit.Graphics;
    public class Assets
    {
        LabGame game;

        public Assets(LabGame game)
        {
            this.game = game;
        }

        // Dictionary of currently loaded models.
        // New/existing models are loaded by calling GetModel(modelName, modelMaker).
        public Dictionary<String, MyModel> modelDict = new Dictionary<String, MyModel>();

        // Load a model from the model dictionary.
        // If the model name hasn't been loaded before then modelMaker will be called to generate the model.
        public delegate MyModel ModelMaker();
        public MyModel GetModel(String modelName, ModelMaker modelMaker)
        {
            if (!modelDict.ContainsKey(modelName))
            {
                modelDict[modelName] = modelMaker();
            }
            return modelDict[modelName];
        }

        // Create a cube with one texture for all faces.
        public MyModel CreateTexturedCube(String textureName, float size)
        {
            return CreateTexturedCube(textureName, new Vector3(size, size, size));
        }

        public MyModel CreateTexturedCube(String texturePath, Vector3 size)
        {
            Vector3 frontNorm = new Vector3(0, 0, -1);
            Vector3 backNorm = -frontNorm;
            Vector3 leftNorm = new Vector3(-1, 0, 0);
            Vector3 rightNorm = -leftNorm;
            Vector3 topNorm = new Vector3(0, 1, 0);
            Vector3 bottomNorm = -topNorm;
            VertexPositionNormalColor[] shapeArray = new VertexPositionNormalColor[]{
            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), frontNorm, Color.Yellow), // Front
            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), frontNorm, Color.Yellow),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), frontNorm, Color.Yellow),
            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), frontNorm, Color.Yellow),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), frontNorm, Color.Yellow),
            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), frontNorm, Color.Yellow),

            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), backNorm, Color.Red), // BACK
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), backNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), backNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), backNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), backNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), backNorm, Color.Red),

            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), topNorm, Color.Red), // Top
            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), topNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), topNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), topNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), topNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), topNorm, Color.Red),

            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), bottomNorm, Color.Red), // Bottom
            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), bottomNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), bottomNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), bottomNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), bottomNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), bottomNorm, Color.Red),

            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), leftNorm, Color.Red), // Left
            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), leftNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), leftNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), leftNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), leftNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), leftNorm, Color.Red),

            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), rightNorm, Color.Red), // Right
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), rightNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), rightNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), rightNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), rightNorm, Color.Red),
            new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), rightNorm, Color.Red),
            };

            for (int i = 0; i < shapeArray.Length; i++)
            {
                shapeArray[i].Position.X *= size.X / 2;
                shapeArray[i].Position.Y *= size.Y / 2;
                shapeArray[i].Position.Z *= size.Z / 2;
            }

            float collisionRadius = (size.X + size.Y + size.Z) / 6 ;
            return new MyModel(game, shapeArray, texturePath, collisionRadius);
        }
    }
}
