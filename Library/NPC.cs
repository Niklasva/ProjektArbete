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
    /// <summary>
    ///  Icke-spelarkaraktärer.
    /// </summary>
    public class NPC
    {
        public String name;
        public Vector2 position;
        private Dialog dialog;
        private Dialog dialog2;
        public int dialogID;
        public int dialog2ID;
        public int itemID;
        private Texture2D stillTexture;
        private Texture2D talkTexture;
        private AnimatedSprite stillSprite;
        private AnimatedSprite talkSprite;
        private AnimatedSprite activeSprite;
        private float layerPosition;
        private Boolean isTalking = false;
        private SoundEffect dialogSound;
        private SoundEffect dialog2Sound;
        private bool giveItem = false;
        private bool dialogSwitch = false;
        private Point frameSize;

        public bool GiveItem { get { return giveItem; }}
        public int ItemID { get { return itemID; } }
        public Vector2 getPosition { get { return position; } }
        public Point getFrameSize { get { return frameSize; } }

        /// <summary>
        /// Laddar in karaktärens
        /// • Texturer
        /// • Dialoger
        /// • Ljud
        /// </summary>
        /// <param name="game">Game</param>
        public void loadContent(Game game)
        {
            this.talkTexture = game.Content.Load<Texture2D>(@"Images/Characters/" + name + "talk");
            this.stillTexture = game.Content.Load<Texture2D>(@"Images/Characters/" + name);
            frameSize = new Point(stillTexture.Width / 3, stillTexture.Height);
            stillSprite = new AnimatedSprite(stillTexture, position, 0, new Point(stillTexture.Width / 3, stillTexture.Height), new Point(0, 0), new Point(3, 1), 200);
            talkSprite = new AnimatedSprite(talkTexture, position, 0, new Point(talkTexture.Width / 3, talkTexture.Height), new Point(0, 0), new Point(3, 1), 200);
            dialog = Registry.dialogs[dialogID];
            dialog2 = Registry.dialogs[dialog2ID];
            dialog.setFont(game.Content.Load<SpriteFont>(@"textfont"));
            dialog2.setFont(game.Content.Load<SpriteFont>(@"textfont"));
            this.activeSprite = stillSprite;
            this.dialogSound = game.Content.Load<SoundEffect>(@"Sound/Voice/" + dialogID);
            this.dialog2Sound = game.Content.Load<SoundEffect>(@"Sound/Voice/" + dialog2ID);
        }
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {

            // När man klickar på NPC:n så börjar dialogen
            if (Mousecontrol.clickedOnItem(position, new Point(stillTexture.Width / 3, stillTexture.Height), Mousecontrol.clicked()) && !isTalking)
            {
                Talk();
            }

            if (dialog.getActiveLine() == "0")
            {
                isTalking = false;
            }

            ///
            if (dialog.getSpeaker() == "NPC")
            {
                activeSprite = talkSprite;
            }
            else
            {
                activeSprite = stillSprite;
            }
            ///

            if (dialog.getActiveLine() == "give")
            {
                giveItem = true;
                dialogSwitch = true;
            }

            if (!isTalking)
            {
                activeSprite = stillSprite;
            }

            layerPosition = (1 - (position.Y + activeSprite.Texture.Height) / 180) / 3;

            activeSprite.Update(gameTime, clientBounds);
        }

        /// <summary>
        /// Startar en dialog mellan spelaren och NPC:n
        /// </summary>
        public void Talk()
        {
            dialog.resetDialog();

            // NPC:n ändrar dialog
            if (dialogSwitch)
            {
                dialog = dialog2;
                dialog2Sound.Play();
            }

            isTalking = true;

            if (!dialogSwitch)
            {
                dialogSound.Play();
            }
        }

        /// <summary>
        /// Ritar ut NPC:n och dess dialog
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            activeSprite.Draw(gameTime, spriteBatch, 1f, layerPosition);

            if (isTalking == true)
            {
                dialog.Speak(gameTime, spriteBatch, position);
            }
        }

        /// <summary>
        /// NPC:n slutar placera saker framför sig
        /// </summary>
        public void resetItem()
        {
            giveItem = false;
        }
    }

}
