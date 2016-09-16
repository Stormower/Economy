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

namespace Maps
{

    public class Map
    {

        public Texture2D texture;
        public Rectangle topHitBox;
		public Rectangle botHitBox;
		public Rectangle leftHitBox;
		public Rectangle rightHitBox;



        public Map()
        {
        }
    }
}
