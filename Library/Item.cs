using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Library
{
    class Item
    {
        public string name;
        public bool isPickable;
        public string description;


        public void pick()
        {
            if (isPickable)
            {
                //Lägg till föremål
            }
        }
    }
}
