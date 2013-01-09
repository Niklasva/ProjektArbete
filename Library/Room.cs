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
        public List<string> npc = new List<string>();
        private List<Door> doors = new List<Door>();
        private List<Object> objects = new List<Object>();
        private List<NPC> npcs = new List<NPC>();
        private List<Item> items = new List<Item>();
        private NPC npc1;

        private Texture2D background;
        private Texture2D foreground;
        
        //Muskontroll
        private Mousecontrol mousecontrol = new Mousecontrol();
        //Bool för att lägga till föremål i spelaren och ta bort från rummet
        private bool isItemClicked;
        //Och föremålet i fråga
        private Item itemClicked = new Item();

        public void LoadContent(Game game, List<Item> items)
        {
            this.background = game.Content.Load<Texture2D>(@"Images/Backgrounds/" + backgroundID);
            npc1 = game.Content.Load<NPC>(@"Data/NPC/" + npc[0]);
            npc1.loadContent(game);
            this.items = items;
            isItemClicked = false;
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            npc1.Update(gameTime, clientBounds);
            mousecontrolUpdate();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            npc1.Draw(gameTime, spriteBatch, playerPosition);
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
