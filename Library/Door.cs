using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Library
{
    /// <summary>
    ///  Portaler som förflyttar spelare mellan rum.
    /// </summary>
    public class Door
    {
        public Vector2 position;
        public string nextRoomID;
        public Vector2 door2Position;
        public bool isLocked;
        public string key;
        public string textureID;
        private AnimatedSprite sprite;
        private Texture2D texture;


        public void LoadContent(Game game)
        {
           
            texture = game.Content.Load<Texture2D>(@"Images/Sprites/" + textureID);
            sprite = new AnimatedSprite(texture, position, 0, new Point(texture.Width / 2, texture.Height), new Point(0, 0), new Point(1, 0), 1);
        }

        public void Unlock()
        {
            isLocked = false;
        }
        public void Lock()
        {
            isLocked = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, 1f, 0.3332f);
        }

        public Sprite getSprite()
        {
            return sprite;
        }
        public string getKey()
        {
            return key;
        }
    }
}
