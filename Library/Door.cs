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
        bool isLocked;
        string key;

        //Konstruktor
        public Door(Vector2 position, Room nextRoom, Vector2 door2Position, Boolean isLocked, String key)
        {
            this.position = position;
            this.nextRoom = nextRoom;
            this.door2Position = door2Position;
            this.isLocked = isLocked;
        }

        //Ändrar aktivt rum och förflyttar spelaren till dörren på nästa sida
        public void Teleport()
        {
            if (isLocked == false)
            {
                Registry.currentRoom = nextRoom;
                Registry.playerPosition = door2Position;
            }
            else
            {
                //TODO: Berättaren säger något om att dörren inte kan öppnas.
            }
        }

        void Unlock(Item objct)
        {
            if (objct.name == this.key)
            {
                isLocked = false;
            }
            else
            {
                //TODO: Berättaren säger något om att man inte kan använda objektet på det sättet
            }
        }
    }
}
