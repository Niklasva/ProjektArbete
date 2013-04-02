using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Library
{        
    /// <summary>
    /// Rum
    /// </summary>
    public class Room
    {
        //XML
        public string backgroundID;                         // Bakgrundens filnamn (används även till förgrund och mask)
        public string[] npcID;                              // NPC:ers nummer (hämtar ur Registry)
        public string[] itemID;                             // ID nummer på de items som finns i rummet (hämtar ur Registry)
        public string[] itemPosition;                       // Positioner för tidigare nämnda items
        public List<Door> doors = new List<Door>();         // Rummets dörrar
        public string song;                                 // Rummets bakgrundsmusik (sätt som null om det inte ska vara någon ny musik i rummet)
        public string dialogID;                             // Rumdialog
        public bool save;                                   // Om spelet ska spara
        
        private List<NPC> npcs = new List<NPC>();
        private List<Item> items = new List<Item>();
        private Game game;
        private Texture2D background;
        private Texture2D mask;
        private Color[,] maskData;
        private Texture2D foreground;
        private bool visited = false;
        private Dialog roomDialog;
        private bool dialogIsActive = false;
        private AnimatedSprite animbackground;
        private SoundEffect dialogSound;
        private bool soundhasplayed = true;
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
                soundhasplayed = false;
                this.background = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID);
                animbackground = new AnimatedSprite(background, new Vector2(0,0) , 0, new Point(background.Width / (background.Width / 320), background.Height), new Point(0, 0), new Point(2, 1), 100);
                this.mask = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID + "mask");
                this.foreground = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID + "fg");
                this.maskData = TextureTo2DArray(mask);
                this.game = game;
                int tal = 0;
                int.TryParse(dialogID, out tal);
                this.roomDialog = Registry.dialogs[tal];
                isItemClicked = false;
                roomDialog.setFont(game.Content.Load<SpriteFont>(@"textfont"));
                dialogIsActive = true;
                dialogSound = game.Content.Load<SoundEffect>(@"Sound/Voice/" + dialogID);
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
                if (Registry.currentSong != song)
                {
                    Registry.music = game.Content.Load<Song>(@"Sound/BGM/" + song);
                    Registry.currentSong = song;
                    MediaPlayer.Play(Registry.music);
                }
                MediaPlayer.IsRepeating = true;
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            animbackground.Update(gameTime, clientBounds);
            doorUpdate();
            foreach (NPC item in npcs)
            {
                item.Update(gameTime, clientBounds);
                if (item.GiveItem)
                {
                    Item itemToBeAdded = new Item();
                    itemToBeAdded.loadNewItem(Registry.items[item.ItemID]);
                    itemToBeAdded.Initialize(game.Content.Load<Texture2D>(@itemToBeAdded.TextureString));
                    Console.WriteLine(item.position.Y.ToString() + " + " + item.getFrameSize.Y.ToString() + " = " + (item.position.Y + item.getFrameSize.Y).ToString());
                    if ((item.position.Y + item.getFrameSize.Y) >= 170)
                    {
                        itemToBeAdded.setPosition(new Vector2(item.position.X + item.getFrameSize.X + 20, item.position.Y + item.getFrameSize.Y / 2));
                    }
                    else
                    {
                        itemToBeAdded.setPosition(new Vector2(item.position.X, item.position.Y + item.getFrameSize.Y + 5));
                    }
                    items.Add(itemToBeAdded);
                    item.resetItem();
                }
                if (dialogIsActive || item.getIsTalking)
                {
                    Registry.pause = true;
                }
                else
                {
                    Registry.pause = false;
                }
            }
            if (roomDialog.getActiveLine() == "0")
            {
                roomDialog.resetDialog();
                dialogIsActive = false;
            }
            if (dialogIsActive)
            {
                Registry.pause = true;
            }
            else
            {
                Registry.pause = false;
            }
            roomDialog.checkLines();

            mousecontrolUpdate();
            if (save)
            {
                Registry.save();
                save = false;
            }
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
        public bool giveDoorItem(Item itemClickedOn)
        {
            bool temp = false;
            foreach (Door door in doors)
            {
                if (door.isLocked && Mousecontrol.clickedOnItem(door.getSprite().Position, door.getSprite().FrameSize, true) && (itemClickedOn.name == door.getKey()))
                {
                    door.Unlock();
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
            animbackground.Draw(gameTime, spriteBatch, 1f, 0.3f);
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
                bool clickedOnDoor = false;
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
                if (!clickedOnItem)
                {
                    foreach (Door door in doors)
                    {
                        if (Mousecontrol.rightClickedOnItem(door) && Mouse.GetState().RightButton == ButtonState.Pressed)
                            clickedOnDoor = true;
                    }
                    if (clickedOnDoor)
                    {
                        textToDraw = Mousecontrol.getDescription();
                        drawText = true;
                    }
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
            if (dialogIsActive)
            {
                if (soundhasplayed == false)
                {
                    dialogSound.Play();
                    soundhasplayed = true;
                }
                roomDialog.Speak(gameTime, spriteBatch, Vector2.Zero);
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

        private void mousecontrolUpdate()
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

        private void doorUpdate()
        {
            bool toChangeRoom = false;
            int nextRoomId = 0;
            Vector2 nextRoomDoorPosition = Vector2.Zero;
            foreach (Door item in doors)
            {
                if (Mousecontrol.inProximityToItem(item.position, new Point(item.getSprite().FrameSize.X + 10, item.getSprite().FrameSize.Y + 10)) && Mousecontrol.clickedOnItem(item.getSprite().Position,
                    item.getSprite().FrameSize, Mousecontrol.clicked()) && !Registry.inventoryInUse)
                {
                    if (!item.isLocked)
                    {
                        toChangeRoom = true;
                        nextRoomId = int.Parse(item.nextRoomID);
                        nextRoomDoorPosition = item.door2Position; 
                    }
                }
            }
            if (toChangeRoom)
            {
                changeRoom(nextRoomId, nextRoomDoorPosition);
            }
        }
        public void changeRoom(int nextRoomId, Vector2 nextRoomDoorPosition)
        {
            Registry.nextRoomDoorPosition = nextRoomDoorPosition;
            Registry.currentRoom = Registry.rooms[nextRoomId];
            Registry.currentRoom.LoadContent(game);
            Registry.changingRoom = true;

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
        public List<Door> getDoors()
        {
            return doors;
        }

    }
}
