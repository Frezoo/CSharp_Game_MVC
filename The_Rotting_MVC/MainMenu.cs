
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Rotting_MVC.View
{
    public class MainMenu
    {
        private SpriteFont _font;
        private string[] _menuItems = { "Start Game", "Quit" };
        private Texture2D _menuBackground;
        private int _selectedIndex = 0;
        private Vector2 _position;
        private Color _selectedColor = Color.Yellow;
        private Color _normalColor = Color.White;
        private KeyboardState _previousKeyboardState;
        public bool StartGameRequested { get; private set; }
        public bool ExitRequested { get; private set; }

        public MainMenu(SpriteFont font, Vector2 position, Texture2D menuBackground)
        {
            _menuBackground = menuBackground;
            _font = font;
            _position = position;
        }

        public void Update()
        {
            KeyboardState state = Keyboard.GetState();

            // Используем флаги для предотвращения повторных нажатий (lag-free)
            bool upPressed = state.IsKeyDown(Keys.Up);
            bool downPressed = state.IsKeyDown(Keys.Down);
            bool enterPressed = state.IsKeyDown(Keys.Enter);

            // Обработка нажатия клавиш со временем
            if (upPressed && _previousKeyboardState.IsKeyUp(Keys.Up)) // Только если клавиша нажата в данный момент и не была нажата в предыдущем кадре
            {
                _selectedIndex = (_selectedIndex - 1 + _menuItems.Length) % _menuItems.Length;
            }
            if (downPressed && _previousKeyboardState.IsKeyUp(Keys.Down))
            {
                _selectedIndex = (_selectedIndex + 1) % _menuItems.Length;
            }

            if (enterPressed && _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                if (_selectedIndex == 0)
                    StartGameRequested = true;
                else if (_selectedIndex == 1)
                    ExitRequested = true;
            }

            // Сохраняем текущее состояние клавиш для следующего обновления
            _previousKeyboardState = state;
        }

        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuBackground,Vector2.Zero,Color.White);
            for (int i = 0; i < _menuItems.Length; i++)
            {
                Color color = (i == _selectedIndex) ? _selectedColor : _normalColor;
                Vector2 itemPosition = _position + new Vector2(0, i * 40);
                spriteBatch.DrawString(_font, _menuItems[i], itemPosition, color);
            }
        }

        public void Reset()
        {
            StartGameRequested = false;
            ExitRequested = false;
        }
    }
}
