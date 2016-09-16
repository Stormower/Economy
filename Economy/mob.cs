using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Economy;

namespace Mob
{
    public class mob
    {
        public Texture2D mobText;
        Vector2 mobPos;
        bool mobInLife = true;
        Rectangle mobHitBox = new Rectangle();
       


        public mob()
        {
        }
//Le mob est-il en vie ?
        public bool mobInLifeOrNot()
        {
            return mobInLife;
        }
//Tuer le mob
        public void killMob(bool inLife)
        {
            mobInLife = inLife;
        }
//Définir la hitbox du mob
        public void createMobHitBox(Rectangle rectangle)
        {
            mobHitBox = rectangle;
        }
//Récupérer la hitbox du mob
        public Rectangle getMobHitBox()
        {
            return mobHitBox;
        }
//Récupérer sa position
        public Vector2 getMobPos()
        {
            return mobPos;
        }
//Définir une nouvelle position pour le mob
        public void moveMob(Vector2 newPos)
        {
            mobPos = newPos;
        }
    }
}
