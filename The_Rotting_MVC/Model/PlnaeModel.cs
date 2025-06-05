using Microsoft.Xna.Framework;
using System;

namespace The_Rotting_MVC.Model
{
    public class PlaneModel
    {
        public Vector2 Position;
        public Vector2 Target;
        public float Speed;
        public bool IsActive;
        public bool DropComplete;
        public float Rotation;
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private Vector2 _direction; // Сохраняем направление движения
        private bool _reachedDropPoint = false; 

        public PlaneModel(int screenWidth, int screenHeight)
        {
            Speed = 400f;
            IsActive = false;
            DropComplete = false;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        public bool TryStartAmmoDrop(out Vector2 dropPos)
        {
            if (IsActive)
            {
                dropPos = Vector2.Zero;
                return false; // Самолет уже активен
            }

            // Генерируем случайную целевую позицию на экране
            dropPos = new Vector2(
                Random.Shared.Next(50, _screenWidth - 50),
                Random.Shared.Next(50, _screenHeight - 50)
            );

            // Генерируем случайную стартовую позицию за пределами экрана
            int side = Random.Shared.Next(4);
            Vector2 planeStart = side switch
            {
                0 => new Vector2(-100, Random.Shared.Next(-100, _screenHeight + 100)), // слева
                1 => new Vector2(_screenWidth + 100, Random.Shared.Next(-100, _screenHeight + 100)), // справа
                2 => new Vector2(Random.Shared.Next(-100, _screenWidth + 100), -100), // сверху
                3 => new Vector2(Random.Shared.Next(-100, _screenWidth + 100), _screenHeight + 100), // снизу
            };

            Start(planeStart, dropPos);

            // Вычисляем направление движения (один раз при старте)
            _direction = dropPos - planeStart;
            _direction.Normalize();
            _reachedDropPoint = false; // Сбрасываем флаг при старте

            return true;
        }

        public void Start(Vector2 start, Vector2 target)
        {
            Position = start;
            Target = target;
            IsActive = true;
            DropComplete = false;

            Vector2 direction = target - start;
            Rotation = (float)Math.Atan2(direction.Y, direction.X) + MathF.PI / 2;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;

            if (!_reachedDropPoint)
            {
                float distanceToTarget = Vector2.Distance(Position, Target);
                if (distanceToTarget < Speed * deltaTime)
                {
                    // Достигли точки сброса
                    Position = Target;
                    _reachedDropPoint = true;
                    DropComplete = true;  //  DropComplete устанавливается в true, когда самолет достигает Target
                    // Продолжаем лететь в том же направлении
                    Position += _direction * Speed * deltaTime;
                }
                else
                {
                    Position += _direction * Speed * deltaTime;
                }
            }
            else
            {
                // Летим дальше по прямой, пока не вылетим за экран
                Position += _direction * Speed * deltaTime;

                if (Position.X < -200 || Position.X > _screenWidth + 200 ||
                    Position.Y < -200 || Position.Y > _screenHeight + 200)
                {
                    IsActive = false;
                    DropComplete = false;
                    _reachedDropPoint = false;
                }
            }
        }
    }
}