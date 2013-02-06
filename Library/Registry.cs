using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace Library
{
    /// <summary>
    ///  Det här är en klass som håller reda på saker som alla klasser ska kunna komma åt
    /// </summary>
    public static class Registry
    {
        public static Vector2 playerPosition;
        public static Room currentRoom;
        public static Item[] items;
        public static Dialog[] dialogs;
        public static NPC[] npcs;
        public static Room[] rooms;
        public static bool inventoryInUse = false;
        public static bool playerIsMoving = false;
        
        public static bool changingRoom = false;
        public static Vector2 nextRoomDoorPosition;
        public static Song music;
        public static bool musbol = false;

        private static List<Item> itemsInInventory = new List<Item>();
        //

        public static void save()
        {
            StreamWriter utfil = new StreamWriter(@"save.txt");

            utfil.Write("nuvarande rum, föremål i inventoryn, intar på de rum som man besökt och de föremål som finns kvar där");
            int currentRoomInt = 0;
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] == currentRoom)
                {
                    currentRoomInt = i;
                }
            }
            utfil.WriteLine(currentRoomInt);

            foreach (Item item in itemsInInventory)
            {
                utfil.Write(item.name + ".");
            }

            utfil.Close();

            
        }

        public static void load()
        {
            List<string> data = new List<string>();
            StreamReader open = new StreamReader(@"save.txt");
            string input = null;
            while ((input = open.ReadLine()) != null)
            {
                data.Add(input);
            }
            open.Close();

            currentRoom = rooms[int.Parse(data[0])];

            string[] namesOfItems = data[1].Split(' ');


        }
     }
}
