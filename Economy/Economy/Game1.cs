using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Economy
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldState;
        Texture2D perso;
        Texture2D perso2;
        Texture2D attack;
        Texture2D mob;
        int angle = 0;
        Vector2 persoPos = new Vector2(250, 250);
        Vector2 attackPos;
        Vector2 mobPos = new Vector2(500, 250);
        MouseState oldSourisState;
        bool attackOrNot = false;
        int sensPerso;
        bool mobInLife = true;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.perso = Content.Load<Texture2D>("perso");
            this.perso2 = Content.Load<Texture2D>("perso2");
            this.attack = Content.Load<Texture2D>("attack");
            this.mob = Content.Load<Texture2D>("mob");
        }


        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            uInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if(mobInLife)
            {
                spawn(this.mob, new Vector2(500, 250), 0, 0.5f);
            }
            if (sensPerso == 1)
            {
                spawn(this.perso, persoPos, angle, 1f);
                attackPos = new Vector2(persoPos.X + 50, persoPos.Y);
            }
            else if (sensPerso == 0)
            {
                spawn(this.perso2, persoPos, angle, 1f);
                attackPos = new Vector2(persoPos.X - 50, persoPos.Y);
            }
            
            /*   else if (sensPerso == 2)
               {

               }
               else if (sensPerso == 3)
               {

               }*/
            if (attackOrNot)
            {
                spawn(attack, attackPos, 0, 1f);
                if (attack.Bounds.Intersects(mob.Bounds))
                {
                    mobInLife = false;
                }
                attackOrNot = false;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void uInput()
        {
            KeyboardState newState = Keyboard.GetState();
             MouseState sourisState = Mouse.GetState();
           /* if (newState.IsKeyDown(Keys.Z))
            {
                persoPos.Y--;
            }
            if (newState.IsKeyDown(Keys.S))
            {
                persoPos.Y++;
            }*/
            if (newState.IsKeyDown(Keys.Q))
            {
                persoPos.X--;
                sensPerso = 0;
                angle = 0;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                persoPos.X++;
                sensPerso = 1;
                angle = 0;
                
            }
            oldState = newState;

            if (sourisState.LeftButton == ButtonState.Pressed && oldSourisState.LeftButton != ButtonState.Pressed)
            {
                attackOrNot = true;
            }
            oldSourisState = sourisState;
        }
        private void spawn(Texture2D image, Vector2 position, int angle, System.Single size)
        {
            spriteBatch.Draw(
             image,                                  // Texture (Image)
                  position,                               // Position de l'image
                  null,                                       // Zone de l'image à afficher
                  Color.White,                                // Teinte
                  MathHelper.ToRadians(angle),         // Rotation (en rad)
                  new Vector2(perso.Width / 2, perso.Height / 2),  // Origine
                  size,                                       // Echelle
                  SpriteEffects.None,                        // Effet
                  0);                                         // Profondeur
        }
    }
}
