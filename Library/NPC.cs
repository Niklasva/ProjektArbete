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
        //XML
        public String name;                                             // NPCns namn. Används för att ge NPC:n en sprite
        public Vector2 position;
        public List<string> dialogIDs = new List<string>();             // NPCns dialoger. DialogID hämtar en dialog ur Registry.dialogs som i sin tur hämtar dialoger från xml-filen
        public int itemID;                                              // Den item som NPC:n ger bort (om hen gör det) hämtar item ur Registry.items
        public string wantedItem;                                       // Den item som NPC:n vill ha
        public bool lookatplayer;                                       // Om sann, tittar NPC:n alltid åt spelarens håll

        //get metoder
        public bool GiveItem { get { return giveItem; } }               // Kollar om NPC:n ger bort sin item
        public int ItemID { get { return itemID; } }                    // Skickar ID# för NPCns item
        public Vector2 getPosition { get { return position; } }         // Delar med sig av NPCns position
        public Point getFrameSize { get { return frameSize; } }         // Skickar framesize
        public bool getIsTalking { get { return isTalking; } }          // Kollar om NPCn pratar
        
        //privata saker (schhhhh)
        private List<Dialog> dialogs = new List<Dialog>();              // Lista med dialoger. Hämtade ur Registry.dialogs med hjälp av dialogIDs
        private List<SoundEffect> dialogAudio =                         // Dialogens ljud (helst i .mp3-format eller dylikt för att wav-filer är ENORMA)
            new List<SoundEffect>();                                    
        private Dialog activeDialog;                                    // Den nuvarnde dialogen
        private Texture2D stillTexture;                                 // Texturen för NPC:n när hen står still
        private Texture2D talkTexture;                                  //                              pratar
        private AnimatedSprite stillSprite;                             // Animerad sprite för när NPC:n inte pratar
        private AnimatedSprite talkSprite;                              //                              pratar
        private AnimatedSprite activeSprite;                            // Animerad sprite som växlas beroende på om NPC:n pratar eller inte (om NPC:n pratar blir activeSprite = talkSprite, annars stillSprite)
        private float layerPosition;                                    // Håller koll på när NPC:n hamnar bakom saker
        private Boolean isTalking = false;
        private bool giveItem = false;
        private int dialogNumber = 0;                                   // Ändrar den kommande dialogen
        private Point frameSize;                                        // Sprite
        private bool flip;                                              // Används i Draw för att vända på spriten horisontellt


        /// <summary>
        /// Laddar in karaktärens
        /// • Texturer
        /// • Dialoger
        /// • Ljud
        /// </summary>
        /// <param name="game">Game</param>
        public void loadContent(Game game)
        {
            // Laddar in alla dialogers ljud
            foreach (string id in dialogIDs)
            {
                int temp = 0;
                int.TryParse(id, out temp);
                dialogs.Add(Registry.dialogs[temp]);
                dialogAudio.Add(game.Content.Load<SoundEffect>(@"Sound/Voice/" + temp));
            }
            // Tilldelar dialogen ett typsnitt
            foreach (Dialog dialog in dialogs)
            {
                dialog.setFont(game.Content.Load<SpriteFont>(@"textfont"));
            }
            activeDialog = Registry.dialogs[0];
            this.talkTexture = game.Content.Load<Texture2D>(@"Images/Characters/" + name + "talk");
            this.stillTexture = game.Content.Load<Texture2D>(@"Images/Characters/" + name);
            frameSize = new Point(stillTexture.Width / 4, stillTexture.Height);
            stillSprite = new AnimatedSprite(stillTexture, position, 0, frameSize, new Point(0, 0), new Point(3, 1), 600);
            talkSprite = new AnimatedSprite(talkTexture, position, 0, new Point(talkTexture.Width / 2, talkTexture.Height), new Point(0, 0), new Point(2, 1), 100);
            
            this.activeSprite = stillSprite;    // Eftersom NPC:n står still i början, blir activeSprite stillSprite i LoadContent
        } 
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // När man klickar på NPC:n så börjar dialogen
            if (Mousecontrol.clickedOnItem(position, new Point(stillTexture.Width / 3, stillTexture.Height), Mousecontrol.clicked()) && !isTalking)
            {
                Talk();
            }

            // -- Händelser baserade på dialoger -- //
            
            if (activeDialog.getActiveLine() == "give")                 // NPC:n ger bort sin item
            {
                giveItem = true;
                dialogNumber++;
            }
            
            if (activeDialog.getActiveLine() == "0" || Keyboard.GetState().IsKeyDown(Keys.S))                    // Avslutar dialogen
            {
                isTalking = false;
            }

            activeDialog.checkLines();

            // Spriteändringar beroende på om NPC:N pratar
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

            if (position.Y != 0)
            {
                layerPosition = (1 - (position.Y + activeSprite.Texture.Height) / 180) / 3; //3-D EFFEKTER!!!! NPC:n hamnar bakom och framför saker beroende på vart den står
            }
            else
            {
                layerPosition = 0.2f;
            }

            activeSprite.Update(gameTime, clientBounds);

            // Följ efterspelaren
            if (lookatplayer)
            {
                if (position.X < Registry.playerPosition.X)
                {
                    flip = false;
                }
                else
                {
                    flip = true;
                }
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
            activeSprite.Draw(gameTime, spriteBatch, 1f, layerPosition, flip);

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
