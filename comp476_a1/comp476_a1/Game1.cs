using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace comp476_a1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        SpriteFont font;

        public static Vector2 screenDimensions { get; private set; }
        public static Random seed { get; private set; }
        public static bool heuristic { get; private set; }  //  true is kinetic, false is steering        
        private KeyboardState lastKeyboardState;
        private float fovy;

        public enum behaviour
        {
            PLAYER = 0,
            TAG
        }

        public static behaviour currentBehaviour { get; private set; }

        #region Camera
        public static Vector3 thirdPersonReference { get; private set; }
        public static Vector3 cameraPosition { get; private set; }
        public static Vector3 lookAt { get; private set; }

        public static Matrix view { get; private set; }
        public static Matrix projection { get; private set; }
        public static BoundingFrustum frustrum { get; private set; }
        
        
        
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            spriteManager = new SpriteManager(this);
            this.Components.Add(spriteManager); // Adds the SpriteManager class to be called for update and draw

            Content.RootDirectory = "Content";
            screenDimensions = new Vector2(800.0f, 600.0f);
            graphics.PreferredBackBufferWidth = (int)screenDimensions.X;
            graphics.PreferredBackBufferHeight = (int)screenDimensions.Y;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            seed = new Random();    //  Initializes the seed for random number generation
            heuristic = true;
            currentBehaviour = behaviour.PLAYER;


            //  camera settings
            cameraPosition = new Vector3(50.0f, 0.0f, 0.0f); //cameraPosition = {X:21.91004 Y:15.44271 Z:-20.90045}
            lookAt = new Vector3(0.0f, 0.0f, 0.0f); // lookAt = {X:9.221636 Y:6.635674 Z:-11.73702}            
            view = Matrix.CreateLookAt(cameraPosition, lookAt, new Vector3(0.0f, 1.0f, 0.0f));
            fovy = MathHelper.Pi / 2.0f;
            projection = Matrix.CreatePerspectiveFieldOfView(fovy, 800f / 600f, 0.1f, 50f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] keys = keyboardState.GetPressedKeys();

            foreach (Keys key in keys)
            {
                if (CheckPressedKeys(key))
                {
                    switch (key)
                    {
                        case Keys.Escape:
                            this.Exit();
                            break;
                        case Keys.F1:
                            fovy += 0.05f;
                            projection = Matrix.CreatePerspectiveFieldOfView(fovy, 800f / 600f, 0.1f, 50f);
                            break;
                        case Keys.F2:
                            fovy -= 0.05f;
                            projection = Matrix.CreatePerspectiveFieldOfView(fovy, 800f / 600f, 0.1f, 50f);
                            break;
                        case Keys.D1:
                            SpriteManager.Teleport();
                            break;
                        case Keys.X:
                            heuristic = !heuristic;
                            break;
                        case Keys.P:
                            currentBehaviour = behaviour.PLAYER;
                            spriteManager.Initialize();
                            break;
                        case Keys.T:
                            currentBehaviour = behaviour.TAG;
                            spriteManager.Initialize();
                            break;
                    }
                }
            }

            lastKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.DrawString(font, String.Format("Controls:"), new Vector2(10, 0), Color.Black);
            spriteBatch.DrawString(font, String.Format("(X) Heuristic: {0}", (heuristic) ? "1" : "2"), new Vector2(10, 20), Color.Black);
            spriteBatch.DrawString(font, String.Format("(F1/F2) Zoom: {0}", fovy), new Vector2(10, 40), Color.Black);
            spriteBatch.DrawString(font, String.Format("Legend:"), new Vector2(10, 80), Color.Black);
            spriteBatch.DrawString(font, String.Format("Red: Seeker"), new Vector2(10, 100), Color.Black);
            spriteBatch.DrawString(font, String.Format("Blue: Evaders"), new Vector2(10, 120), Color.Black);
            spriteBatch.DrawString(font, String.Format("Green: Player"), new Vector2(10, 140), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private bool CheckPressedKeys(Keys key)
        {
            bool keyPressed = false;

            if (lastKeyboardState.IsKeyUp(key))
            {
                keyPressed = true;
            }

            return keyPressed;
        }
    }
}
