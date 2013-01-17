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
    /// En vanlig sprite i spelet
    /// </summary>
    public class Sprite
    {
        private Texture2D texture;
        private Vector2 position;
        private int collisionOffset;
        private Point frameSize;

        //Konstruktorer
        public Sprite(Texture2D texture, Vector2 position, int collisionOffset, Point frameSize)
        {
            this.texture = texture;
            this.position = position;
            this.collisionOffset = collisionOffset;
            this.frameSize = frameSize;
        }

        //Ritar ut sprite
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        //get och set för texturer
        public Texture2D Texture
        {
            set
            {
                this.texture = value;
            }
            get
            {
                return this.texture;
            }
        }

        //Get och set för position
        public Vector2 Position
        {
            set
            {
                this.position = value;
            }
            get
            {
                return this.position;
            }
        }

        public virtual Rectangle collisonRect
        {
            get
            {
                return new Rectangle((int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        public Point FrameSize
        {
            get
            {
                return frameSize;
            }
        }
    }
}
