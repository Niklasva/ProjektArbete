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
        public static bool changingRoom = false;
        public static Vector2 nextRoomDoorPosition;
        public static Song music;
        public static bool musbol = false;
        public static bool pause = false;
        public enum WhichClothes { vanliga, militar, kvinna, babyjerry, jkea, vanligaKort, finkladd, fall };
        public static WhichClothes playersClothes = WhichClothes.jkea;
        public static string currentSong = "";
        public static List<Item> itemsInInventory = new List<Item>();


        public static void save()
        {
            StreamWriter utfil = new StreamWriter(@"save.txt");

            //Skriver det rummet man är i
            int currentRoomInt = 0;
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] == currentRoom)
                {
                    currentRoomInt = i;
                }
            }
            utfil.WriteLine(currentRoomInt);
            //Skriver namnen på föremålen i inventoryn
            foreach (Item item in itemsInInventory)
            {
                utfil.Write(item.name + ".");
            }
            utfil.WriteLine();           
            //Skriver strängen som anger vilka kläder som spelaren har.
            utfil.WriteLine(playersClothes);
            //Skriver spelarens position
            utfil.Write(playerPosition.X + ":" + playerPosition.Y);
            utfil.Close();
        }

        public static void load(Game game)
        {
            List<string> data = new List<string>();
            StreamReader open = new StreamReader(@"save.txt");
            string input = null;
            while ((input = open.ReadLine()) != null)
            {
                data.Add(input);
            }
            open.Close();
            //Laddar föremålen i inventoryn
            string[] namesOfItems = data[1].Split('.');
            List<Item> itemsToBeAddedToInventory = new List<Item>();
            foreach (string name in namesOfItems)
            {
                foreach (Item item in items)
                {
                    if (item.name == name)
                    {
                        Item itemToBeAdded = new Item();
                        itemToBeAdded.loadNewItem(item);
                        //Kör initialize i game1
                        itemsToBeAddedToInventory.Add(item);
                    }
                }
            }
            itemsInInventory = itemsToBeAddedToInventory;
            //KÖR INTIZIALIZE EFTER LOAD I GAME

            if (data[2] == "vanliga")
                playersClothes = WhichClothes.vanliga;
            if (data[2] == "militar")
                playersClothes = WhichClothes.militar;
            if (data[2] == "kvinna")
                playersClothes = WhichClothes.kvinna;
            if (data[2] == "jkea")
                playersClothes = WhichClothes.jkea;
            if (data[2] == "finkladd")
                playersClothes = WhichClothes.finkladd;
            if (data[2] == "babyjerry")
                playersClothes = WhichClothes.babyjerry;
            string[] playerPositionString = data[3].Split(':');
            float tempPlayerX;
            float tempPlayerY;
            if(float.TryParse(playerPositionString[0], out tempPlayerX) && float.TryParse(playerPositionString[1], out tempPlayerY))
            {
                playerPosition = new Vector2(tempPlayerX, tempPlayerY);
            }

            //Laddar nuvarande rum
            currentRoom = rooms[int.Parse(data[0])];
        }
    }
}
