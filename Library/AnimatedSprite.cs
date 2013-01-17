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

namespace Library
{
    /// <summary>
    /// Den sprite som animeras
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        private Point frameSize;
        private Point currentFrame;
        private Point sheetSize;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame;

        //Konstruktor
        public AnimatedSprite(Texture2D texture, Vector2 position, int collisionOffset, Point frameSize, Point currentFrame, Point sheetSize,
                         int millisecondsPerFrame)
            : base(texture, position, collisionOffset, frameSize)
        {
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        //Ritar ut den animerade figuren
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
        {
            spriteBatch.Draw(Texture,
                Position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X,
                    frameSize.Y),
                Color.White,
                0,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0);
        }

        //Updaterar figuren
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }

            }
        }
    }
}
