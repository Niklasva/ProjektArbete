using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library
{
    public class Menu
    {
        private Sprite background;
        private AnimatedSprite openSprite;
        private AnimatedSprite newSprite;

        private bool clickedOnOpen = false;
        private bool clickedOnNew = false;

        //Konstruktor
        public Menu(Texture2D backgroundTexture, Texture2D openTexture, Texture2D newTexture)
        {
            background = new Sprite(backgroundTexture, Vector2.Zero, 0, new Point(backgroundTexture.Width, backgroundTexture.Height));
            openSprite = new AnimatedSprite(openTexture, new Vector2(12, 130), 0, new Point(openTexture.Width / 2, openTexture.Height), new Point(0, 0),
                new Point(2, 0), 16);
            newSprite = new AnimatedSprite(newTexture, new Vector2(12, 144), 0, new Point(newTexture.Width / 2, newTexture.Height), new Point(1, 0),
                new Point(2, 0), 16);
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            
            
            clickedOnNew = false;
            clickedOnOpen = false;
            if (Mousecontrol.clickedOnItem(openSprite.Position, openSprite.FrameSize, Mousecontrol.clicked()))
            {
                clickedOnOpen = true;
            }

            if (Mousecontrol.clickedOnItem(newSprite.Position, newSprite.FrameSize, Mousecontrol.clicked()))
            {
                clickedOnNew = true;
            }

            if (clickedOnNew)
                newSprite.Update(gameTime, clientBounds);
            if (clickedOnOpen)
                openSprite.Update(gameTime, clientBounds);

            if (Mousecontrol.mouseOverItem(openSprite.Position, openSprite.FrameSize)) { openSprite.Update(gameTime, clientBounds); }
            else if (Mousecontrol.mouseOverItem(newSprite.Position, newSprite.FrameSize)) { newSprite.Update(gameTime, clientBounds); }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            background.Draw(gameTime, spriteBatch, 1f, 0.3333333f);
            openSprite.Draw(gameTime, spriteBatch, 1f, 0.3333332f, true);
            newSprite.Draw(gameTime, spriteBatch, 1f, 0.3333332f, true);
        }

        public bool ClickedOnNew
        {
            get
            {
                return clickedOnNew;
            }
            set
            {
                clickedOnNew = value;
            }
        }
        public bool ClickedOnOpen
        {
            get
            {
                return clickedOnOpen;
            }
            set
            {
                clickedOnOpen = value;
            }
        }
    }
}
