using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library
{
    /// <summary>
    /// Ett föremål i spelet
    /// Ex. En sten
    /// </summary>
    public class Item
    {
        public string name;
        public bool isPickable;
        public string description;
        public string textureString;
        public int frameSizeX;
        public int frameSizeY;
        public bool isCombinable;
        //Nummret i arrayen som föremålet som kombineras till har.
        public int combinedItemInt;

        private Texture2D texture;
        private Sprite sprite;
        private Vector2 position;

        public string TextureString
        {
            get
            {
                return textureString;
            }
        }

        public void Initialize(Texture2D textureForItem, Vector2 position)
        {
            this.position = position;
            texture = textureForItem;
            sprite = new Sprite(texture, position, 10, new Point(frameSizeX, frameSizeY));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Position = position;
            sprite.Draw(gameTime, spriteBatch);
        }

        public void setPosition(Vector2 x)
        {
            position = x;
        }

        public Sprite getSprite()
        {
            return sprite;
        }
    }
}
