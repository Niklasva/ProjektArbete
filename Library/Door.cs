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
        public Vector2 position;
        public string nextRoomID;
        public Vector2 door2Position;
        public bool isLocked;
        public string key;
        private Dialog cantOpenDialog;
        private Room nextRoom;

        public void LoadContent(Game game)
        {
            nextRoom = Registry.rooms[int.Parse(nextRoomID)];
            nextRoom.LoadContent(game);
            cantOpenDialog = Registry.dialogs[1];
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
