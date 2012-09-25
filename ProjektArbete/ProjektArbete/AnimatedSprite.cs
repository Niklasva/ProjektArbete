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

namespace ProjektArbete
{
    class AnimatedSprite : Sprite
    {
        private Point frameSize { get; set; }
        private Point currentFrame { get; set; }
        private Point sheetSize { get; set; }
        private int timeSinceLastFrame { get; set; }
        private int millisecondsPerFrame { get; set; }
        private int collisionRectOffset { get; set; }

        //Konstruktor
        public AnimatedSprite(Texture2D texture, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize,
                         int timeSinceLastFame, int millisecondsPerFrame, int collisionRectOffset)
            : base(texture, position)
        {
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.timeSinceLastFrame = timeSinceLastFame;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.collisionRectOffset = collisionRectOffset;
        }

        //Ritar ut den animerade figuren
        public override void Draw(SpriteBatch spriteBatch)
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
                1,
                SpriteEffects.None,
                0);
        }
    }
}
