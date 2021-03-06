﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Economy;

namespace Personnage
{
    public class Perso
    {
        public Texture2D perso;
        public Texture2D perso2;
        public Texture2D attack;
		public Texture2D persoAv0;
		public Texture2D persoAv1;
		public Texture2D persoAv2;
		public Texture2D persoAv3;
		public Texture2D perso2Av0;
		public Texture2D perso2Av1;
		public Texture2D perso2Av2;
		public Texture2D perso2Av3;
		Vector2 persoPos = new Vector2(150, 180);
        Vector2 attackPos;
        bool attackOrNot = false;
        int sensPerso = 1;
        int pv;
        Rectangle attackHitBox = new Rectangle();
        Rectangle persoHitBox = new Rectangle();
        bool jumping = false;
        public bool walking = false;
        public bool canJump = false;

        public Perso()
        {
            
        }


// Récupérer le nombre de PV
        public int getnPV()
        {
            return pv;
        }
//Enlever un nombre de PV
        public void hit(int n)
        {
            pv -= n;
        }
//Récupérer le sens du personnage
        public int getSens()
        {
            return sensPerso;
        }
//Retourner le personnage
        public void changeSens(int newSens)
        {
            sensPerso = newSens;
        }
//Récupérer sa position
        public Vector2 getPersoPos()
        {
            return persoPos;
        }
//Définir une nouvelle position pour le personnage
        public void movePerso(Vector2 newPos)
        {
            persoPos = newPos;
        }
//Récupérer la position de son attaque
        public Vector2 getAttPos()
        {
            return attackPos;
        }
//Définir une nouvelle position pour l'attaque
        public void moveAttack(Vector2 newPos)
        {
            attackPos = newPos;
        }
//Savoir si il y a une ataque en cours
        public bool getAttOrNot()
        {
            return attackOrNot;
        }
//Lancer ou arrêter une attaque
        public void attacking(bool attOrNot)
        {
            attackOrNot = attOrNot;
        }
//Définir la hitbox du perso
        public void createAttackHitBox(Rectangle rectangle)
        {
            attackHitBox = rectangle;
        }
//Récupérer la hitbox du perso
        public Rectangle getAttackHitBox()
        {
            return attackHitBox;
        }
        //Définir la hitbox de l'attaque
        public void createPersoHitBox(Rectangle rectangle)
        {
            persoHitBox = rectangle;
        }
//Récupérer la hitbox de l'attaque
        public Rectangle getPersoHitBox()
        {
            return persoHitBox;
        }  
//Savoir si le perso saute
        public bool isJumpingOrNot()
        {
            return jumping;
        } 
//Faire le pero sauter ou pas
        public void jump(bool trueorfalse)
        {
            jumping = trueorfalse;
        }
    }
}
