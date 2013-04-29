using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Library;

namespace ProjektArbete
{
    /// <summary>
    ///  Portaler som förflyttar spelare mellan rum.
    /// </summary>
    class Door
    {
        Vector2 position;
        Room nextRoom;
        Vector2 door2Position;

        //Konstruktor
        public Door(Vector2 position, Room nextRoom, Vector2 door2Position)
        {
            this.position = position;
            this.nextRoom = nextRoom;
            this.door2Position = door2Position;
        }

        public void Teleport()
        {
            Registry.currentRoom = nextRoom;
            Registry.playerPosition = door2Position;
        }
    }
}
