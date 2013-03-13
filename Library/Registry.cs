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
            utfil.WriteLine();
            //Skriver huruvida dörrarna i rummet är låsta eller inte
            foreach (Room room in roomsVisited)
            {
                foreach (Door door in room.getDoors())
                {
                    utfil.Write(door.isLocked + "!");
                }
                utfil.Write(".");
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

            //Ladda de rum som man tidigare besökt
            string[] roomsPreviouslyVisitedInt = data[2].Split('.');
            string[] itemsLeftInEachRoom = data[3].Split('.');
            string[] doorsInEachRoom = data[4].Split('.');

            int numberOfRoom = 0;
            foreach (string stringInt in roomsPreviouslyVisitedInt)
            {
                if (stringInt != "")
                {
                    int temp;
                    int.TryParse(stringInt, out temp);
                    rooms[temp].LoadContent(game);
                    //Tar bort alla föremål från rummet
                    while (0 < rooms[temp].getItems().Count)
                    {
                        rooms[temp].removeItem(rooms[temp].getItems()[0]);
                    }

                    string[] eachItemLeftInEachRoom = itemsLeftInEachRoom[numberOfRoom].Split('!');

                    //Lägger tillbaka föremålen som finns sparade i rummet
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
                    string[] eachDoorInEachRoom = doorsInEachRoom[numberOfRoom].Split('!');
                    int numberOfDoor = 0;
                    foreach (string doorString in eachDoorInEachRoom)
                    {
                        if (doorString != "")
                        {
                            bool tempBool;
                            if (bool.TryParse(doorString, out tempBool))
                            {
                                if (tempBool)
                                {
                                    rooms[temp].doors[numberOfDoor].Lock();
                                }
                                else
                                {
                                    rooms[temp].doors[numberOfDoor].Unlock();
                                }
                            }
                            numberOfDoor++;
                        }
                    }
                    numberOfRoom++;
                }
            }
            if (data[5] == "vanliga")
                playersClothes = WhichClothes.vanliga;
            if (data[5] == "militar")
                playersClothes = WhichClothes.militar;
            if (data[5] == "kvinna")
                playersClothes = WhichClothes.kvinna;
            if (data[5] == "jkea")
                playersClothes = WhichClothes.jkea;
            if (data[5] == "spion")
                playersClothes = WhichClothes.spion;
            string[] playerPositionString = data[6].Split(':');
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
