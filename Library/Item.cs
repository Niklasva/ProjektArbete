using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Library
{
    /// <summary>
    /// Ett föremål i spelet
    /// Ex. En sten
    /// </summary>
    public class Item
    {
        public string name;
        public bool isPickable;
        public string description;
        public bool isCombinable;
        //Nummret i arrayen som föremålet som kombineras till har.
        public int combinedItemInt;
        


        public void pick()
        {
            if (isPickable)
            {
                //Lägg till föremål
            }
        }
    }
}
