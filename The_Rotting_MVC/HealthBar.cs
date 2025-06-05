using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Rotting_MVC
{
    class HealthBar
    {
        public Vector2 Position;
        public Vector2 Origin;

        private SpriteBatch _sb;

        public Texture2D FrontTexture;
        public Texture2D BackTexture;

        private float _maxHealth = 1f;
        private float _hpCondition = 1f;

        public HealthBar(Vector2 position, SpriteBatch sb, Texture2D frontTexture, Texture2D backTexture)
        {
            Position = position;
            _sb = sb;
            FrontTexture = frontTexture;
            BackTexture = backTexture;
            Origin = new Vector2(BackTexture.Width / 2, BackTexture.Height / 2);
        }

        public void DrawHealthBar(float currentHealth)
        {
            _hpCondition = currentHealth / _maxHealth;
            _sb.Draw(BackTexture, Position, null, Color.White, 0, Origin, 1f, SpriteEffects.None, 1f);
            _sb.Draw(FrontTexture, Position, new Rectangle(0, 0, (int)(303 * _hpCondition), 49), Color.White, 0, Origin, 1f, SpriteEffects.None, 1f);
        }
    }
}
