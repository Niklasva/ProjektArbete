using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Library
{
    public class Dialog
    {
        public Vector2 playerPosition
        {
            get
            {
                return Registry.playerPosition;
            }
        }
        public List<String> lines;

        //Vector2 npcPosition;
        
        //String dialogID;
        //List<string> lines = new List<string>();

        //public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    spriteBatch.DrawString(font, 
        //        lines[0]
        //        , new Vector2(5, 5), Color.Red);
        //}
    }
}
