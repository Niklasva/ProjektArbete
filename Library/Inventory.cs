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
        private Item[] items;
        private int inventoryOffset = 30;
        private Texture2D background;
        private Sprite backgroundSprite;
        private bool iPressed;
        private int wait = 0;

        //Konstruktor
        public Inventory(Item[] items, Texture2D background, Rectangle clientBounds)
        {
            this.items = items;
            this.background = background;
            backgroundSprite = new Sprite(background, new Vector2(0, clientBounds.Height / 6), 0, new Point(0, 0));
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
           
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
                foreach (Item item in inventory)
                {
                    item.Draw(gameTime, spriteBatch);
                }
                backgroundSprite.Draw(gameTime, spriteBatch);
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
                    addItem(items[item1.combinedItemInt]);
                    removeItem(item1);
                    removeItem(item2);
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
                item.setPosition(new Vector2(120, inventoryOffset));
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
                inventory[i].setPosition(new Vector2(120, inventoryOffset * (i + 1)));
            }
        }
    }
}
