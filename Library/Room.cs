using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Library
{
    public class Room
    {
        /// <summary>
        /// Rum
        /// </summary>
        public List<Door> doors;
        public List<Object> objects;
        public List<NPC> npcs;
        Texture2D background;
        Texture2D foreground;
    }
}
