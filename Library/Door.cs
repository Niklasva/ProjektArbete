using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Library
{
    /// <summary>
    ///  Portaler som förflyttar spelare mellan rum.
    /// </summary>
    public class Door
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

        //Ändrar aktivt rum och förflyttar spelaren till dörren på nästa sida
        public void Teleport()
        {
            Registry.currentRoom = nextRoom;
            Registry.playerPosition = door2Position;
        }
    }
}
