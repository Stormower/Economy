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

namespace Mob
{
    public class mob
    {
        public Texture2D mobText;
        Vector2 mobPos = new Vector2(500, 250);
        bool mobInLife = true;
        public mob()
        {
        }
//Le mob est-il en vie ?
        public bool mobInLifeOrNot()
        {
            return mobInLife;
        }
//Tuer le mob
        public void killMob()
        {
            mobInLife = false;
        }
    }
}