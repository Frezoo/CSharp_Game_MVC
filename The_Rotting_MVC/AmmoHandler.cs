using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Rotting_MVC.Model;
using The_Rotting_MVC.View;

namespace The_Rotting_MVC
{


    public class AmmoHandler
    {
        public float TimeSinceLastSpawn = 0f;
        float _spawnCooldown = 10f;
        public List<AmmoBoxModel> AmmoBoxes = new List<AmmoBoxModel>();
        public List<AmmoBoxView> AmmoBoxViews = new List<AmmoBoxView>();
        public List<View.IDrawable> Drawables;
        private Texture2D _ammoBoxTexture;

        public AmmoHandler(List<View.IDrawable> drawables, Texture2D ammoBoxTexutre)
        {
            Drawables = drawables;
            _ammoBoxTexture = ammoBoxTexutre;
        }



        public void SpawnAmmoBox(Vector2 position,int ammoAmount)
        {
            Random random = new Random();
            //Vector2 position = new Vector2(random.Next(50, 1230), random.Next(50, 670));
            //int ammoAmount = random.Next(25, 45);
            AmmoBoxes.Add(new AmmoBoxModel(position, ammoAmount));
            AmmoBoxViews.Add(new AmmoBoxView(_ammoBoxTexture, AmmoBoxes.Last()));
            Drawables.Add(AmmoBoxViews.Last());
            TimeSinceLastSpawn = 0f;



        }

        public void CollectBox(int number)
        {
            AmmoBoxes.RemoveAt(number);
            Drawables.Remove(AmmoBoxViews[number]);
            AmmoBoxViews.RemoveAt(number);
        }

        public void SetTexture(Texture2D texture)
        {
            _ammoBoxTexture = texture;
        }

        public void UpdateTimers(float deltaTime)
        {
            TimeSinceLastSpawn += deltaTime;
        }
    }
}
