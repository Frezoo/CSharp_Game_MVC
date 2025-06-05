using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Rotting_MVC.Model;

namespace The_Rotting_MVC.View
{
    public class AmmoBoxView:IDrawable
    {
        
        Texture2D _texture;
        AmmoBoxModel _boxModel;
        float _scale;

        public AmmoBoxView( Texture2D texture,AmmoBoxModel boxModel)
        {
            
            _texture = texture;
            _scale = 0.05f;
            _boxModel = boxModel;
        }

        public void Draw(SpriteBatch _sb)
        {
            _sb.Draw(_texture, _boxModel.Position, null, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0f);

        }
    }
}
