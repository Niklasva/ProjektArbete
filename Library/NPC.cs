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
        public List<string> dialogIDs = new List<string>();
        public int itemID;
        public bool GiveItem { get { return giveItem; } }
        public int ItemID { get { return itemID; } }
        public Vector2 getPosition { get { return position; } }
        public Point getFrameSize { get { return frameSize; } }
        public bool getIsTalking { get { return isTalking; } }
        public string wantedItem;
        private List<Dialog> dialogs = new List<Dialog>();
        private List<SoundEffect> dialogAudio = new List<SoundEffect>();
        private Dialog activeDialog;
        private Texture2D stillTexture;
        private Texture2D talkTexture;
        private AnimatedSprite stillSprite;
        private AnimatedSprite talkSprite;
        private AnimatedSprite activeSprite;
        private float layerPosition;
        private Boolean isTalking = false;
        private bool giveItem = false;
        private int dialogNumber = 0;
        private Point frameSize;


        /// <summary>
        /// Laddar in karaktärens
        /// • Texturer
        /// • Dialoger
        /// • Ljud
        /// </summary>
        /// <param name="game">Game</param>
        public void loadContent(Game game)
        {
            foreach (string id in dialogIDs)
            {
                int temp = 0;
                int.TryParse(id, out temp);
                dialogs.Add(Registry.dialogs[temp]);
                dialogAudio.Add(game.Content.Load<SoundEffect>(@"Sound/Voice/" + temp));
            }
            foreach (Dialog dialog in dialogs)
            {//
                dialog.setFont(game.Content.Load<SpriteFont>(@"textfont"));
            }
            activeDialog = Registry.dialogs[0];
            this.talkTexture = game.Content.Load<Texture2D>(@"Images/Characters/" + name + "talk");
            this.stillTexture = game.Content.Load<Texture2D>(@"Images/Characters/" + name);
            frameSize = new Point(stillTexture.Width / 3, stillTexture.Height);
            stillSprite = new AnimatedSprite(stillTexture, position, 0, frameSize, new Point(0, 0), new Point(3, 1), 400);
            talkSprite = new AnimatedSprite(talkTexture, position, 0, new Point(talkTexture.Width / 2, talkTexture.Height), new Point(0, 0), new Point(2, 1), 200);
            this.activeSprite = stillSprite;
        } 
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // När man klickar på NPC:n så börjar dialogen
            if (Mousecontrol.clickedOnItem(position, new Point(stillTexture.Width / 3, stillTexture.Height), Mousecontrol.clicked()) && !isTalking)
            {
                Talk();
            }

            if (activeDialog.getActiveLine() == "0")
            {
                isTalking = false;
            }

            if (activeDialog.getActiveLine() == "give")
            {
                giveItem = true;
                dialogNumber++;
            }

            if (activeDialog.getSpeaker() == "NPC")
            {
                activeSprite = talkSprite;
            }
            else
            {
                activeSprite = stillSprite;
            }

            if (!isTalking)
            {
                activeSprite = stillSprite;
            }

            layerPosition = (1 - (position.Y + activeSprite.Texture.Height) / 180) / 3;

            activeSprite.Update(gameTime, clientBounds);

            if (isTalking)
            {
                Registry.pause = true;
            }
            else
            {
                Registry.pause = false;
            }


        }

        /// <summary>
        /// Startar en dialog mellan spelaren och NPC:n
        /// </summary>
        public void Talk()
        {
            activeDialog.resetDialog();
            activeDialog = dialogs[dialogNumber];
            dialogAudio[dialogNumber].Play();
            isTalking = true;
        }

        /// <summary>
        /// Ritar ut NPC:n och dess dialog
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            activeSprite.Draw(gameTime, spriteBatch, 1f, layerPosition);

            if (isTalking == true)
            {
                activeDialog.Speak(gameTime, spriteBatch, position);
            }
        }

        /// <summary>
        /// NPC:n slutar placera saker framför sig
        /// </summary>
        public void resetItem()
        {
            giveItem = false;
        }

        /// <summary>
        /// Växlar till NPCns andra dialog
        /// Används t.ex när man ger en sak till NPC:n
        /// </summary>
        public void givenItem()
        {
            wantedItem = "null";
            dialogNumber++;
        }
    }

}
