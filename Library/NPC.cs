using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Library
{
    public class NPC
    {
        public String name;
        public Vector2 position;
        public int dialogID;
        private Dialog dialog;

        public void setDialog(Dialog dialog)
        {
            this.dialog = dialog;
        }

        public String getDialog()
        {
           return dialog.lines[0];
        }
    }

}
