
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using The_Rotting_MVC.Controller;
using The_Rotting_MVC.Model;
using The_Rotting_MVC.View;

public class BulletView : The_Rotting_MVC.View.IDrawable
{
    public Texture2D _texture;
    private SpriteBatch _sb;
    private float _scale;
    private List<BulletModel> _bullets;

    int The_Rotting_MVC.View.IDrawable.Layer => 7;

    public BulletView(SpriteBatch spriteBatch, Texture2D texture, List<BulletModel> bullets)
    {
        _sb = spriteBatch;
        _texture = texture;
        _bullets = bullets;
        _scale = 1f; 
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in _bullets)
        {
            DrawBullet(bullet);
        }
    }

    public void DrawBullet(BulletModel bulletModel)
    {
        // Твоя логика отрисовки одной пули
        _sb.Draw(_texture, bulletModel.Position, null, Color.White, bulletModel.Rotation, Vector2.Zero, _scale, SpriteEffects.None, 0.9f);
    }
}