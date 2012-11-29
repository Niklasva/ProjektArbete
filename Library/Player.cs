using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Library;

namespace Library
{
    /// <summary>
    /// Spelar karaktären
    /// </summary>
    public class Player
    {
        public Vector2 position = new Vector2(10, 10);
        private Texture2D texture;
        private AnimatedSprite sprite;
        private List<Item> Inventory = new List<Item>();
        Item[] items;

        //kontroller
        private MouseState mouseState;
        private Vector2 target = new Vector2(200,150);
        private Vector2 direction;
        private int speed = 3;

        //Konstruktor
        public Player(Texture2D texture, Item[] items)
        {
            this.texture = texture;
            this.sprite = new AnimatedSprite(texture, position, 10, new Point(75, 75), new Point(0, 0), new Point(6, 8), 16);
            this.items = items;
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //styrning av spelare
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                target.X = mouseState.X;
                target.Y = mouseState.Y;
            }
            direction = target - position;
            direction.Normalize();
            if (position.X != target.X && position.Y != target.Y)
            {
                position.X += direction.X * speed;
                position.Y += direction.Y * speed;
            }
            sprite.Position = position;
            sprite.Update(gameTime, spriteBatch);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }

        //Lägger till föremål i inventory
        public void addItem(Item item)
        {
            if (item.isPickable)
                Inventory.Add(item);
        }

        //Tar bort föremål från inventory
        public void removeItem(Item item)
        {
            Inventory.Remove(item);
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
                    //Tar bort de föremål som man kombinerar och lägger till det nya förempålet
                    removeItem(item1);
                    removeItem(item2);
                    addItem(items[item1.combinedItemInt]);
                }
            }
        }
    }
}
