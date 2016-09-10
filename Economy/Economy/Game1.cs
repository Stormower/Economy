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
using Mob;


namespace Economy
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldState;        
        int angle = 0;
        MouseState oldSourisState;       
        Perso perso = new Perso();
        mob Mob = new mob();


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
            Mob.mobText = Content.Load<Texture2D>("mob");
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
            if(Mob.mobInLifeOrNot()) // Si le mob est en vie
            {
                spawn(Mob.mobText, Mob.getMobPos(), 0, 0.5f); //One le spawn
                Mob.createMobHitBox(new Rectangle((int)Mob.getMobPos().X, (int)Mob.getMobPos().Y, 64, 64));//Et on définit sa hitbox
            }
            if (perso.getSens() == 1) //Si les perso est dans le sens 1
            {
                spawn(perso.perso, perso.getPersoPos(), angle, 1f); // on affiche le perso
                perso.moveAttack(new Vector2(perso.getPersoPos().X + 50, perso.getPersoPos().Y)); //et on positionne l'endroit où apparaîtra son attaque si il attaque
            }
            else if (perso.getSens() == 0) //Si il est dans le sens 0
            {
                spawn(perso.perso2, perso.getPersoPos(), angle, 1f); //On affiche le perso2
                perso.moveAttack(new Vector2(perso.getPersoPos().X - 50, perso.getPersoPos().Y)); //idem
            }
            
            /*   else if (sensPerso == 2)
               {

               }
               else if (sensPerso == 3)
               {

               }*/ //Au début on voulait 2 directions mais une ça ira pour le début
            if (perso.getAttOrNot()) //Si le perso attaque
            {
                spawn(perso.attack, perso.getAttPos(), 0, 1f); //On spawn l'attaque du perso
                perso.createAttackHitBox(new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64)); // On crée sa hitbox
                if (perso.getAttackHitBox().Intersects(Mob.getMobHitBox())) //Si elle touche le mob
                {
                    Mob.killMob(false); // On tue le mob
                    
                }
                perso.attacking(false); // Le perso n'attaque plus
            }
            Mob.moveMob(new Vector2(Mob.getMobPos().X - 1, Mob.getMobPos().Y)); //On déplace le mob d'un pixel à chaque refresh
            spriteBatch.End();
            base.Draw(gameTime);


            if (!Mob.mobInLifeOrNot()) // Si le Mob est mort
            {
                Mob.moveMob(new Vector2(700, 250)); //Changer sa position
                Mob.killMob(true);  //Le réssusiter
            }
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
