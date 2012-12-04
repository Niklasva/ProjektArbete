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
        Inventory inventory;
        

        //kontroller
        private MouseState mouseState;
        private Vector2 mousePosition;
        private Vector2 target = new Vector2(200,150);
        private Vector2 direction;
        private int speed = 3;

        //Konstruktor
        public Player(Texture2D texture, Item[] items)
        {
            this.texture = texture;
            this.sprite = new AnimatedSprite(texture, position, 10, new Point(75, 75), new Point(0, 0), new Point(6, 8), 16);
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //styrning av spelare
            mouseState = Mouse.GetState();

            //mousePosition = mouseState/3 för att mouseState inte har något med upplösningen (som tredubblas) att göra
            mousePosition.X = (mouseState.X / 3);
            mousePosition.Y = (mouseState.Y / 3);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                target.X = mousePosition.X - 32.5f;
                target.Y = mousePosition.Y - 32.5f;
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

        public void addItem(Item item)
        {
            inventory.addItem(item);
        }
        public void removeItem(Item item)
        {
            inventory.removeItem(item);
        }
        public void combineItem(Item item1, Item item2)
        {
            inventory.combineItem(item1, item2);
        }


    }
}
