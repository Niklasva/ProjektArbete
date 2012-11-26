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
    /// <summary>
    ///  Icke-spelarkaraktärer.
    /// </summary>
    public class NPC
    {
        public String name;
        public Vector2 position;
        public int dialogID;
        private Dialog dialog;
        private Texture2D texture;
        int collisionOffset = 10;
        Point frameSize = new Point(10, 10);
        Point currentFrame = new Point(10, 10);
        Point sheetSize = new Point(10, 10);
        int millisecondsPerFrame = 10;

        //public NPC(Texture2D texture, Vector2 position, int collisionOffset, Point frameSize, Point currentFrame, Point sheetSize,
        //    int millisecondsPerFrame)
        //    : base(texture, position, collisionOffset, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        //{
        //}

        public void setDialog(Dialog dialog)
        {
            this.dialog = dialog;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public String getDialog()
        {
           return dialog.lines[0];
        }
    }

}
