using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Library
{
    public class Room
    {
        /// <summary>
        /// Rum
        /// </summary>
        /// 
        public string backgroundID;
        public string[] npcID;
        public string[] itemID;
        public string[] itemPosition;
        public List<Door> doors = new List<Door>();
        public string song;
        private List<NPC> npcs = new List<NPC>();
        private List<Item> items = new List<Item>();
        private Game game;

        private Texture2D background;
        private Texture2D mask;
        private Color[,] maskData;
        private Texture2D foreground;
        private bool visited = false;

        //Muskontroll 
        //Bool för att lägga till föremål i spelaren och ta bort från rummet
        private bool isItemClicked;
        //Och föremålet i fråga
        private Item itemClicked;

        /// <summary>
        /// Laddar rummets och de saker i rummets innehåll
        /// </summary>
        public void LoadContent(Game game)
        {
            // Första gången man besöker ett rum, laddas det in
            // Om man redan har besökt ett rum är allting där det var när man lämnade rummet
            if (visited == false)
            {
                this.background = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID);
                this.mask = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID + "mask");
                this.foreground = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID + "fg");
                this.maskData = TextureTo2DArray(mask);
                this.game = game;

                isItemClicked = false;
                foreach (string id in itemID)
                {
                    Item itemToBeAdded = new Item();
                    itemToBeAdded.loadNewItem(Registry.items[int.Parse(id)]);
                    itemToBeAdded.Initialize(game.Content.Load<Texture2D>(@itemToBeAdded.TextureString));
                    items.Add(itemToBeAdded);
                }
                foreach (string id in npcID)
                {
                    npcs.Add(Registry.npcs[int.Parse(id)]);
                }
                foreach (NPC item in npcs)
                {
                    item.loadContent(game);
                }
                foreach (Door item in doors)
                {
                    item.LoadContent(game);
                }

                int i = 0;
                foreach (Item item in items)
                {
                    string[] temp = itemPosition[i].Split(new char[] { ',' }, 2);
                    item.setPosition(new Vector2(float.Parse(temp[0]), float.Parse(temp[1])));
                    i++;
                }
                visited = true;
            }
            if (song != "null")
            {
                if (Registry.musbol == true)
                {
                    if (!Registry.music.IsDisposed)
                    {
                        MediaPlayer.Stop();
                        Registry.music.Dispose();
                    }
                }
                Registry.music = game.Content.Load<Song>(@"Sound/BGM/" + song);
                MediaPlayer.Play(Registry.music);
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {

            doorUpdate();
            foreach (NPC item in npcs)
            {
                item.Update(gameTime, clientBounds);
                if (item.GiveItem)
                {
                    Item itemToBeAdded = new Item();
                    itemToBeAdded.loadNewItem(Registry.items[item.ItemID]);
                    itemToBeAdded.Initialize(game.Content.Load<Texture2D>(@itemToBeAdded.TextureString));
                    itemToBeAdded.setPosition(new Vector2(item.position.X, item.position.Y + 61));
                    items.Add(itemToBeAdded);
                    item.resetItem();
                }
            }
            mousecontrolUpdate();

        }

        public bool giveNPCItem(Item itemClickedOn)
        {
            bool temp = false;
            foreach (NPC npc in npcs)
            {
                if (Mousecontrol.clickedOnItem(npc.getPosition, npc.getFrameSize, true) && (itemClickedOn.name == npc.wantedItem))
                {
                    npc.givenItem();
                    temp = true;
                }
            }
            return temp;
        }
        /// <summary>
        /// Ritar ut allt som ska ritas ut i rummet
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            updateLayerPosition();

            // ritar ut alla objekt i rummet
            foreach (Item item in items)
            {
                item.Draw(gameTime, spriteBatch);
            }

            // dörrar
            foreach (Door item in doors)
            {
                item.Draw(gameTime, spriteBatch);
            }

            // bakgrunden
            spriteBatch.Draw(background,
                 Vector2.Zero,
                 new Rectangle(0, 0,
                 game.Window.ClientBounds.Width,
                 game.Window.ClientBounds.Height),
                 Color.White,
                 0,
                 Vector2.Zero,
                 1,
                 SpriteEffects.None,
                 0.333333333333f);

            // förgrunden
            spriteBatch.Draw(foreground,
                 Vector2.Zero,
                 new Rectangle(0, 0,
                 game.Window.ClientBounds.Width,
                 game.Window.ClientBounds.Height),
                 Color.White,
                 0,
                 Vector2.Zero,
                 1,
                 SpriteEffects.None,
                 0.003f);



            if (!Registry.inventoryInUse)
            {
                string textToDraw = null;
                bool drawText = false;
                bool clickedOnItem = false;
                foreach (Item item in items)
                {
                    if (Mousecontrol.rightClickedOnItem(item))
                    {
                        clickedOnItem = true;
                    }
                }
                if ((Mouse.GetState().RightButton == ButtonState.Pressed && clickedOnItem))
                {
                    textToDraw = Mousecontrol.getDescription();
                    drawText = true;
                }
                else if (clickedOnItem)
                {
                    textToDraw = Mousecontrol.getName();
                    drawText = true;
                }
                if (drawText)
                {
                    spriteBatch.DrawString(game.Content.Load<SpriteFont>(@"textfont"), textToDraw,
                            new Vector2(game.Window.ClientBounds.Width / 6 - 4f * (textToDraw.Count()),
                                0), Color.White);
                }
            }

            // npc:er
            foreach (NPC npc in npcs)
            {
                npc.Draw(gameTime, spriteBatch, Registry.playerPosition);
            }
        }

        public bool isItemClickedInRoom()
        {
            return isItemClicked;
        }
        public void itemWasClicked()
        {
            isItemClicked = false;
        }
        public Item getClickedItem()
        {
            return itemClicked;
        }
        public void removeItem()
        {
            items.Remove(itemClicked);
        }
        public List<Item> getItems()
        {
            return items;
        }
        public void addItem(Item x)
        {
            Item itemToBeAdded = x;
            itemToBeAdded.loadNewItem(x);
            itemToBeAdded.Initialize(game.Content.Load<Texture2D>(@itemToBeAdded.TextureString));
            items.Add(itemToBeAdded);
        }

        public void mousecontrolUpdate()
        {
            //Om man klickar ner musen
            //Om man klickar på ett föremål i rummet
            bool clickedOnItem = false;
            Item tempItem = new Item();
            isItemClicked = false;
            foreach (Item item in items)
            {
                if (Mousecontrol.clickedOnItem(item.getSprite().Position, item.getSprite().FrameSize, Mousecontrol.clicked()))
                {
                    clickedOnItem = true;
                    tempItem = item;
                }
            }
            if (clickedOnItem)
            {
                //Kan man plocka upp föremålet?
                if (tempItem.isPickable)
                {
                    //Ändra en bool som anger om ett föremål har blivit klickat på
                    isItemClicked = true;
                    //Föremålet som klickades på tilldelas till en variabel i rummet
                    itemClicked = tempItem;
                }
            }
        }

        public void doorUpdate()
        {
            bool changeRoom = false;
            int nextRoomId = 0;
            foreach (Door item in doors)
            {
                if (Mousecontrol.inProximityToItem(item.position, item.getSprite().FrameSize) && Mousecontrol.clickedOnItem(item.getSprite().Position, item.getSprite().FrameSize, Mousecontrol.clicked()))
                {
                    changeRoom = true;
                    nextRoomId = int.Parse(item.nextRoomID);
                    Registry.nextRoomDoorPosition = item.door2Position;
                    Registry.changingRoom = true;
                }
            }
            if (changeRoom)
            {

                Registry.currentRoom = Registry.rooms[nextRoomId];
                Registry.currentRoom.LoadContent(game);
                changeRoom = false;

            }

            for (int j = 0; j < items.Count; j++)
            {
                List<Item> itemsInRoom = items;
                if (!itemsInRoom[j].getActive())
                    itemsInRoom.RemoveAt(j);
            }
                
            
        }

        /// <summary>
        /// GetData skapar av någon dum anledning enbart endimensionella arrayer
        /// Den här funktionen konverterar en endimensionell array till en tvådimensionell.
        /// </summary>
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


        public Color[,] getMask()
        {
            return this.maskData;
        }

        private void updateLayerPosition()
        {
            foreach (Item item in items)
            {
                item.setLayerPosition((1 - ((item.getSprite().Position.Y + item.getSprite().Texture.Height) / 180)) / 3);
            }
        }

        public bool getVisited()
        {
            return visited;
        }

        public void setVisited()
        {
            visited = true;
        }
        public void removeItem(Item x)
        {
            items.Remove(x);
        }


    }
}
