using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using The_Rotting_MVC.Controller;
using The_Rotting_MVC.Model;
using The_Rotting_MVC.View;


namespace The_Rotting_MVC.View
{
    class EntityView:IDrawable
    {
        private EntityModel _entity;
        private Texture2D _texture;
        private const float _size = 0.07f;
        private Vector2 _origin;

        public EntityView(EntityModel entity, Texture2D texture)
        {                                   
            _entity = entity;
            _texture = texture;
            _origin = new Vector2(_texture.Width/2,_texture.Height/2); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _entity.Position, null, Color.White, 0, _origin, _size, SpriteEffects.None, 0);
        }
    }
}
