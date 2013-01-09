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
        private List<Door> doors = new List<Door>();
        private List<Object> objects = new List<Object>();
        private List<NPC> npcs = new List<NPC>();
        private List<Item> items = new List<Item>();

        private Texture2D background;
        private Texture2D foreground;
        
        //Muskontroll
        private Mousecontrol mousecontrol = new Mousecontrol();
        //Bool för att lägga till föremål i spelaren och ta bort från rummet
        private bool isItemClicked;
        //Och föremålet i fråga
        private Item itemClicked = new Item();

        public void LoadContent(Game game)
        {

            this.background = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID);
            isItemClicked = false;
            //LÄGG TILL VILKA FÖREMÅL SOM SKA VISAS HÄR
            items.Add(Registry.items[1]);
            items.Add(Registry.items[0]);

            foreach (string id in npcID)
            {
                npcs.Add(Registry.npcs[int.Parse(id)]);
            }
            foreach (NPC item in npcs)
            {
                item.loadContent(game);
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
            mousecontrol.update();

            //Om man klickar ner musen
            //Om man klickar på ett föremål i rummet
            if (mousecontrol.clickedOnItem(items, mousecontrol.clicked()))
            {
                //Kan man plocka upp föremålet?
                if (mousecontrol.getClickedItem().isPickable)
                {
                    //Ändra en bool som anger om ett föremål har blivit klickat på
                    isItemClicked = true;
                    //Föremålet som klickades på tilldelas till en variabel i rummet
                    itemClicked = mousecontrol.getClickedItem();
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

        
    }
}
