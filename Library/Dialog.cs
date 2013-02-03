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
        /// <summary>
        /// Sköter all dialog i spelet
        /// </summary>
        public List<Line> lines;
        SpriteFont font;
        float timer = 2;
        int i = 0;
        private String activeLine = "";
        private String speaker = "";

        public void setFont(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        public String getActiveLine()
        {
            return activeLine;
        }

        public String getSpeaker()
        {
            return speaker;
        }

        public void Speak(GameTime gameTime, SpriteBatch spriteBatch, Vector2 npcPosition)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            timer -= elapsed;
            speaker = lines[i].speaker;
            if (timer > 0)
            {
                if (speaker == "Player")
                {
                    Draw(gameTime, spriteBatch, lines[i].line, new Vector2(Registry.playerPosition.X - ((lines[i].line.Count() * 6) / 2), Registry.playerPosition.Y - 15), lines[i].getColor());
                }
                else if (speaker == "NPC")
                {
                    Draw(gameTime, spriteBatch, lines[i].line, new Vector2(npcPosition.X - ((lines[i].line.Count() * 6) / 2), npcPosition.Y - 15), lines[i].getColor());
                }
                else
                {
                    Draw(gameTime, spriteBatch, lines[i].line, lines[i].position, lines[i].getColor());
                }
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
                activeLine = lines[i].line;
                
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, String output, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, output, position, color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.001f);
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
                    return Color.AliceBlue;
                }

                else if (speaker == "NPC")
                {
                    return Color.PaleVioletRed;
                }

                else if (speaker == "Narrator")
                {
                    //smoke
                    return Color.Wheat;
                }

                else
                {
                    return Color.Brown;
                }
            }
        }

        public Color getColor()
        {
            return color;
        }
    }
}
