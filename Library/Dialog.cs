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
        public List<Line> lines;
    }

    public class Line
    {
        public string speaker;
        public Vector2 position;
        public String line;
        private Color color
        {
            get
            {
                if (speaker == "Player")
                {
                    return Color.Red;
                }

                else if (speaker == "NPC")
                {
                    return Color.Blue;
                }

                else if (speaker == "Narrator")
                {
                    //smoke
                    return Color.Wheat;
                }

                else
                {
                    return Color.Black;
                }
            }
        }

        public Color getColor()
        {
            return color;
        }
    }
}
