using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Rotting_MVC.Model;
using The_Rotting_MVC.View;

namespace The_Rotting_MVC
{
    class EntitySpawner
    {
        private Random _random = new Random();
        private int _screenWidth;
        private int _screenHeight;
        private int _gridSize = 64; // Размер ячейки сетки

        public List<EntityModel> EntityModels = new();
        List<EntityView> _views = new();
        SceneRenderer _sceneRednerer;

        Texture2D _entityTexture;
        public EntitySpawner(int screenWidth, int screenHeight,SceneRenderer sceneRenderer)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _sceneRednerer = sceneRenderer;
      
        }

        public void SetEntityTexture(Texture2D texture)
        {
            _entityTexture = texture;
        }

        public List<Vector2> SpawnBoxes(int numberOfBoxes)
        {
            List<Vector2> boxPositions = new List<Vector2>();
            int gridWidth = _screenWidth / _gridSize;
            int gridHeight = _screenHeight / _gridSize;

            // Создаем список доступных ячеек
            List<Point> availableCells = new List<Point>();
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    availableCells.Add(new Point(x, y));
                }
            }

            // Выбираем случайные ячейки
            for (int i = 0; i < numberOfBoxes && availableCells.Count > 0; i++)
            {
                int randomIndex = _random.Next(availableCells.Count);
                Point cell = availableCells[randomIndex];
                availableCells.RemoveAt(randomIndex);

                // Центрируем коробочку в ячейке
                float x = cell.X * _gridSize + _gridSize / 2f;
                float y = cell.Y * _gridSize + _gridSize / 2f;
                Vector2 position = new Vector2(x, y);
                EntityModels.Add(new EntityModel(position, new Vector2(_entityTexture.Width, _entityTexture.Height), true));
                _views.Add(new EntityView(EntityModels.Last(),_entityTexture));
                _sceneRednerer.Views.Add(_views.Last());

            }

            

            return boxPositions;
        }
    }
}
