using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic; 
using The_Rotting_MVC.Controller;
using The_Rotting_MVC.Model;
using The_Rotting_MVC.View;

namespace The_Rotting_MVC.Controller
{
    public class BulletController
    {
        private readonly List<BulletModel> _bullets;
        private readonly List<ZombieModel> _zombies;

        private readonly BulletView _bulletView;


        private readonly List<ZombieView> _zombieViews;

        public BulletController(
            List<BulletModel> bullets,
            List<ZombieModel> zombies,
            BulletView bulletView,
            List<ZombieView> zombieViews)
        {
            _bullets = bullets;
            _zombies = zombies;

            _bulletView = bulletView;
            _zombieViews = zombieViews;
        }

        public void ProcessBullets(float deltaTime,int textureWidth,int textureHeight)
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                var bullet = _bullets[i];

                Rectangle bulletBounds = new Rectangle(
                    (int)bullet.Position.X,
                    (int)bullet.Position.Y,
                    _bulletView._texture.Width,
                    _bulletView._texture.Height
                );

                for (int zombieNumber = 0; zombieNumber < _zombies.Count; zombieNumber++)
                {
                    Rectangle zombieBounds = new Rectangle(
                        (int)_zombies[zombieNumber].Position.X - (int)246 / 2,
                        (int)_zombies[zombieNumber].Position.Y - (int)311 / 2,
                        textureWidth / 16,
                        textureHeight
                    );

                    if (CheckCollision(bulletBounds, zombieBounds))
                    {
                        _zombies[zombieNumber].GetDamage(bullet.BulletDamage);
                        _zombieViews[zombieNumber].IsTakingDamage = true; 
                        _bullets.RemoveAt(i);
                        break;
                    }
                }

                if (bullet.IsOffScreen(1280, 720))
                {
                    _bullets.RemoveAt(i);
                }
            }
        }

        public bool CheckCollision(Rectangle fristRectangle, Rectangle secondRectangle)
        {
            return fristRectangle.Intersects(secondRectangle);
        }
    }
}