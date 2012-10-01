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

namespace LevelEditor
{
    class Sprite
    {
        private Texture2D texture;
        private Vector2 position;

        //Konstruktor
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        //Ritar ut sprite
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
        }

        //get och set
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

        //Get och set
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
    }
}
