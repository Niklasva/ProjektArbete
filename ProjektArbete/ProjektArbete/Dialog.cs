using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektArbete
{
    class Dialog
    {
        Vector2 npcPosition;
        Vector2 playerPosition;
        String dialogID;
        List<string> lines = new List<string>();
        //Konstruktor
        public Dialog(Vector2 playerPosition, Vector2 npcPosition, String dialogID)
        {
            this.npcPosition = npcPosition;
            this.dialogID = dialogID;
            this.playerPosition = playerPosition;
        }
        public void LoadContent()
        {
            using (StreamReader reader = new StreamReader(@"Data/dialog"))
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
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
