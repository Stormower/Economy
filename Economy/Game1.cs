using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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
		Texture2D attackD;
		//Rectangle mainFrame;
		KeyboardState oldState;
		MouseState oldSourisState;
		Perso perso = new Perso();
		mob[] Mob = new mob[2];
		Map[] maps = new Map[16];
		bool b = true;
		bool r;
		bool l;
		bool t;
		int vitChutte;
		bool refreshImages = true;
		bool attackDistance;
		bool isAttackDistanceHere = false;
		Vector2 attDPos = new Vector2();
		int framePerso = 0;

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
			perso.persoAv0 = Content.Load<Texture2D> ("perso_av_0");
			perso.persoAv1 = Content.Load<Texture2D> ("perso_av_1");
			perso.persoAv2 = Content.Load<Texture2D> ("perso_av_2");
			perso.persoAv3 = Content.Load<Texture2D> ("perso_av_3");
			perso.perso2Av0 = Content.Load<Texture2D> ("perso2_av_0");
			perso.perso2Av1 = Content.Load<Texture2D> ("perso2_av_1");
			perso.perso2Av2 = Content.Load<Texture2D> ("perso2_av_2");
			perso.perso2Av3 = Content.Load<Texture2D> ("perso2_av_3");
			personnage = perso.persoAv0;
			perso.attack = Content.Load<Texture2D>("attack");
			attackD = Content.Load<Texture2D> ("attack2");
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
			this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);

			foreach (Map map in maps)
			{

				if (map.topHitBox.Intersects(perso.getPersoHitBox()) && perso.getPersoPos().Y > limiteSaut)
				{
					// Si le personnage touche la map ET que sa position est plus basse que sa limite de saut c'est qu'il a touché le sol ALORS
					// Il marche
					// Il peut à nouveau sauter
					perso.walking = true;
					perso.canJump = true;
					b = false;
					break;
				}



				if (perso.getPersoPos().Y <= limiteSaut || map.topHitBox.Intersects(perso.getPersoHitBox()) || vitChutte == 2)
				{
					// Si le perso atteint pu dépasse sa limite de saut OU si il touche la map, il ne saute plus. (ou si on presse s)
					perso.jump(false);
				}
			}

			if (!perso.isJumpingOrNot ()) {
				for (int i = 0; i < 10 * vitChutte; i++) {
					foreach (Map map in maps) {
						if (!map.topHitBox.Intersects (perso.getPersoHitBox ()))
							b = true;
						else {
							b = false;
							break;
						}
					}
					if (b) {
						if (refreshImages)
							perso.movePerso (new Vector2 (perso.getPersoPos ().X, perso.getPersoPos ().Y + 1));	
					}
				}
			}

			if (perso.isJumpingOrNot ()) {
				for (int i = 0; i < 6; i++) {
					foreach (Map map in maps) {
						if (!map.botHitBox.Intersects (perso.getPersoHitBox ()))
							t = true;
						else {
							t = false;
							b = true;
							break;
						}
					}	
					if (t) {
						if (refreshImages)
							perso.movePerso (new Vector2 (perso.getPersoPos ().X, perso.getPersoPos ().Y - 1));	
					}
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

			                                                     

			popMap(0, 150, 400);
			popMap(1, 300, 400);
			popMap(2, 600, 400);
			popMap(3, 485, 200);
			popMap(4, 750, 300);
			popMap(5, 300, 200);
			popMap(6, 600, 200);
			popMap(7, 600, 400);
			popMap(8, 485, 200);
			popMap(9, 750, 300);
			popMap(10, 364, 400);
			popMap(11, 300, 400);
			popMap(12, 600 -64, 400);
			popMap(13, 485 + 64, 200);
			popMap(14, 750 - 64, 350);
			popMap(15, 364, 200);

			if (attackDistance) {
				if (!isAttackDistanceHere) {
					attDPos = perso.getAttPos ();
					isAttackDistanceHere = !isAttackDistanceHere;
				} else {
					if (perso.getSens() == 1)
						attDPos = new Vector2 (attDPos.X + 15, attDPos.Y);
					else 
						attDPos = new Vector2 (attDPos.X - 15, attDPos.Y);
									}
				spawn(attackD, attDPos, 0, 1f);
			}

			if (attDPos.X > 1000 || attDPos.X < 0) {
				attackDistance = false;
				isAttackDistanceHere = false;
			}

			if (perso.getSens () == 1) { //Si le perso est dans le sens 1
				if (r) {
					if (framePerso < 15 /2)
						personnage = perso.persoAv0;
					else if (framePerso < 15 && framePerso > 15/2)
						personnage = perso.persoAv1;
					else if (framePerso > 15 && framePerso < 45/2)
						personnage = perso.persoAv2;
					else
						personnage = perso.persoAv3;
					r = false;
				}
					perso.createPersoHitBox (new Rectangle ((int)perso.getPersoPos ().X + 20, (int)perso.getPersoPos ().Y , 4, 64));
					if (refreshImages)
						perso.moveAttack (new Vector2 (perso.getPersoPos ().X + 50, perso.getPersoPos ().Y)); //et on positionne l'endroit o� appara�tra son attaque si il attaque
				}
			else  //Si il est dans le sens 0
			{
				if (l) {
					if (framePerso < 15 /2)
						personnage = perso.perso2Av0;
					else if (framePerso < 15 && framePerso > 15/2)
						personnage = perso.perso2Av1;
					else if (framePerso > 15 && framePerso < 45/2)
						personnage = perso.perso2Av2;
					else
						personnage = perso.perso2Av3;
					l = false;
				}
				perso.createPersoHitBox(new Rectangle((int)perso.getPersoPos().X + 24, (int)perso.getPersoPos().Y + 53, 10, 10));
				if (refreshImages)
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
				perso.createAttackHitBox(new Rectangle((int)perso.getAttPos().X, (int)perso.getAttPos().Y, 64, 64)); // On cr�e sa hitbox
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
				if (refreshImages)
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
			refreshImages = !refreshImages;
			framePerso++;
			if (framePerso >= 30)
				framePerso = 0;
		}

		private void uInput()
		{
			KeyboardState newState = Keyboard.GetState();
			MouseState sourisState = Mouse.GetState();

			if (newState.IsKeyDown(Keys.A))
			{
				foreach (Map map in maps)
				{
					if (refreshImages && !perso.getPersoHitBox ().Intersects (map.rightHitBox)) {
						l = true;

					} else {
						l = false;
						break;
					}
				}
				if (l) {
					perso.movePerso (new Vector2 (perso.getPersoPos ().X - 6, perso.getPersoPos ().Y));
				}
				perso.changeSens(0);
			}
			else if (newState.IsKeyDown(Keys.D))
			{
				foreach (Map map in maps)
				{
					if (refreshImages && !perso.getPersoHitBox ().Intersects (map.leftHitBox)) {
						r = true;

					} else {
						r = false;
						break;
					}
				}
				if (r) {
					perso.movePerso (new Vector2 (perso.getPersoPos ().X + 6, perso.getPersoPos ().Y));
				}
				perso.changeSens(1);
			}
			if (newState.IsKeyDown(Keys.W) && !oldState.IsKeyDown(Keys.W) && perso.canJump == true && !b)
			{
				perso.jump(true);
				perso.canJump = false;
				perso.walking = false;
				limiteSaut = perso.getPersoPos().Y - 100;
			}

			if ((newState.IsKeyDown(Keys.Back) && !oldState.IsKeyDown(Keys.Back)) || perso.getPersoPos().Y > 800)
			{
				perso.movePerso(new Vector2(150, 180));
				Mob[0].moveMob(new Vector2(500, 250));
				Mob[1].moveMob(new Vector2(600, 250));
			}

			if (newState.IsKeyDown(Keys.S))
			{
				vitChutte = 2;
			}

			else
			{
				vitChutte = 1;
			}
			oldState = newState;

			if (sourisState.LeftButton == ButtonState.Pressed && oldSourisState.LeftButton != ButtonState.Pressed)
			{
				perso.attacking(true);
			}
			if (sourisState.RightButton == ButtonState.Pressed && oldSourisState.RightButton != ButtonState.Pressed && !attackDistance)
			{
				attackDistance = true;
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


		private void popMap(int nMap, int posX, int posY)
		{
			spawn(maps[nMap].texture, new Vector2(posX, posY), 0, 1f);                                    // On affiche chaque carré de map
			maps[nMap].topHitBox = new Rectangle(posX, posY, 64, 10);                                     // Et on définit leur hitbox
			maps[nMap].leftHitBox = new Rectangle(posX - 15, posY + 24, 24, 40);
			maps[nMap].rightHitBox = new Rectangle (posX + 49, posY + 24, 24, 40);
			maps[nMap].botHitBox = new Rectangle(posX, posY + 64, 64, 10);  
		}

	}
}




