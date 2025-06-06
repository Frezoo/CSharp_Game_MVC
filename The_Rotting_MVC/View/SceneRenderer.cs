
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace The_Rotting_MVC.View
{
   
    public class SceneRenderer
    {
        public List<View.IDrawable> Views = new List<View.IDrawable>();

        public void InitializeDrawables(params IDrawable[] drawables)
        {
            foreach (var drawable in drawables)
            {
                Views.Add(drawable);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var drawable in Views.OrderBy(d => d.Layer))
                drawable.Draw(sb);
        }
    }
}
