﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.Devices.Sensors;

namespace Project
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    using SharpDX.Direct3D;

    public class LabGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public List<GameObject> gameObjects;
        private Stack<GameObject> addedGameObjects;
        private Stack<GameObject> removedGameObjects;
        private KeyboardManager keyboardManager;
        public KeyboardState keyboardState;

        public Sphere sphere =null;
        public Player player = null;
        public MazeLandscape mazeLandscape = null;

        public AccelerometerReading accelerometerReading;
        public GameInput input;
        public int score;
        public MainPage mainPage;
        public CompleteScreen completeScreen=null;

        // TASK 4: Use this to represent difficulty
        public float difficulty;

        // Represents the camera's position and orientation
        public Camera camera;

        // Graphics assets
        public Assets assets;

        // Random number generator
        public Random random;

        // World boundaries that indicate where the edge of the screen is for the camera.
        public float boundaryLeft;
        public float boundaryRight;
        public float boundaryTop;
        public float boundaryBottom;
        public float boundaryFront;
        public float boundaryBack;

        public bool started = false;
        public bool resumed = false;

        public int mazeMaxDimension =100;

        public int mazeDimension = 15;

        public int mazeSeed;//123;

        public float accelerationFraction;

        public float gravityFactor =0.2f;

        public Vector2 currentPositionInMazeArray;

        public Double currentGameTimeSecond;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabGame" /> class.
        /// </summary>
        public LabGame(MainPage mainPage)
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Create the keyboard manager
            keyboardManager = new KeyboardManager(this);
            assets = new Assets(this);
            random = new Random(Environment.TickCount);
            input = new GameInput();

            // Set boundaries.
            boundaryLeft = -4.5f;
            boundaryRight = 4.5f;
            boundaryTop = 4;
            boundaryBottom = -4.5f;
            boundaryFront = -4.5f;
            boundaryBack = 4.5f;

            // Initialise event handling.
            input.gestureRecognizer.Tapped += Tapped;
            input.gestureRecognizer.ManipulationStarted += OnManipulationStarted;
            input.gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;
            input.gestureRecognizer.ManipulationCompleted += OnManipulationCompleted;

            this.mainPage = mainPage;

            score = 0;
            difficulty = 1;
            currentGameTimeSecond = 0;
            //make it safe
            mazeSeed=random.Next(1,Int32.MaxValue-1);
        }
        public void reCreate()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Initialize();
            LoadContent();
            completeScreen = null;
            currentGameTimeSecond = 0;

        }

        protected override void LoadContent()
        {
            // Initialise game object containers.
            gameObjects = new List<GameObject>();
            addedGameObjects = new Stack<GameObject>();
            removedGameObjects = new Stack<GameObject>();
           
            // Create game objects.
            //gameObjects.Add(player);
            gameObjects.Add(mazeLandscape);
            gameObjects.Add(sphere);
            //gameObjects.Add(new EnemyController(this));

            // Create an input layout from the vertices


            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Lab 4";
            camera = new Camera(this);
            mazeLandscape = new MazeLandscape(this,mazeDimension,mazeSeed);
            player = new Player(this);
            sphere = new Sphere(Content.Load<Model>("sphere"), this);
            camera.setStartingPosView();
            

            //testModel = Content.Load<Model>("woodsphere_obj");
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            currentPositionInMazeArray = sphere.PositionInMaze(sphere.pos);
            if (started&&resumed)
            {
                currentGameTimeSecond += gameTime.ElapsedGameTime.TotalSeconds;
                keyboardState = keyboardManager.GetState();
                flushAddedAndRemovedGameObjects();
                accelerometerReading = input.accelerometer.GetCurrentReading();
                for (int i = 0; i < gameObjects.Count; i++)
                {
                   gameObjects[i].Update(gameTime);
                }

                mainPage.UpdateScore(score);

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                    this.Dispose();
                    App.Current.Exit();
                }
                camera.Update();
                // Handle base.Update
                


            }

            if (currentPositionInMazeArray.X == mazeLandscape.maze.destX &&
                currentPositionInMazeArray.Y == mazeLandscape.maze.destY)
            {
                if (completeScreen == null)
                {
                    completeScreen = new CompleteScreen(mainPage, this, currentGameTimeSecond);
                }
                if( !mainPage.Children.Contains(completeScreen))
                {
                    mainPage.Children.Add(completeScreen);
                }
            }

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            if (started)
            {
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(Color.CornflowerBlue);

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Draw(gameTime);
                }
                sphere.Draw(gameTime);
                //testModel.Draw(GraphicsDevice, world, view, projection);
            }
            // Handle base.Draw
            base.Draw(gameTime);
        }
        // Count the number of game objects for a certain type.
        public int Count(GameObjectType type)
        {
            int count = 0;
            foreach (var obj in gameObjects)
            {
                if (obj.type == type) { count++; }
            }
            return count;
        }

        // Add a new game object.
        public void Add(GameObject obj)
        {
            if (!gameObjects.Contains(obj) && !addedGameObjects.Contains(obj))
            {
                addedGameObjects.Push(obj);
            }
        }

        // Remove a game object.
        public void Remove(GameObject obj)
        {
            if (gameObjects.Contains(obj) && !removedGameObjects.Contains(obj))
            {
                removedGameObjects.Push(obj);
            }
        }

        // Process the buffers of game objects that need to be added/removed.
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedGameObjects.Count > 0) { gameObjects.Add(addedGameObjects.Pop()); }
            while (removedGameObjects.Count > 0) { gameObjects.Remove(removedGameObjects.Pop()); }
        }

        public void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            // Pass Manipulation events to the game objects.

        }

        public void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            // Pass Manipulation events to the game objects.
            foreach (var obj in gameObjects)
            {
                obj.Tapped(sender, args);
            }
        }

        public void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            camera.pos.Z = camera.pos.Z * args.Delta.Scale;
            // Update camera position for all game objects
            foreach (var obj in gameObjects)
            {
                if (obj.basicEffect != null) { obj.basicEffect.View = camera.View; }
                obj.OnManipulationUpdated(sender, args);
            }
        }

        public void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
        }


     
    }
}
