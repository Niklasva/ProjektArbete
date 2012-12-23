using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library
{
    public class Room
    {
        /// <summary>
        /// Rum
        /// </summary>
        /// 
        public string backgroundID;
        public string foregroundID;
        /*
         * List<Door> doors;
        List<Object> objects;
        List<NPC> npcs;
        List<Item> items;
         */
        Texture2D background;
        Texture2D foreground;

        public void LoadContent(Game game)
        {
            this.background = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
        }
    }
}
