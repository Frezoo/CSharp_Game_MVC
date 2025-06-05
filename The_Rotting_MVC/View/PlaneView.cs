using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using The_Rotting_MVC.Model;

namespace The_Rotting_MVC.View
{
    public class PlaneView:IDrawable
    {
        private Texture2D _texture;
        private PlaneModel _model;

        int The_Rotting_MVC.View.IDrawable.Layer => 9;
        public PlaneView(Texture2D texture, PlaneModel model)
        {
            _texture = texture;
            _model = model;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_model.IsActive)
                spriteBatch.Draw(_texture, _model.Position,null ,Color.White,_model.Rotation,new Vector2(_texture.Width/2,_texture.Height/2),0.2f,SpriteEffects.None,1);
        }
        
    }
}


