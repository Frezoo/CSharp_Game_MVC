using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using The_Rotting_MVC.Model;
using The_Rotting_MVC.View;

namespace The_Rotting_MVC
{
    public class ZombieSpawner
    {
        private Random rnd = new Random();
        private PlayerModel _player;
        public int Wave = 1;
        private bool waveStarted = false;

        public List<ZombieModel> Zombies = new List<ZombieModel>();
        public List<ZombieView> ZombiesViews = new List<ZombieView>();
        public List<View.IDrawable> Drawables;

        public Texture2D ordinaryZombie;
        int enemyCount => Zombies.Count();

        public ZombieSpawner(PlayerModel player,List<View.IDrawable> drawables)
        {
            _player = player;
            Drawables = drawables;

        }

        public void Update()
        {
            if (!waveStarted)
            {
                StartWave();
                waveStarted = true;
            }

            for (int i = 0; i < Zombies.Count; i++)
            {
                if (Zombies[i].IsAlive == false)
                {
                    Zombies.RemoveAt(i);
                    Drawables.Remove(ZombiesViews[i]);
                    ZombiesViews.RemoveAt(i);
                }
            }


            if (enemyCount == 0 && waveStarted)
            {
                Wave++;
                waveStarted = false;
            }
        }

        private void StartWave()
        {
            int numberOfEnemies = CalculateEnemiesForWave(Wave);
            SpawnEnemies(numberOfEnemies);
        }

        private int CalculateEnemiesForWave(int waveNumber)
        {
            int baseEnemies = 5;
            float increasePercentage = 0.2f;

            return (int)(baseEnemies * Math.Pow(1 + increasePercentage, waveNumber - 1));
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var zombie = new ZombieModel(GetSpawnPosition(), _player);
                Zombies.Add(zombie);
                ZombiesViews.Add(new ZombieView(zombie,ordinaryZombie));
                Drawables.Add(ZombiesViews.Last());

            }
        }

        private Vector2 GetSpawnPosition()
        {
            return new Vector2(rnd.Next(50, 1230), rnd.Next(50, 670));
        }

        public void SetZombieTexture(Texture2D ordinaryZombie)
        {
            this.ordinaryZombie = ordinaryZombie;
        }

    }
}