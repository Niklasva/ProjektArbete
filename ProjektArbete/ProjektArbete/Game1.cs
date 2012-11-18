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
using Library;

namespace ProjektArbete
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Dialog testDialog;

        Sprite item;
        AnimatedSprite animatedItem;
        NPC npc1;
        Player player = new Player();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 320 * 2;
            graphics.PreferredBackBufferHeight = 180 * 2;
            Content.RootDirectory = "Content";
            Registry.playerPosition = player.position;
            
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

            item = new Sprite(Content.Load<Texture2D>(@"Images/Sprites/object"), new Vector2(40, 60), 10);
            animatedItem = new AnimatedSprite(Content.Load<Texture2D>(@"Images/AnimatedSprites/threerings"), new Vector2(400, 20), 10, new Point(75, 75),
                new Point(0, 0), new Point(6, 8), 16);
            
            //testDialog = new Dialog(new Vector2(20, 20), new Vector2(10, 120), "1", Content.Load<SpriteFont>(@"font"));
            //testDialog.LoadDialog();
            npc1 = Content.Load<NPC>("NPC1");
            npc1.setDialog(Content.Load<Dialog>("Dialog1"));

            //test
            System.Console.WriteLine(npc1.name);
            System.Console.WriteLine(npc1.position.ToString());
            System.Console.WriteLine(npc1.getDialog());
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
            animatedItem.Update(gameTime, spriteBatch);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            item.Draw(gameTime, spriteBatch);
            animatedItem.Draw(gameTime, spriteBatch);
            //testDialog.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}
