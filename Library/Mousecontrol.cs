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

        private static string description;
        private static string name;

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

        public static bool clickedOnItem(Vector2 position, Point frameSize, bool leftMouseButtonClicked)
        {
            bool clickedOnItem = false;
            //Har man klickat ned musen
            if (leftMouseButtonClicked)
            {
                //Om musen befinner sig inom föremålets gränser...
                if (mouseOverItem(position, frameSize))
                {
                    //...blir en bool sann.
                    clickedOnItem = true;
                }

            }
            return clickedOnItem;
        }

        private static bool mouseOverItem(Vector2 position, Point frameSize)
        {
            bool isMouseOverItem = false;
            //Beffinner sig musen inom området för spriten?
            //Delar positionen med 3 för spelets grafik dras ut
            if (currMouseState.X / 3 >= position.X && currMouseState.X / 3 <= position.X + frameSize.X)
            {
                if (currMouseState.Y / 3 >= position.Y && currMouseState.Y / 3 <= position.Y + frameSize.Y)
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

        public static bool rightClickedOnItem(Item item)
        {
            bool rightClickedOnItem = false;

            if (clickedOnItem(item.getSprite().Position, item.getSprite().FrameSize, true))
            {
                rightClickedOnItem = true;
                description = item.description;
                name = item.name;
            }

            return rightClickedOnItem;
        }
        public static bool rightClickedOnItem(Door door)
        {            bool rightClickedOnItem = false;

            if (clickedOnItem(door.getSprite().Position, door.getSprite().FrameSize, true))
            {
                rightClickedOnItem = true;
                if (door.isLocked)
                    description = "Locked";
                else
                    description = "Open";
            }

            return rightClickedOnItem;
        }

        public static string getDescription()
        {
            return description;
        }

        public static string getName()
        {
            return name;
        }

        public static bool inProximityToItem(Vector2 position, Point frameSize)
        {
            /// <summary>
            /// Kollar spelar befinner sig i närheten av ett föremål. Skicka med föremålets position och storlek. 
            /// </summary>
            bool isInProximity = false;
            //Befinner sig spelare inom en visst område runt föremålet?
            if ((Registry.playerPosition.X + 15) >= position.X && (Registry.playerPosition.X - 10) <= (position.X + frameSize.X) &&
                (Registry.playerPosition.Y + 67) >= position.Y && (Registry.playerPosition.Y - 10) <= (position.Y + frameSize.Y))
            {
                isInProximity = true;
            }

            return isInProximity;
        }
    }
}
