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
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using Personnage;
using Mob;



namespace Economy
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D map;
        Texture2D personnage;
        Rectangle mainFrame;
        KeyboardState oldState;
        int angle = 0;
        MouseState oldSourisState;
        Perso perso = new Perso();
        mob[] Mob = new mob[2];
        Rectangle mapCollison;

   //     int sol = 250;
        float limiteSaut;

        Color[] mapTextureData;
        Color[] persoTextureData;

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
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            for (int i = 0; i <= 1; i++)
            {
                Mob[i] = new mob();
            }
            Mob[0].moveMob(new Vector2(500, 250));
            Mob[1].moveMob(new Vector2(600, 250));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<Texture2D>("map");
            mapCollison = GetSmallestRectangleFromTexture(map);
            mapTextureData =
                new Color[map.Width * map.Height];
            map.GetData(mapTextureData);
            perso.perso = Content.Load<Texture2D>("perso");
            perso.perso2 = Content.Load<Texture2D>("perso2");
            persoTextureData =
                new Color[perso.perso.Width * perso.perso.Height];
            perso.perso.GetData(persoTextureData);
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
            uInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(map, mainFrame, Color.White);

            if (perso.getSens() == 1) //Si le perso est dans le sens 1
            {
                personnage = perso.perso;
                perso.createPersoHitBox(new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64));
                perso.moveAttack(new Vector2(perso.getPersoPos().X + 50, perso.getPersoPos().Y)); //et on positionne l'endroit o� appara�tra son attaque si il attaque
            }
            else if (perso.getSens() == 0) //Si il est dans le sens 0
            {
                personnage = perso.perso2;
                perso.createPersoHitBox(new Rectangle((int)perso.getPersoPos().X, (int)perso.getPersoPos().Y, 64, 64));
                perso.moveAttack(new Vector2(perso.getPersoPos().X - 50, perso.getPersoPos().Y)); //idem
            }

            spawn(personnage, perso.getPersoPos(), angle, 1f); //On affiche le personnage

            if (IntersectPixels(mapCollison, mapTextureData, perso.getPersoHitBox(), persoTextureData) && perso.getPersoPos().Y > limiteSaut)
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

            if (!perso.walking && !perso.isJumpingOrNot() )
            {
                // Si le perso ne marche pas et ne saute pas non plus ALORS il tombe
                perso.movePerso(new Vector2(perso.getPersoPos().X, perso.getPersoPos().Y + 4));
            }

            if (perso.getPersoPos().Y <= limiteSaut || IntersectPixels(mapCollison, mapTextureData, perso.getPersoHitBox(), persoTextureData))
            {
                // Si le perso atteint pu dépasse sa limite de saut OU si il touche ka map, il ne saute plus.
                perso.jump(false);
            }

            if (!IntersectPixels(mapCollison, mapTextureData, perso.getPersoHitBox(), persoTextureData))
            {
                // Si le perso ne touche pas la map, on considère qu'il ne marche pas
                perso.walking = false;
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
                {
                    Mob[i].moveMob(new Vector2(700, 250)); //Changer sa position //////RES LE MOB//////
                    Mob[i].killMob(true);  //Le réssusiter
                }
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
                perso.movePerso(new Vector2(perso.getPersoPos().X - 3, perso.getPersoPos().Y));
                perso.changeSens(0);
                angle = 0;
            }
            else if (newState.IsKeyDown(Keys.D))
            {
                perso.movePerso(new Vector2(perso.getPersoPos().X + 3, perso.getPersoPos().Y));
                perso.changeSens(1);
                angle = 0;
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





        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                 Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        public static Rectangle GetSmallestRectangleFromTexture(Texture2D Texture)
        {
            //Create our index of sprite frames
            Color[,] Colors = TextureTo2DArray(Texture);

            //determine the min/max bounds
            int x1 = 9999999, y1 = 9999999;
            int x2 = -999999, y2 = -999999;

            for (int a = 0; a < Texture.Width; a++)
            {
                for (int b = 0; b < Texture.Height; b++)
                {
                    //If we find a non transparent pixel, update bounds if required
                    if (Colors[a, b].A != 0)
                    {
                        if (x1 > a) x1 = a;
                        if (x2 < a) x2 = a;

                        if (y1 > b) y1 = b;
                        if (y2 < b) y2 = b;
                    }
                }
            }

            //We now have our smallest possible rectangle for this texture
            return new Rectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
        }

        //convert texture to 2d array
        private static Color[,] TextureTo2DArray(Texture2D texture)
        {
            //Texture.GetData returns a 1D array
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            //convert the 1D array to 2D for easier processing
            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }


    }
    }




