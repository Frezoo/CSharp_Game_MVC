using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Rotting_MVC.Model
{
    public class BulletModel
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float BulletDamage = 0.3f;
        public float Rotation;
        private float speed = 2800f;

        public BulletModel(Vector2 position, Vector2 direction, float rotation)
        {
            Position = position;
            Direction = direction;
            Rotation = rotation;
        }

        public void UpdateBulletPosition(float deltaTime)
        {
            Position += Direction * speed * deltaTime;
        }

        public bool IsOffScreen(int screenWidth, int screenHeight)
        {
            return Position.X < 0 || Position.X > screenWidth || Position.Y < 0 || Position.Y > screenHeight;
        }

    }
}
