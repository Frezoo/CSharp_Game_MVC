using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace The_Rotting_MVC.Controller
{
    class InputHandler_Controller_
    {
        public event EventHandler OnMoveUp;
        public event EventHandler OnMoveDown;
        public event EventHandler OnMoveLeft;
        public event EventHandler OnMoveRight;
        public event EventHandler OnReload;
        public event EventHandler OnShoot;
        public event EventHandler<Vector2> OnMousePositionChanged;


        public void Update(GameTime gameTime, Matrix _transformMatrix)
        {
            HandleKeyboardInput(Keyboard.GetState());
            HandleMouseInput(Mouse.GetState(), _transformMatrix);
        }

        private void HandleKeyboardInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                OnMoveUp?.Invoke(this, EventArgs.Empty);
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                OnMoveDown?.Invoke(this, EventArgs.Empty);
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                OnMoveLeft?.Invoke(this, EventArgs.Empty);
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                OnMoveRight?.Invoke(this, EventArgs.Empty);
            }
            if (keyboardState.IsKeyDown(Keys.R))
            {
                OnReload?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandleMouseInput(MouseState mouseState, Matrix _transformMatrix)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                OnShoot?.Invoke(this, EventArgs.Empty);
            }

            Vector2 worldPosition = GetMouseWorldPosition(_transformMatrix);
            OnMousePositionChanged?.Invoke(this, worldPosition);
        }

        public Vector2 GetMouseWorldPosition(Matrix _transformMatrix)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePosition = mouseState.Position;
            Vector2 worldPosition = Vector2.Transform(mousePosition.ToVector2(), Matrix.Invert(_transformMatrix));
            return worldPosition;
        }
    }
}