using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Rotting_MVC.Model
{
    public class EntityModel
    {
        public Vector2 Position;
        public Vector2 Size;
        public bool IsPushable;
        private const float PushActivationDistance = 55f; // Максимальное расстояние для активации отталкивания

        public EntityModel(Vector2 position, Vector2 size, bool isPushable = false)
        {
            Position = position;
            Size = size;
            IsPushable = isPushable;
        }

        public void TryPushFromPlayer(PlayerModel player, float pushSpeed, float deltaTime)
        {
            if (!IsPushable) return;

            // 1. Вычисляем расстояние от игрока до коробки
            float distanceToBox = Vector2.Distance(Position , player.Position);

            // 2. Проверяем, находится ли игрок достаточно близко к коробке
            if (distanceToBox <= PushActivationDistance)
            {
                // 3. Если достаточно близко, толкаем коробку в направлении движения игрока
                if (player.Direction != Vector2.Zero)
                {
                    Vector2 pushDirection = player.Direction; // Направление движения игрока
                    pushDirection.Normalize(); // Нормализуем, чтобы скорость была постоянной
                    Push(pushDirection, pushSpeed, deltaTime);
                }
            }
        }

        public void Push(Vector2 direction, float speed, float deltaTime)
        {
            if (IsPushable)
            {
                Position += direction * speed * deltaTime;
            }
        }
    }
}