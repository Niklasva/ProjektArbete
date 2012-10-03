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
        Stream bgStream;

        List<Sprite> spriteList = new List<Sprite>();

        Texture2D bgTexture;
        Texture2D texture;

        MouseState currentMouseState;
        MouseState lastMouseState;
        Vector2 mousePositionPoint;
        Vector2 mousePosition;
        
        

        private List<Vector2> vertexes;
        const int maxLines = 32;

        public Game1()
        {
            vertexes = new List<Vector2>();
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
            PolygonDraw.LoadContent(GraphicsDevice);
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
            KeyboardState keyboardState = Keyboard.GetState();
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            mousePositionPoint.X = currentMouseState.X - texture.Width / 2;
            mousePositionPoint.Y = currentMouseState.Y - texture.Height / 2;


            mousePosition.X = currentMouseState.X;
            mousePosition.Y = currentMouseState.Y;

            // Läser in bakgrund från en stream i formobjektet
            bgStream = form.bgStream;

            // Skapar en punkt när man klickar om fönstret är aktivt och mode är draw
            if (currentMouseState.LeftButton == ButtonState.Released && 
                lastMouseState.LeftButton == ButtonState.Pressed && 
                System.Windows.Forms.Form.ActiveForm == (System.Windows.Forms.Control.FromHandle(Window.Handle) as System.Windows.Forms.Control) &&
                Form1.mode == Form1.Mode.Drawing)
            {
                vertexes.Add(mousePosition);
                spriteList.Add(new Sprite(texture, mousePositionPoint));
                
            }
            
            // Byter bakgrund när man högerklickar
            // NOTERA: Dum lösning, programmet kraschar ibland när man högerklickar och jag vill nog hellre ha en refreshknapp i Form1.
            if (currentMouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed)
            {
                // Konverterar bgStream till Texture2D
                bgTexture = Texture2D.FromStream(GraphicsDevice, bgStream);
            }
            

            // Tar bort punkter och linjer med deletetangenten
            if (keyboardState.IsKeyDown(Keys.Delete))
            {
                vertexes.Clear();
                spriteList.Clear();
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
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            DrawLines();
            spriteBatch.End();
            base.Draw(gameTime);

            // Ritar ut punkter
            foreach (Sprite s in spriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }
        }

        private void DrawLines()
        {
            for (int i = 0; i < vertexes.Count - 1; i++)
            {
                spriteBatch.DrawLineSegment(vertexes[i], vertexes[i + 1], Color.Orange, 3);
            }
        }
    }
}
