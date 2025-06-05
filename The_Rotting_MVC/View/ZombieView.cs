
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;
using The_Rotting_MVC.Model;

namespace The_Rotting_MVC.View
{
    public class ZombieView:IDrawable
    {
        public float Scale = 0.372f;
        public Vector2 Origin;
        private float _frameTime = 0.03f;
        private float _timeSinceLastFrame = 0f;
        private int _frame = 0;
        private int _totalFrames = 17;

        private float fadeAlpha = 1f;
        private bool isFading = false;
        private float fadeSpeed = 3f;

        public bool IsTakingDamage = false;
        private float damageBlinkTimer = 0.05f;
        private const float damageBlinkDuration = 0.05f;
        private Color damageColor = Color.Red;

        private int frameWidth = 250;   // ширина одного кадра
        private int frameHeight = 282;  // высота одного кадра


        private ZombieModel zombieModel;
        private Texture2D _texture;


        private Rectangle[] _sourceRectangles;
        int The_Rotting_MVC.View.IDrawable.Layer => 1;

        public ZombieView(ZombieModel zombieModel,Texture2D texture)
        {
            Origin = new Vector2(frameWidth / 2f, frameHeight / 2f);
            _sourceRectangles = new Rectangle[_totalFrames];
            InitializeSourceRectangles();
            this.zombieModel = zombieModel;
            _texture = texture;
        }
        private void InitializeSourceRectangles()
        {
            // Координаты для каждого кадра
            _sourceRectangles[0] = new Rectangle(0, 0, frameWidth, frameHeight);
            _sourceRectangles[1] = new Rectangle(250, 0, frameWidth, frameHeight);
            _sourceRectangles[2] = new Rectangle(500, 0, frameWidth, frameHeight);
            _sourceRectangles[3] = new Rectangle(730, 0, frameWidth, frameHeight);
            _sourceRectangles[4] = new Rectangle(960, 0, frameWidth, frameHeight);
            _sourceRectangles[5] = new Rectangle(1165, 0, frameWidth, frameHeight);
            _sourceRectangles[6] = new Rectangle(1390, 0, frameWidth, frameHeight);
            _sourceRectangles[7] = new Rectangle(1625, 0, frameWidth, frameHeight);
            _sourceRectangles[8] = new Rectangle(1880, 0, frameWidth, frameHeight);
            _sourceRectangles[9] = new Rectangle(2100, 0, frameWidth, frameHeight);
            _sourceRectangles[10] = new Rectangle(2335, 0, frameWidth, frameHeight);
            _sourceRectangles[11] = new Rectangle(2550, 0, frameWidth, frameHeight);
            _sourceRectangles[12] = new Rectangle(2770, 0, frameWidth, frameHeight);
            _sourceRectangles[13] = new Rectangle(3010, 0, frameWidth, frameHeight);
            _sourceRectangles[14] = new Rectangle(3260, 0, frameWidth, frameHeight);
            _sourceRectangles[15] = new Rectangle(3520, 0, frameWidth, frameHeight);
            _sourceRectangles[16] = new Rectangle(3770, 0, frameWidth, frameHeight);
        }


        public void Update(float deltaTime)
        {
            
            if (zombieModel.Health <= 0 && !isFading)
                isFading = true;

            if (IsTakingDamage)
            {
                damageBlinkTimer -= deltaTime;
                if (damageBlinkTimer <= 0)
                {
                    IsTakingDamage = false;
                    damageBlinkTimer = damageBlinkDuration;
                }
            }

            if (isFading)
            {
                fadeAlpha -= fadeSpeed * deltaTime;
                fadeAlpha = MathHelper.Clamp(fadeAlpha, 0f, 1f);
                if (fadeAlpha <= 0)
                    zombieModel.IsAlive = false;
            }
            else
            {
                _timeSinceLastFrame += deltaTime;
                if (_timeSinceLastFrame >= _frameTime)
                {
                    _frame = (_frame + 1) % _totalFrames;
                    _timeSinceLastFrame = 0f;
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = IsTakingDamage ? damageColor : Color.White * fadeAlpha;
            spriteBatch.Draw(
            _texture,
            zombieModel.Position,
            _sourceRectangles[_frame],
            drawColor,
            zombieModel.Rotation,
            Origin,
            Scale,
            SpriteEffects.None,
            0.6f
            );
        }

    }
}
