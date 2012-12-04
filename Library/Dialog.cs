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
        SpriteFont font;
        float timer = 2;
        int i = 0;

        public void setFont(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        public void Speak(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer > 0)
            {
                Draw(gameTime, spriteBatch, lines[i].line, lines[i].position, lines[i].getColor());
            }
            if (timer <= 0)
            {
                i++;
                if (i < lines.Count)
                {
                    timer = lines[i].time;
                }
                else
                    timer = -10;
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, String output, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, output, position, color);
        }
    }

    public class Line
    {
        public string speaker;
        public Vector2 position;
        public String line;
        public int time;
        private Color color
        {
            get
            {
                if (speaker == "Player")
                {
                    return Color.NavajoWhite;
                }

                else if (speaker == "NPC")
                {
                    return Color.LightBlue;
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
