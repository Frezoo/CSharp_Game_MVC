using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace The_Rotting_MVC.Model
{
    public class ZombieModel
    {
        public Vector2 Position;
        public float Rotation;
        public float Speed;
        public float Damage = 0.1f;
        public float Health = 1f;

        private PlayerModel _player;
        private float distanceToPlayer;
        public bool IsAlive = true;

        const float distanceToUpdateRotation = 30;
        const float radiusOfSoftRepulsion = 100;
        const float minimumDistanceBetweenZombies = 40;
        const float distanceToAttack = 3;

        public ZombieModel(Vector2 position , PlayerModel player)
        {
            Position = position;
            _player = player;
            Random random = new Random();
            Speed = 300f + (float)(random.NextDouble() * 40f - 20f);
        }

        public void MoveToPlayer(float deltaTime, List<ZombieModel> allZombies)
        {
            // Вычисляем направление к игроку
            Vector2 direction = _player.Position - Position;
            distanceToPlayer = direction.Length();

            // Нормализуем вектор направления
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            // Обновляем угол поворота, только если зомби далеко от игрока
            if (distanceToPlayer >= distanceToUpdateRotation)
            {
                Rotation = (float)Math.Atan2(direction.Y, direction.X);
            }

          
            foreach (var otherZombie in allZombies)
            {
                if (otherZombie != this) // Исключаем текущего зомби
                {
                    float distanceToOtherZombie = Vector2.Distance(Position, otherZombie.Position);

                    // Мягкое отталкивание на большом расстоянии
                    if (distanceToOtherZombie < radiusOfSoftRepulsion)
                    {
                        Vector2 repulsionDirection = Position - otherZombie.Position;
                        repulsionDirection.Normalize();
                        direction += repulsionDirection * (100f - distanceToOtherZombie) / 100f; // Чем ближе, тем сильнее отталкивание
                    }

                    // Сильное отталкивание при близком контакте
                    if (distanceToOtherZombie < minimumDistanceBetweenZombies)
                    {
                        Vector2 strongRepulsionDirection = Position - otherZombie.Position;
                        strongRepulsionDirection.Normalize();
                        direction += strongRepulsionDirection * 0.5f; // Сильная сила отталкивания
                    }
                }
            }

            // Нормализуем направление после всех изменений
            direction.Normalize();

            // Обновляем позицию зомби
            Position += direction * Speed * deltaTime;
        }

        public void Update(float deltaTime,List<ZombieModel> zombies)
        {
            MoveToPlayer(deltaTime, zombies);
            Attack();
        }
        public void Attack()
        {
            if (distanceToPlayer <= distanceToAttack)
            {
                _player.ChangeHealth(Damage);
            }
        }
        public void GetDamage(float damage)
        {
            if (Health >= 0)
            {
                Health -= damage;
            }

        }
    }
}
