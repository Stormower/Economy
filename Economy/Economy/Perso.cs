﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
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
        Vector2 persoPos = new Vector2(250, 250);
        Vector2 attackPos;
        bool attackOrNot = false;
        int sensPerso;
        int pv;
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

       
    }
}
