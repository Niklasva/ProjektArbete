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

        /// <summary>
        /// Ger dialogen ett typsnitt
        /// </summary>
        /// <param name="spriteFont">Dialogens typsnitt</param>
        public void setFont(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        /// <summary>
        /// Repliken som används vid tilfället
        /// </summary>
        /// <returns>Aktiv replik</returns>
        public String getActiveLine()
        {
            return activeLine;
        }

        /// <summary>
        /// Hen som yttrar aktiv replik
        /// </summary>
        /// <returns></returns>
        public String getSpeaker()
        {
            return speaker;
        }

        /// <summary>
        /// Återställer dialogen
        /// </summary>
        public void resetDialog()
        {
            i = 0;
            activeLine = "";
            speaker = "";
            timer = 2;
        }

        /// <summary>
        /// En variant av draw-funktionen som istället för att rita ut alla repliker samtidigt, ritar ut en i taget med hjälp av tidkoderna i dialogens XML-fil
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="npcPosition">positionen för objektet</param>
        public void Speak(GameTime gameTime, SpriteBatch spriteBatch, Vector2 npcPosition)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            timer -= elapsed;
            speaker = lines[i].speaker;
            if (timer > 0)
            {
                string[] temp = lines[i].line.Split(new char[] { '/' }, 2);
                if (speaker == "Player")
                {
                    if (Registry.playerPosition.X - ((lines[i].line.Count() * 6) / 1.33f) <= 0)
                    {
                        if (temp.Length != 1)
                        {
                            Draw(gameTime, spriteBatch, temp[0], new Vector2(0f, Registry.playerPosition.Y - 20), lines[i].getColor());
                            Draw(gameTime, spriteBatch, temp[1], new Vector2(0f, Registry.playerPosition.Y - 14), lines[i].getColor());
                        }
                        else
                        Draw(gameTime, spriteBatch, lines[i].line, new Vector2(0f, Registry.playerPosition.Y - 15), lines[i].getColor());
                    }
                    else
                    {
                        if (temp.Length != 1)
                        {
                            Draw(gameTime, spriteBatch, temp[0], new Vector2(Registry.playerPosition.X - ((temp[0].Count() * 6) / 1.33f), Registry.playerPosition.Y - 20), lines[i].getColor());
                            Draw(gameTime, spriteBatch, temp[1], new Vector2(Registry.playerPosition.X - ((temp[1].Count() * 6) / 1.33f), Registry.playerPosition.Y - 13), lines[i].getColor());
                        }
                        else
                            Draw(gameTime, spriteBatch, lines[i].line, new Vector2(Registry.playerPosition.X - ((lines[i].line.Count() * 6) / 2), Registry.playerPosition.Y - 15), lines[i].getColor());
                    }
                }
                else if (speaker == "NPC")
                {
                    if (npcPosition.X - ((npcPosition.X - lines[i].line.Count() * 6) / 2) <= 0)
                    {
                        Draw(gameTime, spriteBatch, lines[i].line, new Vector2(0f, npcPosition.Y - 15), lines[i].getColor());
                    } 
                    else
                    Draw(gameTime, spriteBatch, lines[i].line, new Vector2(npcPosition.X - ((lines[i].line.Count() * 6) / 2), npcPosition.Y - 15), lines[i].getColor());
           

                }
                else if (speaker == "Narrator")
                {
                    if (temp.Length != 1)
                    {
                        Draw(gameTime, spriteBatch, temp[0], new Vector2(320 / 2 - ((temp[0].Count() * 6) / 1.5f), 12), lines[i].getColor());
                        Draw(gameTime, spriteBatch, temp[1], new Vector2(320 / 2 - ((temp[1].Count() * 6) / 1.5f), 22), lines[i].getColor());
                    }
                    else
                        Draw(gameTime, spriteBatch, lines[i].line, new Vector2(320 / 2 - ((lines[i].line.Count() * 6) / 1.5f), 15), lines[i].getColor());
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

    /// <summary>
    /// Underklass för varje enskild replik
    /// Ger replikerna:
    /// • en rad text
    /// • en position (om det inte är spelaren eller objektet som talar)
    /// • namnet på subjektet
    /// • TID
    /// • färg
    /// </summary>
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
