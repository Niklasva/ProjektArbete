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
        public Texture2D texture;
        public Sprite sprite;
        public bool isCombinable;
        //Nummret i arrayen som föremålet som kombineras till har.
        public int combinedItemInt;
        


      
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }

    }
}
