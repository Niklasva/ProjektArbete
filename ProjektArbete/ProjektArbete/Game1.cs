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
        AnimatedSprite animatedItem;
        Player player;
        Item[] items;

        //Muskontroll
        bool rightClickedOnItem = false;

        Texture2D mask;
        Color[,] maskData;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 320 * 3;
            graphics.PreferredBackBufferHeight = 180 * 3;
            this.IsMouseVisible = true;
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            items = Content.Load<Library.Item[]>(@"Data/ItemXML");
            foreach (Item item in items)
            {
                item.Initialize(Content.Load<Texture2D>(@item.TextureString));
            }
            Registry.items = items;

            player = new Player(this, Content.Load<Texture2D>(@"Images/AnimatedSprites/leftTexture"), Content.Load<Texture2D>(@"Images/AnimatedSprites/rightTexture"), 
                Content.Load<Texture2D>(@"Images/AnimatedSprites/downTexture"), Content.Load<Texture2D>(@"Images/AnimatedSprites/upTexture"), 
                Content.Load<Texture2D>(@"Images/AnimatedSprites/stillTexture"), Content.Load<Texture2D>(@"Images/Sprites/invBackground"), Window.ClientBounds);

            animatedItem = new AnimatedSprite(Content.Load<Texture2D>(@"Images/AnimatedSprites/threerings"), new Vector2(400, 20), 10, new Point(75, 75),
                new Point(0, 0), new Point(6, 8), 16);
            Registry.npcs = Content.Load<Library.NPC[]>(@"Data/npcs");
            Registry.dialogs = Content.Load<Library.Dialog[]>(@"Data/dialogs");
            Registry.rooms = Content.Load<Library.Room[]>(@"Data/rooms");
            Registry.currentRoom = Registry.rooms[0];

            Registry.currentRoom.LoadContent(this);

            mask = Content.Load<Texture2D>(@"Images/Backgrounds/backgroundmask");
            maskData = TextureTo2DArray(mask);
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
            Mousecontrol.update();
            //Test av rumbyte. Kod som ska användas till dörrar
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Delete))
            {
                Registry.currentRoom = Registry.rooms[1];
                Registry.currentRoom.LoadContent(this);
            }
            animatedItem.Update(gameTime, Window.ClientBounds);
            player.Update(this, gameTime, Window.ClientBounds);

            Registry.currentRoom.Update(gameTime, Window.ClientBounds);
            
            //Muskontroll
            if (!Registry.playerIsMoving && !Registry.inventoryInUse && Registry.currentRoom.isItemClickedInRoom())
            {
                Sprite item = Registry.currentRoom.getClickedItem().getSprite();
                if (Mousecontrol.inProximityToItem(item.Position, item.FrameSize))
                {
                    player.addItem(Registry.currentRoom.getClickedItem());
                    Registry.currentRoom.removeItem();
                    Registry.currentRoom.itemWasClicked();
                }
            }

            if (IntersectMask(maskData))
            {
                player.Stop();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(3f));
            Registry.currentRoom.Draw(gameTime, spriteBatch, player.position);
            player.Draw(gameTime, spriteBatch);
            animatedItem.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }

        static bool IntersectMask(Color[,] data)
        {
            // Konverterar spelarpositionen (Vector2/float) till int för att kunna använda den som arrayposition
            int x = (int)Math.Round(Registry.playerPosition.X);
            int y = (int)Math.Round(Registry.playerPosition.Y + 39);
            if (x < 1) x = 1;
            if (x > 299) x = 299;
            if (y < 1) y = 1;

            //letar efter !färg
            if (data[x, y].A == 0 || data[x + 20, y].A == 0)
            {
                return true;
            }
            return false;
        }

        private Color[,] TextureTo2DArray(Texture2D texture)
        {
            // GetData skapar av någon dum anledning enbart endimensionella arrayer
            // Den här funktionen konverterar en endimensionell array till en tvådimensionell.
            Color[] colors1 = new Color[texture.Width * texture.Height];
            texture.GetData(colors1); 

            Color[,] colors2 = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2[x, y] = colors1[x + y * texture.Width];

            return colors2;
        }


    }
}