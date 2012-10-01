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
using System.IO;

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Form1 form;
        List<Sprite> spriteList = new List<Sprite>();
        Texture2D texture;
        MouseState currentMouseState;
        MouseState lastMouseState;
        Vector2 mousePosition;
        Texture2D bgTexture;
        Stream bgStream;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            form = new Form1();
            form.ShowInTaskbar = false;
            form.Show();
            this.IsMouseVisible = true;
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
            bgTexture = Content.Load<Texture2D>(@"images/null");
            texture = Content.Load<Texture2D>(@"images/point");
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

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            mousePosition.X = currentMouseState.X - texture.Width / 2;
            mousePosition.Y = currentMouseState.Y - texture.Height / 2;

            // Läser in bakgrund från en stream i formobjektet
            bgStream = form.bgStream;

            // Skapar en punkt när man klickar om fönstret är aktivt
            if (currentMouseState.LeftButton == ButtonState.Released && 
                lastMouseState.LeftButton == ButtonState.Pressed && 
                System.Windows.Forms.Form.ActiveForm == (System.Windows.Forms.Control.FromHandle(Window.Handle) as System.Windows.Forms.Control))
            {
                spriteList.Add(new Sprite(texture, mousePosition));
            }
            
            // Byter bakgrund när man högerklickar
            // NOTERA: Dum lösning, programmet kraschar ibland när man högerklickar och jag vill nog hellre ha en refreshknapp i Form1.
            if (currentMouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed)
            {
                //Konverterar bgStream till Texture2D
                bgTexture = Texture2D.FromStream(GraphicsDevice, bgStream);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.End();

            // Ritar ut punkter
            foreach (Sprite s in spriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }
        }
    }
}
