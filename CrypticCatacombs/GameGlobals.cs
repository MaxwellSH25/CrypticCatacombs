﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrypticCatacombs
{


    public class GameGlobals
    {
        public static int score = 0;

        public static PassObject PassProjectile, PassMob, PassSpawnPoint, CheckScroll;
    }
}
