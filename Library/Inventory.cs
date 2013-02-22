using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library
{
    /// <summary>
    /// Tar hand om all "inventory managment"
    /// </summary>
    class Inventory
    {
        private List<Item> inventory = new List<Item>();
        private int inventoryPosition;
        private Texture2D background;
        private Sprite backgroundSprite;
        private int wait = 0;
        private bool inventoryInUse;

        //Muskontroll
        private Item itemClickedOn;
        private bool isInteractingWithItem = false;
        public Item getItemClickedon { get { return itemClickedOn; } }

        //game
        Game game;

        //Konstruktor
        public Inventory(Texture2D background, Rectangle clientBounds, Game game)
        {
            this.background = background;
            this.inventoryPosition = clientBounds.Height / 6;
            backgroundSprite = new Sprite(background, new Vector2(0, inventoryPosition - (background.Height / 2)), 0, new Point(background.Width, background.Height));
            this.game = game;
            this.inventoryInUse = false;
        }


        public void Update()
        {
            if (isInteractingWithItem && !inventoryInUse)
            {
                if (Mousecontrol.clicked())
                {
                    isInteractingWithItem = false;
                }
            }
            if (isInteractingWithItem)
            {
                itemClickedOn.setPosition(new Vector2(Mouse.GetState().X / 3 - itemClickedOn.getSprite().Texture.Width / 2,
                    Mouse.GetState().Y / 3 - itemClickedOn.getSprite().Texture.Height / 2));
            }

            //Uppdatering av muskontroll
            if (inventoryInUse)
            {
                if (isInteractingWithItem)
                {
                    //Om man klickar med vänstra musknappen...
                    if (Mousecontrol.clicked())
                    {
                        bool clickedOnItem = false;
                        Item tempItem = new Item();
                        foreach (Item item in inventory)
                        {
                            //På ett föremål...
                            if (Mousecontrol.clickedOnItem(item.getSprite().Position, item.getSprite().FrameSize, true))
                            {
                                clickedOnItem = true;
                                tempItem = item;
                            }
                        }
                        //...och om föremålet går att kombinera och man klickar på ett föremål som går att kombinera
                        if (itemClickedOn.isCombinable && clickedOnItem &&
                            itemClickedOn.isCombinable)
                        {
                            //...går det att kombinera föremålen så kombineras de och annars läggs det föremålet som man klickat på tillbaks i inventoryn
                            if (!combineItem(itemClickedOn, tempItem))
                                addItem(itemClickedOn);
                            isInteractingWithItem = false;
                        }
                        else
                        {
                            addItem(itemClickedOn);
                            isInteractingWithItem = false;
                        }
                    }
                }
                else
                {
                    //Om man klickar ner musen...
                    if (Mousecontrol.clicked())
                    {
                        bool clickedOnItem = false;
                        //... och om man klickar på ett föremål i sin inventory...
                        foreach (Item item in inventory)
                        {                            
                            if (Mousecontrol.clickedOnItem(item.getPosition(), item.getSprite().FrameSize, true))
                            {
                                itemClickedOn = item;
                                clickedOnItem = true;
                            }
                        }

                        if (clickedOnItem)
                        {
                            //Blir föremålet ett föremål som följer musen och den tas bort från själva inventoryn
                            isInteractingWithItem = true;
                            removeItem(itemClickedOn);
                        }

                    }
                }
            }
            Registry.itemsInInventory = inventory;
            Registry.inventoryInUse = inventoryInUse;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Klickar man ned i det har gått mer än 1 ruta sedan förra gången man gjorde det öppnas inventoryn
            wait++;
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                if (!inventoryInUse && wait > 1)
                {
                    inventoryInUse = true;

                }
                else if (wait > 1)
                {
                    inventoryInUse = false;

                }
                wait = 0;
            }

            if (isInteractingWithItem)
                itemClickedOn.Draw(gameTime, spriteBatch, 0);

            if (inventoryInUse)
            {
                backgroundSprite.Draw(gameTime, spriteBatch, 1f, 0.002f);

                foreach (Item item in inventory)
                {
                    item.Draw(gameTime, spriteBatch, 0f);
                }

                string textToDraw = null;
                bool drawText = false;
                bool clickedOnItem = false;
                //Om man håller inne musknappen över ett föremål...
                foreach (Item item in inventory)
                {
                    if (Mousecontrol.rightClickedOnItem(item))
                    {
                        clickedOnItem = true;
                    }     
                }
                if ((Mouse.GetState().RightButton == ButtonState.Pressed && clickedOnItem))
                {
                    //...blir texten som ska skrivas ut beskrivningen för föremålet
                    textToDraw = Mousecontrol.getDescription();
                    drawText = true;
                }
                //Om man håller musen över ett föremål...
                else if (clickedOnItem)
                {
                    //...blir texten som ska skrivas ut namnet på föremålet
                    textToDraw = Mousecontrol.getName();
                    drawText = true;
                }
                //Om man ska skriva ut text
                if (drawText)
                {
                    //Texten placeras längst ned i inventoryn. 
                    spriteBatch.DrawString(game.Content.Load<SpriteFont>(@"textfont"), textToDraw,
                        new Vector2(game.Window.ClientBounds.Width / 6 - textToDraw.Count() * 4f,
                            0), Color.White);
                }
            }
        }

        //Kombinerar föremål i inventoryn
        private bool combineItem(Item item1, Item item2)
        {
            bool successfullCombination = false;
            //Kan båda kombineras?
            if (item1.isCombinable && item2.isCombinable)
            {
                //Har de samma komineringsnummer?
                if (item1.combinedItemInt == item2.combinedItemInt)
                {
                    //Tar bort de föremål som man kombinerar och lägger till det nya föremålet
                    Item itemToBeAdded = new Item();
                    itemToBeAdded.loadNewItem(Registry.items[item1.combinedItemInt]);
                    itemToBeAdded.Initialize(game.Content.Load<Texture2D>(@itemToBeAdded.TextureString));
                    removeItem(item1);
                    removeItem(item2);
                    addItem(itemToBeAdded);
                    successfullCombination = true;
                }
            }
            else
            {
                //TODO: berättaren säger något om att man inte kan använda föremålet på det sättet
            }

            return successfullCombination;
        }

        //Lägger till föremål i inventory
        public void addItem(Item item)
        {
            if (item.isPickable)
            {
                inventory.Add(item);
                sortInventory();
            }
        }

        //Tar bort föremål från inventory
        public void removeItem(Item item)
        {
            inventory.Remove(item);
            sortInventory();
        }

        private void sortInventory()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                //Om föremålet ligger längre fram i listan än på första platsen så ska den hamna bakom den förra föremålets rutstorlek
                if (i != 0)
                    inventory[i].setPosition(new Vector2((5 + inventory[i - 1].getSprite().Texture.Width) * i + 3, inventoryPosition - inventory[i].getSprite().Texture.Height / 6));
                else
                    inventory[i].setPosition(new Vector2(3, inventoryPosition - inventory[i].getSprite().Texture.Height / 6));
            }
        }

        public bool InteractingWithItem
        {
            get
            {
                return isInteractingWithItem;
            }
        }

        public bool InventoryInUse
        {
            get
            {
                return inventoryInUse;
            }
        }
    }
}
