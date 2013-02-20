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
        public bool isCombinable;
        //Nummret i arrayen som föremålet som kombineras till har.
        public int combinedItemInt;

        private Texture2D texture;
        private Sprite sprite;
        private Vector2 position;
        private float layerPosition = 0;
        private bool isActive = true;


        public string TextureString
        {
            get
            {
                return textureString;
            }
        }

        public void Initialize(Texture2D textureForItem)
        {
            texture = textureForItem;
            sprite = new Sprite(texture, position, 10, new Point(texture.Width, texture.Height));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Position = position;
            sprite.Draw(gameTime, spriteBatch, 1f, layerPosition);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
            sprite.Position = position;
            sprite.Draw(gameTime, spriteBatch, 1f, layerDepth);
        }

        public void setPosition(Vector2 x)
        {
            position = x;
        }
        public Vector2 getPosition()
        {
            return position;
        }

        public Sprite getSprite()
        {
            return sprite;
        }

        public void setLayerPosition(float x)
        {
            layerPosition = x;
        }

        public void loadNewItem(Item item)
        {
            name = item.name;
            isPickable = item.isPickable;
            description = item.description;
            textureString = item.textureString;
            isCombinable = item.isCombinable;
            combinedItemInt = item.combinedItemInt;
        }
        public void loadNewItem(string name, string isPickable, string description, string textureString, string isCombinable, string combinedItemInt)
        {
            this.name = name;
            this.isPickable = bool.Parse(isPickable);
            this.description = description;
            this.textureString = textureString;
            this.isCombinable = bool.Parse(isCombinable);
            this.combinedItemInt = int.Parse(combinedItemInt);
        }

        public void setInactive(bool x)
        {
            isActive = x;
        }
        public bool getActive()
        {
            return isActive;
        }

        public override string ToString()
        {
            return name + "," + isPickable + "," + description + "," + textureString + "," + isCombinable + "," + combinedItemInt;
        }
    }
}
