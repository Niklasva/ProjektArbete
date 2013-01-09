using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Library
{
    public static class Mousecontrol
    {
        /// <summary>
        /// Sköter allt som har med att man klickar att göra
        /// </summary>
        private static MouseState prevMouseState;
        private static  MouseState currMouseState;
        private static Item itemClickedOn;

        public static void update()
        {
            prevMouseState = currMouseState;
            currMouseState = Mouse.GetState();
        }

        public static bool clicked()
        {
            bool leftMouseButtonClicked = false;
            //Klickar man? (var musen nedtryckt förra rutan och släppt denna?)
            if (prevMouseState.LeftButton == ButtonState.Pressed && currMouseState.LeftButton == ButtonState.Released)
            {
                leftMouseButtonClicked = true;
            }

            return leftMouseButtonClicked;
        }

        public static bool clickedOnItem(List<Item> items, bool leftMouseButtonClicked)
        {
            bool clickedOnItemInRoom = false;
            //Har man klickat ned musen
            if (leftMouseButtonClicked)
            {
                //Om musen befinner sig inom föremålets gränser...
                for (int i = 0; i < items.Count && !clickedOnItemInRoom; i++)
                {
                    if (mouseOverItem(items[i]))
                    {
                        //...blir en bool sann.
                        clickedOnItemInRoom = true;
                        itemClickedOn = items[i];
                    }
                }
            }

            return clickedOnItemInRoom;
        }

        public static Item getClickedItem()
        {
            return itemClickedOn;
        }

        private static bool mouseOverItem(Item item)
        {
            bool isMouseOverItem = false;
            //Beffinner sig musen inom området för spriten?
            //Delar positionen med 3 för spelets grafik dras ut
            if (currMouseState.X / 3 >= item.getSprite().Position.X && currMouseState.X / 3 <= item.getSprite().Position.X + (item.getSprite().FrameSize.X))
            {
                if (currMouseState.Y / 3 >= item.getSprite().Position.Y && currMouseState.Y / 3 <= item.getSprite().Position.Y + (item.getSprite().FrameSize.Y))
                    isMouseOverItem = true;
            }

            return isMouseOverItem;
        }

        public static bool rightClicked()
        {
            bool rightMouseButtonClicked = false;
            //Klickar man? (var musen nedtryckt förra rutan och släppt denna?)
            if (prevMouseState.RightButton == ButtonState.Pressed && currMouseState.RightButton == ButtonState.Released)
            {
                rightMouseButtonClicked = true;
            }

            return rightMouseButtonClicked;
        }


    }
}
