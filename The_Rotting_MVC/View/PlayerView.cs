
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using The_Rotting_MVC.Model;

namespace The_Rotting_MVC.View
{
    class PlayerView:IDrawable
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private SpriteFont _font;
        private HealthBar _healthBar;
        private float _scale;

        private Vector2 _origin;
        private Color _color;

        private PlayerModel player;

        public PlayerView(Texture2D texture, SpriteFont font, HealthBar healthBar,PlayerModel player)
        {
            //_spriteBatch = spriteBatch;
            _texture = texture;
            _font = font;
            _healthBar = healthBar;
            _color = Color.White;
            _scale = 0.14f;
            this.player = player;
            _origin = new Vector2(_texture.Width/2,_texture.Height/2);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, player.Position, null, _color, player.Rotation, _origin, _scale, SpriteEffects.None, 0);
            DrawStatistic(_spriteBatch);
        }

        public void DrawStatistic(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(_font, $"{player.Ammo}/{player.MaxAmmo}", new Vector2(1170, 25), Color.Black);
            if (player.TimeSinceLastReload < player.ReloadCooldown)
            {
                _spriteBatch.DrawString(_font, $"Reload: {player.ReloadCooldown - player.TimeSinceLastReload:F1}s", new Vector2(1100, 50), Color.Blue);
            }
            _healthBar.DrawHealthBar(player.Health);
        }
    }
}
