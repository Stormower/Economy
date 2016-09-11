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
using Maps;



namespace Economy
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D personnage;
        //Rectangle mainFrame;
        KeyboardState oldState;
        MouseState oldSourisState;
        Perso perso = new Perso();
        mob[] Mob = new mob[2];
        Map[] maps = new Map[2];

   //     int sol = 250;
        float limiteSaut;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.graphics.IsFullScreen = false;
            this.graphics.ApplyChanges();
            this.Window.Title = "Jeu";
           // mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            for (int i = 0; i <= 1; i++)
            {
                Mob[i] = new mob();
            }
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i] = new Map();
            }
            Mob[0].moveMob(new Vector2(500, 250));
            Mob[1].moveMob(new Vector2(600, 250));
            perso.movePerso(new Vector2(150, 180));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i].texture = Content.Load<Texture2D>("plateforme");
            }
            perso.perso = Content.Load<Texture2D>("perso");
            perso.perso2 = Content.Load<Texture2D>("perso2");
            personnage = perso.perso;
            perso.attack = Content.Load<Texture2D>("attack");
            for (int i = 0; i <= 1; i++)
            {
                Mob[i].mobText = Content.Load<Texture2D>("mob");
            }
        }


        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            for (int i = 0; i < maps.Length; i++)
            {
                if (!maps[i].hitBox.Intersects(perso.getPersoHitBox()))
                {
                    // Si le perso ne touche pas la map, on considère qu'il ne marche pas
                    perso.walking = false;
                }

                if (maps[i].hitBox.Intersects(perso.getPersoHitBox()) && perso.getPersoPos().Y > limiteSaut)
                {
                    // Si le personnage touche la map ET que sa position est plus basse que sa limite de saut c'est qu'il a touché le sol ALORS
                    // Il marche
                    // Il peut à nouveau sauter
                    perso.walking = true;
                    perso.canJump = true;
                }

                if (perso.isJumpingOrNot() == true)
                {
                    // Si le perso est en train de sauter
                    // Il monte
                    perso.movePerso(new Vector2(perso.getPersoPos().X, perso.getPersoPos().Y - 4));
                }

                if (!perso.walking && !perso.isJumpingOrNot())
                {
                    // Si le perso ne marche pas et ne saute pas non plus 
                    // il tombe
                    perso.movePerso(new Vector2(perso.getPersoPos().X, perso.getPersoPos().Y + 4));
                }

                if (perso.getPersoPos().Y <= limiteSaut || maps[i].hitBox.Intersects(perso.getPersoHitBox()))
                {
                    // Si le perso atteint pu dépasse sa limite de saut OU si il touche ka map, il ne saute plus.
                    perso.jump(false);
                }

                
            }

            uInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spawn(personnage, perso.getPersoPos(), 0, 1f); //On affiche le personnage

            spawn(maps[0].texture, new Vector2(150, 400), 0, 1f);                                                   // On affiche chaque carré de map
            maps[0].hitBox = new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64);       // Et on définit leur hitbox
            spawn(maps[1].texture, new Vector2(300, 400), 0, 1f);                                                   // On affiche chaque carré de map
            maps[1].hitBox = new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64);       // Et on définit leur hitbox


            if (perso.getSens() == 1) //Si le perso est dans le sens 1
            {
                personnage = perso.perso;
                perso.createPersoHitBox(new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64));
                perso.moveAttack(new Vector2(perso.getPersoPos().X + 50, perso.getPersoPos().Y)); //et on positionne l'endroit o� appara�tra son attaque si il attaque
            }
            else  //Si il est dans le sens 0
            {
                personnage = perso.perso2;
                perso.createPersoHitBox(new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64));
                perso.moveAttack(new Vector2(perso.getPersoPos().X - 50, perso.getPersoPos().Y)); //idem
            }
            
            for (int i = 0; i <= 1; i++)
            {
                if (Mob[i].mobInLifeOrNot()) // Si le mob est en vie
                {
                    spawn(Mob[i].mobText, Mob[i].getMobPos(), 0, 0.5f); //One le spawn
                    Mob[i].createMobHitBox(new Rectangle((int)Mob[i].getMobPos().X, (int)Mob[i].getMobPos().Y, 64, 64));//Et on d�finit sa hitbox
                }
            }
           

            /*   else if (sensPerso == 2)
               {

               }
               else if (sensPerso == 3)
               {

               }*/ //Au d�but on voulait 2 directions mais une �a ira pour le d�but
            if (perso.getAttOrNot()) //Si le perso attaque
            {
                spawn(perso.attack, perso.getAttPos(), 0, 1f); //On spawn l'attaque du perso
                perso.createAttackHitBox(new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64)); // On cr�e sa hitbox
                for (int i = 0; i <= 1; i++)
                {
                    if (perso.getAttackHitBox().Intersects(Mob[i].getMobHitBox())) //Si elle touche le mob
                    {
                        Mob[i].killMob(false); // On tue le mob

                    }
                }
                perso.attacking(false); // Le perso n'attaque plus
            }

            for (int i = 0; i <= 1; i++)
            {
                Mob[i].moveMob(new Vector2(Mob[i].getMobPos().X - 1, Mob[i].getMobPos().Y)); //On déplace le mob d'un pixel a chaque refresh
            }
            spriteBatch.End();
            base.Draw(gameTime);

            for (int i = 0; i < Mob.Length; i++)
            {
                if (!Mob[i].mobInLifeOrNot()) // Si le Mob est mort
                {                                                                 /////////////////////
                    Mob[i].moveMob(new Vector2(700, 250)); //Changer sa position //////RES LE MOB//////
                    Mob[i].killMob(true);  //Le réssusiter                       //////////////////////
                }
            }
        }

        private void uInput()
        {
            KeyboardState newState = Keyboard.GetState();
            MouseState sourisState = Mouse.GetState();

            if (newState.IsKeyDown(Keys.Q))
            {
                perso.movePerso(new Vector2(perso.getPersoPos().X - 3, perso.getPersoPos().Y));
                perso.changeSens(0);
            }
            else if (newState.IsKeyDown(Keys.D))
            {
                perso.movePerso(new Vector2(perso.getPersoPos().X + 3, perso.getPersoPos().Y));
                perso.changeSens(1);
            }

            if (newState.IsKeyDown(Keys.Z) && !oldState.IsKeyDown(Keys.Z) && perso.canJump == true)
            {
                perso.jump(true);
                perso.canJump = false;
                perso.walking = false;
                limiteSaut = perso.getPersoPos().Y - 100;
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
                  null,                                       // Zone de l'image � afficher
                  Color.White,                                // Teinte
                  MathHelper.ToRadians(angle),         // Rotation (en rad)
                  new Vector2(perso.perso.Width, perso.perso.Height),  // Origine
                  size,                                       // Echelle
                  SpriteEffects.None,                        // Effet
                  0);                                         // Profondeur
        }



    }
    }




