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
using Personnage;


namespace Economy
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldState;        
        Texture2D mob;
        int angle = 0;
        Vector2 mobPos = new Vector2(500, 250);
        MouseState oldSourisState;       
        bool mobInLife = true;
        Perso perso = new Perso();


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
            perso.perso = Content.Load<Texture2D>("perso");
            perso.perso2 = Content.Load<Texture2D>("perso2");
            perso.attack = Content.Load<Texture2D>("attack");
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
            if (perso.getSens() == 1)
            {
                spawn(perso.perso, perso.getPersoPos(), angle, 1f);
                perso.moveAttack(new Vector2(perso.getPersoPos().X + 50, perso.getPersoPos().Y));
            }
            else if (perso.getSens() == 0)
            {
                spawn(perso.perso2, perso.getPersoPos(), angle, 1f);
                perso.moveAttack(new Vector2(perso.getPersoPos().X - 50, perso.getPersoPos().Y));
            }
            
            /*   else if (sensPerso == 2)
               {

               }
               else if (sensPerso == 3)
               {

               }*/
            if (perso.getAttOrNot())
            {
                spawn(perso.attack, perso.getAttPos(), 0, 1f);
                if (perso.attack.Bounds.Intersects(mob.Bounds))
                {
                    mobInLife = false;
                }
                perso.attacking(false);
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
                perso.movePerso(new Vector2(perso.getPersoPos().X - 1, perso.getPersoPos().Y));
                perso.changeSens(0);
                angle = 0;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                perso.movePerso(new Vector2(perso.getPersoPos().X + 1, perso.getPersoPos().Y));
                perso.changeSens(1);
                angle = 0;
                
            }
            oldState = newState;

            if (sourisState.LeftButton == ButtonState.Pressed && oldSourisState.LeftButton != ButtonState.Pressed)
            {
                perso.attacking(true);
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
                  new Vector2(perso.perso.Width / 2, perso.perso.Height / 2),  // Origine
                  size,                                       // Echelle
                  SpriteEffects.None,                        // Effet
                  0);                                         // Profondeur
        }
    }
}
