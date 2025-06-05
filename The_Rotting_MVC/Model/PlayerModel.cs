using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using The_Rotting_MVC.View;

namespace The_Rotting_MVC.Model
{
    public class PlayerModel
    {
        public Vector2 Position;
        public Vector2 Direction;
        public  Vector2 Size;

        public float Rotation;

        private float _speed;
        public float Health = 1f;
        public int Ammo = 30;
        public int MaxAmmo = 90;
        public float AmmoTriggerDistation = 50f;

        private float _shootCooldown = 0.05f;
        public float TimeSinceLastShot = 0f;

        public float ReloadCooldown = 1f;
        public float TimeSinceLastReload = 1f;

        public List<BulletModel> Bullets = new List<BulletModel>();
        public PlayerModel()
        {
            Position = new Vector2(640, 360);
            Direction = Vector2.Zero;
            Rotation = 0f;
            _speed = 750f;
            Size = new Vector2(654, 518) * 014f;
        }

        public void MovePlayer(float deltaTime)
        {
            Vector2 newPosition = Position + Direction * _speed * deltaTime;
            if (Direction != Vector2.Zero)
            {
                Direction.Normalize();
                if (newPosition.X >= 40 && newPosition.X <= 1240 && newPosition.Y >= 40 && newPosition.Y <= 680)
                {
                    Position = newPosition;
                }
            }
            Direction = Vector2.Zero;
        }

        public void UpdateRotation(Vector2 mousePosition)
        {
            float deltaX = mousePosition.X - Position.X;
            float deltaY = mousePosition.Y - Position.Y;
            Rotation = MathF.Atan2(deltaY, deltaX);
        }

        public void UpdateTimers(float deltaTime)
        {
            TimeSinceLastShot += deltaTime;
            TimeSinceLastReload += deltaTime;
        }

        public void Shoot()
        {
            if (TimeSinceLastShot >= _shootCooldown && Ammo - 1 >= 0 && TimeSinceLastReload >= ReloadCooldown)
            {
                Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                BulletModel newBullet = new BulletModel(Position, direction, Rotation);
                Bullets.Add(newBullet);
                TimeSinceLastShot = 0f;
                Ammo--;
            }
        }

        public void Reload()
        {
            if (TimeSinceLastReload >= ReloadCooldown)
            {
                int ammoToAdd = Math.Min(30 - Ammo, MaxAmmo);
                if (ammoToAdd > 0)
                {
                    Ammo += ammoToAdd;
                    MaxAmmo -= ammoToAdd;
                    TimeSinceLastReload = 0f;
                }
            }
        }

        public void HandleAmmoCollection(AmmoHandler ammoHandler)
        {
            for (int i = ammoHandler.AmmoBoxes.Count - 1; i >= 0; i--)
            {
                if (Vector2.Distance(Position, ammoHandler.AmmoBoxes[i].Position) < AmmoTriggerDistation)
                {
                    AddAmmo(ammoHandler.AmmoBoxes[i].AmmoAmount);
                    ammoHandler.CollectBox(i);
                }
            }
        }

        public void AddAmmo(int amount)
        {
            MaxAmmo += amount;
        }

        public void ChangeHealth(float delta)
        {
            Health -= delta;
        }

        public void HandleMoveUp()
        {
            Direction.Y -= 1;
        }

        public void HandleMoveDown()
        {
            Direction.Y += 1;
        }

        public void HandleMoveLeft()
        {
            Direction.X -= 1;
        }

        public void HandleMoveRight()
        {
            Direction.X += 1;
        }

        public void HandleShoot()
        {
            Shoot();
        }

        public void HandleReload()
        {
            Reload();
        }

        public void HandleMousePositionChanged(Vector2 mousePosition)
        {
            UpdateRotation(mousePosition);
        }


    }
}