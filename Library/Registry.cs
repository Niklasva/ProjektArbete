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
        public enum WhichClothes { vanliga, militar, kvinna, spion, jkea };
        public static WhichClothes playersClothes = WhichClothes.vanliga;

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
            //Skriver föremålens namn
            foreach (Item item in itemsInInventory)
            {
                utfil.Write(item.name + ".");
            }
            utfil.WriteLine();
            //Skriver de rummen man tidigare varit i
            List<Room> roomsVisited = new List<Room>();
            int roomInt = 0;
            foreach (Room item in rooms)
            {
                if (item.getVisited())
                {
                    utfil.Write(roomInt + ".");
                    roomsVisited.Add(item);
                }
                roomInt++;
            }
            utfil.WriteLine();  
            //Skriver de föremål som är kvar i dessa rum 
            foreach (Room room in roomsVisited)
            {
                foreach (Item item in room.getItems())
                {
                    utfil.Write(item + "," + item.getPosition().X.ToString() + ":" + item.getPosition().Y + "!");
                }
                utfil.Write(".");
            }

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

            //Ladda de rum som man tidigare besökt
            string[] roomsPreviouslyVisitedInt = data[2].Split('.');
            string[] itemsLeftInEachRoom = data[3].Split('.');

            int numberOfRoom = 0;
            foreach (string stringInt in roomsPreviouslyVisitedInt)
            {
                if (stringInt != "")
                {
                    int temp;
                    int.TryParse(stringInt, out temp);
                    rooms[temp].LoadContent(game);

                    while (0 < rooms[temp].getItems().Count)
                    {
                        rooms[temp].removeItem(rooms[temp].getItems()[0]);
                    }

                    string[] eachItemLeftInEachRoom = itemsLeftInEachRoom[numberOfRoom].Split('!');
                    numberOfRoom++;
                    foreach (string stringItem in eachItemLeftInEachRoom)
                    {
                        if (stringItem != "")
                        {
                            string[] eachItem = stringItem.Split(',');

                            Item newItem = new Item();
                            bool tempPickable;
                            bool tempCombinable;
                            int tempCombine;
                            if (bool.TryParse(eachItem[1], out tempPickable) && bool.TryParse(eachItem[4], out tempCombinable) && int.TryParse(eachItem[5], out tempCombine))
                            {
                                newItem.loadNewItem(eachItem[0], tempPickable, eachItem[2], eachItem[3], tempCombinable, tempCombine);
                                string[] positionOfItem = eachItem[6].Split(':');
                                float tempX;
                                float tempY;
                                if (float.TryParse(positionOfItem[0], out tempX) && float.TryParse(positionOfItem[1], out tempY))
                                {
                                    newItem.setPosition(new Vector2(tempX, tempY));
                                    rooms[temp].addItem(newItem);
                                }
                            }
                        }
                    }
                }
            }

            //Laddar nuvarande rum
            currentRoom = rooms[int.Parse(data[0])];
        }
    }
}
