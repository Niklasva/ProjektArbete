using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjektArbete
{
    class Dialog
    {
        private SpriteFont font;
        Vector2 npcPosition;
        Vector2 playerPosition;
        String dialogID;
        List<string> lines = new List<string>();
        //Konstruktor
        public Dialog(Vector2 playerPosition, Vector2 npcPosition, String dialogID, SpriteFont font)
        {
            this.npcPosition = npcPosition;
            this.dialogID = dialogID;
            this.playerPosition = playerPosition;
            this.font = font;

        }
        public void LoadDialog()
        {
            using (StreamReader reader = new StreamReader(@"../../../../ProjektArbeteContent/Data/dialog.fil"))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith(dialogID))
                    {
                        line.Replace(dialogID, "");
                        lines.Add(line);
                        line = reader.ReadLine();
                    }
                }
                reader.Close();
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, 
                lines[0]
                , new Vector2(5, 5), Color.Red);
        }
    }
}
