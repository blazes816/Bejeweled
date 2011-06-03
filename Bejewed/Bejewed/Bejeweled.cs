using System;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Bejeweled.States;
using System.Reflection;

namespace Bejeweled
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Bejeweled : Microsoft.Xna.Framework.Game
    {
        public enum GameState {StartMenu, GameMenu, Playing};
        public GameState CurrentState = GameState.Playing;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // State handlers
        private Hashtable gameStates = new Hashtable();

        public Bejeweled()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 500;
            graphics.IsFullScreen = false;
            Window.Title = "Bejeweled";
            Content.RootDirectory = "Content";
            //this.States = new IState[3] { new StartMenu(), new GameMenu(), new Playing() };

            this.gameStates.Add(GameState.StartMenu, new StartMenu());
            this.gameStates.Add(GameState.GameMenu, new GameMenu());
            this.gameStates.Add(GameState.Playing, new Playing());
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            // Set spritebatches and contents to the world!
            foreach (IState state in this.States())
            {
                state.SpriteBatch = spriteBatch;
                state.Content = Content;
            }

            dispatchEvent("LoadContent");

        }

        private void dispatchEvent(String hook, GameTime gameTime)
        {
            foreach(IState state in this.States())
            {
                Type thisType = state.GetType();
                MethodInfo method = thisType.GetMethod(hook);
                object[] args;

                switch (hook)
                {
                    case "Update":
                        {
                            args = new object[1];
                            args[0] = gameTime;
                            break;
                        }
                    default:
                        {
                            args = new object[0];
                            break;
                        }
                }

                method.Invoke(state, args);
            }
        }

        private void dispatchEvent(String hook)
        {
            this.dispatchEvent(hook, new GameTime());
        }

        public IEnumerable States()
        {
            foreach (GameState key in this.gameStates.Keys)
            {
                yield return this.gameStates[key];
            }
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            dispatchEvent("Update", gameTime);

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
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            // Call Board Draw hook
            //this.board.Draw();
            switch (CurrentState)
            {
                case GameState.Playing : {
                    IState playing = (IState)this.gameStates[GameState.Playing];
                    playing.Draw();
                    break;
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
