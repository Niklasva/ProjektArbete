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
        public List<String> npc;
        private List<Door> doors;
        private List<Object> objects;
        private List<NPC> npcs;
        private List<Item> items;
        private NPC npc1;

        Texture2D background;
        Texture2D foreground;

        public void LoadContent(Game game)
        {
            this.background = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID);
            npc1 = game.Content.Load<NPC>(@"Data/NPC/" + npc[0]);
            npc1.loadContent(game);
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            npc1.Update(gameTime, clientBounds);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            npc1.Draw(gameTime, spriteBatch, playerPosition);
        }
    }
}
