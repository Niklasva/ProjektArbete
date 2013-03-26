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
        Player player;
        Item[] items;
        Menu menu;
        Texture2D cursor;
        Texture2D cursor2;
        Texture2D waitcursor;
        bool wait = false;
        MouseState mouseState;
        
        enum StateOfGame {menu, game};
        StateOfGame stateOfGame = StateOfGame.menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;                              // SPELET ÄR INTE I FULLSCREEN (det ser konstigt ut då)
            graphics.PreferredBackBufferWidth = 320 * 3;                // Ändrar storleken på fönstret så att det passar 3x skalningen
            graphics.PreferredBackBufferHeight = 180 * 3;
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
            cursor = Content.Load<Texture2D>(@"Images/MenuImages/cursor");
            cursor2 = Content.Load<Texture2D>(@"Images/MenuImages/cursor2");
            waitcursor = Content.Load<Texture2D>(@"Images/MenuImages/wait");
            player = new Player(this, Content.Load<Texture2D>(@"Images/AnimatedSprites/vanligTexture"), Content.Load<Texture2D>(@"Images/AnimatedSprites/militarTexture"),
                Content.Load<Texture2D>(@"Images/AnimatedSprites/militarTexture"), Content.Load<Texture2D>(@"Images/AnimatedSprites/militarTexture"),
                Content.Load<Texture2D>(@"Images/AnimatedSprites/stillTexture"), Content.Load<Texture2D>(@"Images/AnimatedSprites/stillTexture"), Content.Load<Texture2D>(@"Images/Sprites/invBackground"), Window.ClientBounds);

            menu = new Menu(Content.Load<Texture2D>(@"Images/MenuImages/splash"), Content.Load<Texture2D>(@"Images/MenuImages/OPENBUTTON"), Content.Load<Texture2D>(@"Images/MenuImages/NEWBUTTON"));

            Registry.npcs = Content.Load<Library.NPC[]>(@"Data/npcs2");              // Här händer viktiga saker. NPC-listan i registret skapas
            Registry.dialogs = Content.Load<Library.Dialog[]>(@"Data/dialogs2");     // Dialoglistan i Registry skapas
            Registry.rooms = Content.Load<Library.Room[]>(@"Data/rooms2");           // Rum i Registry skapas
            Registry.currentRoom = Registry.rooms[7];                               // Startrummet
            Registry.currentRoom.LoadContent(this);                                 // GO!
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
            // När spelet är inaktivt eller pausat går det inte att göra saker (duh)
            if (this.IsActive && !Registry.pause)
            {
                Mousecontrol.update();
                wait = false;
            }
            else
            {
                wait = true;
            }
            if (stateOfGame == StateOfGame.game)
            {
                

                player.Update(this, gameTime, Window.ClientBounds);
                Registry.changingRoom = false;               
                Registry.currentRoom.Update(gameTime, Window.ClientBounds);

                //Muskontroll
                if (!player.IsMoving && !player.InventoryInUse && Registry.currentRoom.isItemClickedInRoom())
                {
                    Sprite item = Registry.currentRoom.getClickedItem().getSprite();
                    if (Mousecontrol.inProximityToItem(item.Position, item.FrameSize))
                    {
                        player.addItem(Registry.currentRoom.getClickedItem());
                        Registry.currentRoom.removeItem();
                        Registry.currentRoom.itemWasClicked();
                    }
                }

                // Kollar om spelaren rör sig utanför spelområdet
                if (IntersectMask(Registry.currentRoom.getMask()) != Vector2.Zero)
                {
                    // Rättar till spelarens position så att man inte går utanför spelområdet (det icke-transparenta i masken)
                    player.Stop(IntersectMask(Registry.currentRoom.getMask()));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    stateOfGame = StateOfGame.menu;
            }
            else if (stateOfGame == StateOfGame.menu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    stateOfGame = StateOfGame.game;
                menu.Update(gameTime, Window.ClientBounds);
                if (menu.ClickedOnNew)
                    stateOfGame = StateOfGame.game;
                if (menu.ClickedOnOpen)
                    load();
            }

            // Ändrar musikvolymen om man pratar med folk
            if (Registry.pause)
            {
                MediaPlayer.Volume = 0.5f;          // Låg volym
            }
            else
            {
                MediaPlayer.Volume = 1;             // Hög volym (MINA ÖRON)
            }

            mouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(3f));
            if (Mousecontrol.hover)
            {
                spriteBatch.Draw(
                    cursor2,
                   new Vector2(mouseState.X / 3 - 7, mouseState.Y / 3 - 7),
                    new Rectangle(0, 0,
                    cursor2.Width,
                    cursor2.Height),
                    Color.White,
                    0,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0.1f);
            }
            else if (wait)
            {
                spriteBatch.Draw(
                waitcursor,
                new Vector2(mouseState.X / 3 - 7, mouseState.Y / 3 - 7),
                new Rectangle(0, 0,
                waitcursor.Width,
                waitcursor.Height),
                Color.White,
                0,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0f);
            }
            else
            {
                spriteBatch.Draw(
                    cursor,
                    new Vector2(mouseState.X / 3 - 7, mouseState.Y / 3 - 7),
                    new Rectangle(0, 0,
                    cursor.Width,
                    cursor.Height),
                    Color.White,
                    0,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0f);
            }
            if (stateOfGame == StateOfGame.game)
            {
                Registry.currentRoom.Draw(gameTime, spriteBatch);
                player.Draw(gameTime, spriteBatch);
            }

            if (stateOfGame == StateOfGame.menu)
            {
                menu.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Kolla om spelaren är där hen inte borde vara
        /// </summary>
        /// <param name="data">2D-färgarray</param>
        /// <returns>Var "väggen" är jämfört med spelaren</returns>
        static Vector2 IntersectMask(Color[,] data)
        {
            Vector2 where = Vector2.Zero;
            // Konverterar spelarpositionen (Vector2/float) till int för att kunna använda den som arrayposition
            int x = (int)Math.Round(Registry.playerPosition.X);
            int y = (int)Math.Round(Registry.playerPosition.Y + 67);
            if (x < 1) x = 1;
            if (x > 273) x = 273;
            if (y < 1) y = 1;
            if (y > 178) y = 178;

            // Letar efter genomsynlighet i närheten av spelaren
            if (data[x - 1, y].A == 0)
            {
                where.X = -1;
            } 
            if (data[x + 27, y].A == 0)
            {
                where.X = 1;
            }
            if (data[x, y + 1].A == 0)
            {
                where.Y = 1;
            }
            if (data[x, y - 1].A == 0)
            {
                where.Y = -1;
            }

            // Returnerar var "väggen" är jämfört med spelaren
            return where;
        }

        private void load()
        {
            Registry.load(this);
            foreach (Item item in Registry.itemsInInventory)
            {
                Item itemToBeAdded = new Item();
                itemToBeAdded.loadNewItem(item);
                itemToBeAdded.Initialize(Content.Load<Texture2D>(@itemToBeAdded.TextureString));
                player.addItem(itemToBeAdded);
            }

            player.position = Registry.playerPosition;

            Registry.currentRoom.LoadContent(this);

            stateOfGame = StateOfGame.game;
            
        }
    }
}