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
        //Texturer och animation av spelaren
        public Vector2 position = new Vector2(10, 10);
        private Texture2D leftTexture;
        private Texture2D rightTexture;
        private Texture2D downTexture;
        private Texture2D upTexture;
        private Texture2D stillTexture;
        private AnimatedSprite leftSprite;
        private AnimatedSprite rightSprite;
        private AnimatedSprite downSprite;
        private AnimatedSprite upSprite;
        private AnimatedSprite currentSprite;
        private AnimatedSprite stillSprite;
        private Point leftCurrentFrame = new Point(0, 0);
        private Point rightCurrentFrame = new Point(0, 0);
        private Point downCurrentFrame = new Point(0, 0);
        private Point upCurrentFrame = new Point(0, 0);
        private double deltaX;
        private double deltaY;
        private bool isMoving;
        //Inventory
        Inventory inventory;
        
        //kontroller
        private MouseState mouseState;
        private Vector2 mousePosition;
        private Vector2 target = new Vector2(200,150);
        private Vector2 direction;
        private int speed = 2;

        //Konstruktor
        public Player(Texture2D leftTexture, Texture2D rightTexture, Texture2D downTexture, Texture2D upTexture, Texture2D stillTexture, Item[] items, Texture2D invBackGround, Rectangle clientBounds)
        {
            this.leftTexture = leftTexture;
            this.leftSprite = new AnimatedSprite(leftTexture, position, 10, new Point(20, 40), leftCurrentFrame, new Point(2, 1), 100);
            this.rightTexture = rightTexture;
            this.rightSprite = new AnimatedSprite(rightTexture, position, 10, new Point(20, 40), rightCurrentFrame, new Point(2, 1), 100);
            this.downTexture = downTexture;
            this.downSprite = new AnimatedSprite(downTexture, position, 10, new Point(20, 40), downCurrentFrame, new Point(2, 1), 100);
            this.upTexture = upTexture;
            this.upSprite = new AnimatedSprite(upTexture, position, 10, new Point(20, 40), upCurrentFrame, new Point(2, 1), 100);

            this.stillSprite = new AnimatedSprite(stillTexture, position, 0, new Point(20, 40), new Point(0, 0), new Point(1, 1), 100);
            this.inventory = new Inventory(items, invBackGround, clientBounds);
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //Rör spelaren på sig?
            isMoving = true;
            //styrning av spelare
            mouseState = Mouse.GetState();
            
            //mousePosition = mouseState/3 för att mouseState inte har något med upplösningen (som tredubblas) att göra
            mousePosition.X = (mouseState.X / 3);
            mousePosition.Y = (mouseState.Y / 3);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                target.X = mousePosition.X - 10;
                target.Y = mousePosition.Y - 40;
            }
            direction = target - position;
            direction.Normalize();
            if (position.X != target.X && position.Y != target.Y)
            {
                position.X += direction.X * speed;
                position.Y += direction.Y * speed;
            }
            if (position.X < target.X + 1 && position.X > target.X - 1)
            {
                speed = 0;
                //Spelaren rör inte på sig
                isMoving = false;
            }
            else speed = 2;

            //Rör spelaren på sig ska den animeras som vanilgt
            if (isMoving)
            {
                //Skillnad i X-led och skillnad i Y-led
                if (position.Y <= target.Y)
                    deltaY = target.Y - position.Y;
                else
                    deltaY = position.Y - target.Y;

                if (position.X < target.X)
                    deltaX = target.X - position.X;
                else
                    deltaX = position.X - target.X;
                //Bestämmning av vilken animation som ska användas av spelaren
                //Är man påväg åt höger, vänster, up eller ner?
                //Går man mest vertikalt eller horisontellt?
                if (position.X <= target.X && deltaX > deltaY)
                {
                    currentSprite = rightSprite;
                    leftCurrentFrame = new Point(0, 0);
                    downCurrentFrame = new Point(0, 0);
                    upCurrentFrame = new Point(0, 0);
                }
                else if (position.X >= target.X && deltaX > deltaY)
                {
                    currentSprite = leftSprite;
                    rightCurrentFrame = new Point(0, 0);
                    downCurrentFrame = new Point(0, 0);
                    upCurrentFrame = new Point(0, 0);
                }
                else if (position.Y >= target.Y && deltaX < deltaY)
                {
                    currentSprite = upSprite;
                    leftCurrentFrame = new Point(0, 0);
                    downCurrentFrame = new Point(0, 0);
                    rightCurrentFrame = new Point(0, 0);
                }
                else
                {
                    currentSprite = downSprite;
                    leftCurrentFrame = new Point(0, 0);
                    rightCurrentFrame = new Point(0, 0);
                    upCurrentFrame = new Point(0, 0);
                }

                currentSprite.Position = position;
                currentSprite.Update(gameTime, clientBounds);
            }
            //Rör spelaren inte på sig så ska han ha en stillastående sprite
            else
            {
                currentSprite = stillSprite;
                leftCurrentFrame = new Point(0, 0);
                rightCurrentFrame = new Point(0, 0);
                downCurrentFrame = new Point(0, 0);
                upCurrentFrame = new Point(0, 0);
            }
            currentSprite.Position = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            inventory.Draw(gameTime, spriteBatch);
            currentSprite.Draw(gameTime, spriteBatch);
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
