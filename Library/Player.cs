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
        public Vector2 position = new Vector2(36, 39);
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
        private float layerPosition = 0;

        //Skala spriten
        float scale;

        //Inventory
        Inventory inventory;
        
        //Kontroller
        private MouseState mouseState;
        private Vector2 mousePosition;
        private Vector2 target = new Vector2(36, 39);
        private Vector2 direction;
        private int speed = 1;

        //Konstruktor
        public Player(Game game, Texture2D leftTexture, Texture2D rightTexture, Texture2D downTexture, Texture2D upTexture, Texture2D stillTexture, Texture2D invBackGround, Rectangle clientBounds)
        {
            this.leftTexture = leftTexture;
            this.leftSprite = new AnimatedSprite(leftTexture, position, 10, new Point(20, 40), leftCurrentFrame, new Point(2, 1), 100);
            this.rightTexture = rightTexture;
            this.rightSprite = new AnimatedSprite(rightTexture, position, 10, new Point(20, 40), rightCurrentFrame, new Point(2, 1), 100);
            this.downTexture = downTexture;
            this.downSprite = new AnimatedSprite(downTexture, position, 10, new Point(20, 40), downCurrentFrame, new Point(2, 1), 100);
            this.upTexture = upTexture;
            this.upSprite = new AnimatedSprite(upTexture, position, 10, new Point(20, 40), upCurrentFrame, new Point(2, 1), 100);
            this.stillTexture = stillTexture;
            this.stillSprite = new AnimatedSprite(stillTexture, position, 0, new Point(20, 40), new Point(0, 0), new Point(1, 1), 100);
            
            this.inventory = new Inventory(invBackGround, clientBounds, game);
            this.scale = 1f;
        }

        public void Update(Game game, GameTime gameTime, Rectangle clientBounds)
        {
            
            //scaleToPosition(clientBounds);
            //Är inventoryn öppen ska spelaren inte röra på sig
            if (!Registry.inventoryInUse)
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
                if (position.X < target.X + 1 && position.X > target.X - 1 && position.Y < target.Y + 1 && position.Y > target.Y - 1)
                {
                    speed = 0;
                    //Spelaren rör inte på sig
                    isMoving = false;
                }
                else speed = 2;
            }
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
            Registry.playerIsMoving = isMoving;

            inventory.Update();
            currentSprite.Position = position;
            Registry.playerPosition = position;
            updateLayerDepth();
            if(!Registry.inventoryInUse)
                 currentSprite.Update(gameTime, clientBounds);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            currentSprite.Draw(gameTime, spriteBatch, scale, layerPosition);
            inventory.Draw(gameTime, spriteBatch);
        }

        public void addItem(Item item)
        {
            inventory.addItem(item);
        }
        public void removeItem(Item item)
        {
            inventory.removeItem(item);
        }

        public void Stop()
        {
            target = position;
        }

        private void updateLayerDepth()
        {
            layerPosition = (1 - (position.Y + currentSprite.Texture.Height) / 180) / 3;
        }

        //private void scaleToPosition(Rectangle clientBounds)
        //{
        //    //Skalan blir 1 - skilladen mellan rutans storlek och positionen på karaktären.
        //    //Gör att när man är närmast "kameran" blir karaktären som störst och när man rör sig därifrån blir karaktären mindre.
        //    float temp = (clientBounds.Height / 3) - position.Y * 2f;
        //    double doubleScale = 1 - (temp * 0.001);
        //    scale = float.Parse(doubleScale.ToString());
        //}
    }
}
