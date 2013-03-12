using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Library
{
    /// <summary>
    ///  Portaler som förflyttar spelare mellan rum.
    /// </summary>
    public class Door
    {
        public Vector2 position;
        public string nextRoomID;
        public Vector2 door2Position;
        public bool isLocked;
        public string key;
        public string textureID;
        public string dialogInt;
        private AnimatedSprite sprite;
        private Texture2D texture;
        private Dialog dialog;
        private bool isTalking = false;
        private SoundEffect sound;

        public void LoadContent(Game game)
        {
            int tempDialogInt;
            if (int.TryParse(dialogInt, out tempDialogInt))
            {
                dialog = Registry.dialogs[tempDialogInt];
                sound = game.Content.Load<SoundEffect>(@"Sound/Voice/" + tempDialogInt);
            }  
            
            dialog.setFont(game.Content.Load<SpriteFont>(@"textfont"));
            texture = game.Content.Load<Texture2D>(@"Images/Sprites/" + textureID);
            sprite = new AnimatedSprite(texture, position, 0, new Point(texture.Width / 2, texture.Height), new Point(0, 0), new Point(1, 0), 1);
        }
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {

            // -- Händelser baserade på dialoger -- //
            if (dialog.getActiveLine() == "0")                    // Avslutar dialogen
            {
                isTalking = false;
                dialog.resetDialog();
            }
            if (isTalking)
                Registry.pause = true;
            else
                Registry.pause = false;
            // sprite.Update(gameTime, clientBounds);
        }

        public void Unlock()
        {
            isLocked = false;
        }
        public void Lock()
        {
            isLocked = true;
        }
        public void Talk()
        {
            sound.Play();
            isTalking = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, 1f, 0.3332f);

            if (isTalking)
            {
                dialog.Speak(gameTime, spriteBatch, position);
            }
        }

        public Sprite getSprite()
        {
            return sprite;
        }
        public string getKey()
        {
            return key;
        }
    }
}
