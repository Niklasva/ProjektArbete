using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjektArbete
{
    class Dialog
    {
        NPC npc;
        String dialogID;
        Player player;
        List<string> lines = new List<string>();
        //Konstruktor
        public Dialog(Player player, NPC npc, String dialogID)
        {
            this.npc = npc;
            this.dialogID = dialogID;
            this.player = player;
        }
        public override void LoadContent()
        {
            using (StreamReader reader = new StreamReader(@"dialog.txt"))
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
    }
}
