using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library
{
    public class Room
    {
        /// <summary>
        /// Rum
        /// </summary>
        /// 
        public string backgroundID;
        public string foregroundID;
        public string[] npcID;
        public string[] itemID;
        public string[] itemPosition;
        public List<Door> doors = new List<Door>();
        private List<NPC> npcs = new List<NPC>();
        private List<Item> items = new List<Item>();

        private Texture2D background;
        private Texture2D foreground;
        
        //Muskontroll 
        //Bool för att lägga till föremål i spelaren och ta bort från rummet
        private bool isItemClicked;
        //Och föremålet i fråga
        private Item itemClicked;

        public void LoadContent(Game game)
        {
            this.background = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID);
            isItemClicked = false;
            //LÄGG TILL VILKA FÖREMÅL SOM SKA VISAS HÄR
            foreach (string id in itemID)
            {
                items.Add(Registry.items[int.Parse(id)]);
            }
            foreach (string id in npcID)
            {
                npcs.Add(Registry.npcs[int.Parse(id)]);
            }
            foreach (NPC item in npcs)
            {
                item.loadContent(game);
            }
            
            int i = 0;
            foreach (Item item in items)
            {

                string[] temp = itemPosition[i].Split(new char[] { ',' }, 2);
                item.setPosition(new Vector2(float.Parse(temp[0]), float.Parse(temp[1])));
                i++;
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            foreach (NPC item in npcs)
            {
                item.Update(gameTime, clientBounds);
            }
            mousecontrolUpdate();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            foreach (NPC npc in npcs)
            {
                npc.Draw(gameTime, spriteBatch, playerPosition);
            }
            foreach (Item item in items)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }


        public void mousecontrolUpdate()
        {
            //Om man klickar ner musen
            //Om man klickar på ett föremål i rummet
            if (Mousecontrol.clickedOnItem(items, Mousecontrol.clicked()))
            {
                //Kan man plocka upp föremålet?
                if (Mousecontrol.getClickedItem().isPickable)
                {
                    //Ändra en bool som anger om ett föremål har blivit klickat på
                    isItemClicked = true;
                    //Föremålet som klickades på tilldelas till en variabel i rummet
                    itemClicked = Mousecontrol.getClickedItem();
                }
            }
        }
        public bool isItemClickedInRoom()
        {
            return isItemClicked;
        }
        public void itemWasClicked()
        {
            isItemClicked = false;
        }
        public Item getClickedItem()
        {
            return itemClicked;
        }
        public void removeItem()
        {
            items.Remove(itemClicked);
        }
        public List<Item> getItems()
        {
            return items;
        }

        
    }
}
