using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Library
{
    public class Room
    {
        public List<Door> doors;
        public List<Object> objects;
        Texture2D background;
        Texture2D foreground;
    }
}
