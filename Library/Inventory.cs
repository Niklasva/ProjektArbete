﻿using System;
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
        private bool iPressed;
        private int wait = 0;

        //Muskontroll
        private Item itemClickedOn = new Item();
        private bool isInteractingWithItem = false;


        //Konstruktor
        public Inventory(Texture2D background, Rectangle clientBounds)
        {
            this.background = background;
            this.inventoryPosition = clientBounds.Height / 6;
            backgroundSprite = new Sprite(background, new Vector2(0, inventoryPosition - 24), 0, new Point(0, 0));
            
        }


        public void Update()
        {
            //Uppdatering av muskontroll
           // Mousecontrol.update();
            if (iPressed)
            {
                if (isInteractingWithItem)
                {
                    //Om man klickar med vänstra musknappen
                    if (Mousecontrol.clicked())
                    {
                        //Om föremålet går att kombinera och man klickar på ett föremål som går att kombinera
                        if (itemClickedOn.isCombinable && Mousecontrol.clickedOnItem(inventory, true) &&
                            itemClickedOn.isCombinable)
                        {
                            combineItem(itemClickedOn, Mousecontrol.getClickedItem());
                            isInteractingWithItem = false;
                        }
                        else
                        {
                            isInteractingWithItem = false;
                            addItem(itemClickedOn);
                            sortInventory();
                        }
                    }
                }
                else
                {
                    //Om man klickar ner musen
                    if (Mousecontrol.clicked())
                    {
                        //Om man klickar på ett föremål i sin inventory
                        if (Mousecontrol.clickedOnItem(inventory, true))
                        {
                            itemClickedOn = Mousecontrol.getClickedItem();
                            isInteractingWithItem = true;
                            removeItem(Mousecontrol.getClickedItem());
                        }
                    }
                }

                if (isInteractingWithItem)
                    itemClickedOn.setPosition(new Vector2(Mouse.GetState().X / 3 - itemClickedOn.frameSizeX / 2,
                        Mouse.GetState().Y / 3 - itemClickedOn.frameSizeY / 2));
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            wait++;
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                if (!iPressed && wait > 5)
                    iPressed = true;
                else if (wait > 5)
                    iPressed = false;
                wait = 0;
            }

            if (iPressed)
            {
                backgroundSprite.Draw(gameTime, spriteBatch);

                foreach (Item item in inventory)
                {
                    item.Draw(gameTime, spriteBatch);
                }

                if (isInteractingWithItem)
                    itemClickedOn.Draw(gameTime, spriteBatch);
            }
         
        }

        //Kombinerar föremål i inventoryn
        public void combineItem(Item item1, Item item2)
        {
            //Kan båda kombineras?
            if (item1.isCombinable && item2.isCombinable)
            {
                //Har de samma komineringsnummer?
                if (item1.combinedItemInt == item2.combinedItemInt)
                {
                    //Tar bort de föremål som man kombinerar och lägger till det nya föremålet
                    addItem(Registry.items[item1.combinedItemInt]);
                    removeItem(item1);
                    removeItem(item2);
                    sortInventory();
                }
            }
            else
            {
                //TODO: berättaren säger något om att man inte kan använda föremålet på det sättet
            }
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

        public void sortInventory()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (i != 0)
                    inventory[i].setPosition(new Vector2(inventory[i - 1].frameSizeX * i, inventoryPosition - inventory[i].frameSizeY / 3));
                else
                    inventory[i].setPosition(new Vector2(inventory[i].frameSizeX * i, inventoryPosition - inventory[i].frameSizeY / 3));
            }
        }

        public bool IPressed
        {
            get
            {
                return iPressed;
            }
        }
    }
}
