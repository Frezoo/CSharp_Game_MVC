using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace The_Rotting_MVC.Model
{
    public class AmmoBoxModel
    {
        public Vector2 Position;
        public int AmmoAmount;

        public AmmoBoxModel(Vector2 position, int ammoAmount)
        {
            Position = position;
            AmmoAmount = ammoAmount;
        }
    }
}
