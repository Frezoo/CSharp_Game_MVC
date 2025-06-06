
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

namespace The_Rotting_MVC
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private PlayerModel _playerModel;
        private AmmoHandler _ammoHandler;
        private ZombieSpawner _zombieSpanwer;
        private EntitySpawner _entitySpawner;
        private PlaneModel _planeModel;
        private List<EntityModel> _boxes = new List<EntityModel>();

        private BulletController _bulletController;
        private InputHandler_Controller_ _inputHandler;

        private PlayerView _playerView;
        private BulletView _bulletView;
        private PlaneView _planeView;
        private List<EntityView> _boxViews = new List<EntityView>();


        private Texture2D _crosshair;
        private Texture2D _background;
        private Texture2D _boxTexture;
        private SpriteFont _font;

        private GameState _currentGameState = GameState.MainMenu;

        public SceneRenderer SceneRenderer = new SceneRenderer();

        private MainMenu _mainMenu;


        private float deltaTime;
        const int ZombieWidth = 246;
        const int ZombieHeight = 282;
        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 720;
        private const float AmmoDropInterval = 10f;
        private const int AmmoMin = 25;
        private const int AmmoMax = 45;
        private const float CrosshairScale = 0.03f;
        private const int MenuOffsetX = 100;
        private const int MenuOffsetY = 50;


        private Vector2 _pendingAmmoDropPosition;
        private bool _pendingAmmoDrop = false;
        int boxCount = 0;
        enum GameState
        {
            MainMenu,
            Playing
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            SetResolution(ScreenWidth, ScreenHeight);

            InitializeModels();
            InizializeInput();

            base.Initialize();
        }




        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteFont menuFont = Content.Load<SpriteFont>("font");

            Texture2D zombieWalkTexture, menuBackground, playerTexture, bulleteTexture, ammoTexture, planeTexture;
            InitializeTextures(out zombieWalkTexture, out menuBackground, out playerTexture, out bulleteTexture, out ammoTexture, out planeTexture);
            _ammoHandler.SetTexture(ammoTexture);
            SpriteFont font = Content.Load<SpriteFont>("font");
            _font = font;
            HealthBar healthBar = new HealthBar(new Vector2(161.5f, 39), _spriteBatch, Content.Load<Texture2D>("HP_FRONT"), Content.Load<Texture2D>("HP_BG"));

            InitializeViews(playerTexture, bulleteTexture, ammoTexture, planeTexture, font, healthBar);
          
            _zombieSpanwer.SetZombieTexture(zombieWalkTexture);
            _entitySpawner.SetEntityTexture(_boxTexture);

            _bulletController = new BulletController(_playerModel.Bullets, _zombieSpanwer.Zombies, _bulletView, _zombieSpanwer.ZombiesViews);

            SceneRenderer.InitializeDrawables(_playerView, _planeView,_bulletView);
            InitializeMenu(menuFont, menuBackground);
        }




        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (_currentGameState == GameState.Playing)
                {
                    _currentGameState = GameState.MainMenu;
                    _mainMenu.Reset();
                    IsMouseVisible = false;

                }
                else
                {
                    Exit();
                }
            }

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_playerModel.Health <= 0)
                _currentGameState = GameState.MainMenu;

            
            if (boxCount == 0)
            {
                boxCount++;

                _entitySpawner.SpawnBoxes(25);

            }

            switch (_currentGameState)
            {
                case GameState.MainMenu:
                    _mainMenu.Update();
                    SetDeaflutParameters();
                    if (_mainMenu.StartGameRequested)
                    {
                        _currentGameState = GameState.Playing;
                        _mainMenu.Reset();
                        IsMouseVisible = false;
                    }
                    else if (_mainMenu.ExitRequested)
                    {
                        Exit();
                    }
                    break;

                case GameState.Playing:
                    _playerModel.MovePlayer((float)gameTime.ElapsedGameTime.TotalSeconds);
                    _playerModel.UpdateRotation(Mouse.GetState().Position.ToVector2());
                    _playerModel.HandleAmmoCollection(_ammoHandler);

                    _playerModel.UpdateTimers(deltaTime);
                    _ammoHandler.UpdateTimers(deltaTime);
                    _zombieSpanwer.Update();
                    _planeModel.Update(deltaTime);
                    _inputHandler.Update(gameTime, Matrix.Identity);

                    UpdateBullets();
                    UpdateZombies();
                    foreach(var box in _entitySpawner.EntityModels)
                    {
                        box.TryPushFromPlayer(_playerModel,650,deltaTime);
                    }

                    _bulletController.ProcessBullets(deltaTime, ZombieWidth, ZombieHeight);

                    ProcessingAmmoDrop();




                    break;
            }

            base.Update(gameTime);
        }





        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (_currentGameState)
            {
                case GameState.MainMenu:
                    _mainMenu.Draw(_spriteBatch);
                    break;

                case GameState.Playing:
                    _spriteBatch.Draw(_background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
                    _spriteBatch.Draw(_crosshair, _inputHandler.GetMouseWorldPosition(Matrix.Identity), null, Color.White, 0, new Vector2(_crosshair.Width / 2, _crosshair.Height / 2), CrosshairScale, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(_font, $"Wave: {_zombieSpanwer.Wave}", new Vector2(ScreenWidth / 2 - 30, 25), Color.Red);

                    SceneRenderer.Draw(_spriteBatch);
                 

                    break;
            }

            _spriteBatch.End(); 

            base.Draw(gameTime);
        }

        private void ProcessingAmmoDrop()
        {
            if (!_pendingAmmoDrop)
            {
                if (_ammoHandler.TimeSinceLastSpawn >= AmmoDropInterval)
                {
                    if (_planeModel.TryStartAmmoDrop(out Vector2 dropPos))
                    {
                        _pendingAmmoDropPosition = dropPos;
                        _pendingAmmoDrop = true;
                        _ammoHandler.TimeSinceLastSpawn = 0f;
                    }
                }
            }

            if (_pendingAmmoDrop && _planeModel.DropComplete)
            {
                int ammoAmount = Random.Shared.Next(AmmoMin, AmmoMax);
                _ammoHandler.SpawnAmmoBox(_pendingAmmoDropPosition,ammoAmount);
                _pendingAmmoDrop = false;
            }
        }

        private void UpdateBullets()
        {
            foreach (var bullet in _playerModel.Bullets)
            {
                bullet.UpdateBulletPosition(deltaTime);
            }
        }
        private void UpdateZombies()
        {
            for (int i = 0; i < _zombieSpanwer.Zombies.Count; i++)
            {
                _zombieSpanwer.Zombies[i].MoveToPlayer(deltaTime, _zombieSpanwer.Zombies);
                _zombieSpanwer.Zombies[i].Attack();
                _zombieSpanwer.ZombiesViews[i].Update(deltaTime);

            }
        }
        public void SetResolution(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        public bool CheckCollision(Rectangle fristRectangle, Rectangle secondRectangle)
        {
            return fristRectangle.Intersects(secondRectangle);
        }
        private void InitializeMenu(SpriteFont menuFont, Texture2D menuBackground)
        {
            Vector2 menuPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - MenuOffsetX, _graphics.PreferredBackBufferHeight / 2 - MenuOffsetY);
            _mainMenu = new MainMenu(menuFont, menuPosition, menuBackground);
        }

        private void InitializeViews(Texture2D playerTexture, Texture2D bulleteTexture, Texture2D ammoTexture, Texture2D planeTexture, SpriteFont font, HealthBar healthBar)
        {
            _playerView = new PlayerView(playerTexture, font, healthBar, _playerModel);
            _bulletView = new BulletView(_spriteBatch, bulleteTexture,_playerModel.Bullets);
            _planeView = new PlaneView(planeTexture, _planeModel);
        }

        private void InitializeTextures(out Texture2D zombieWalkTexture, out Texture2D menuBackground, out Texture2D playerTexture, out Texture2D bulleteTexture, out Texture2D ammoTexture, out Texture2D planeTexture)
        {
            zombieWalkTexture = Content.Load<Texture2D>("root");
            menuBackground = Content.Load<Texture2D>("menuBackground");
            playerTexture = Content.Load<Texture2D>("Solider_AK");
            bulleteTexture = Content.Load<Texture2D>("bullet");
            ammoTexture = Content.Load<Texture2D>("ammo_box_texture");
            planeTexture = Content.Load<Texture2D>("plane");
            _background = Content.Load<Texture2D>("background");
            _crosshair = Content.Load<Texture2D>("crosshair");
            _boxTexture = Content.Load<Texture2D>("entityBox");
        }

        private void InitializeModels()
        {
            _playerModel = new PlayerModel();
            _ammoHandler = new AmmoHandler(SceneRenderer.Views,null);
            _zombieSpanwer = new ZombieSpawner(_playerModel, SceneRenderer.Views);
            _entitySpawner = new EntitySpawner(ScreenWidth, ScreenHeight, SceneRenderer);
            _planeModel = new PlaneModel(ScreenWidth, ScreenHeight);
        }


        private void InizializeInput()
        {
            _inputHandler = new InputHandler_Controller_();
            _inputHandler.OnMoveUp += (sender, args) => _playerModel.Direction.Y -= 1;
            _inputHandler.OnMoveDown += (sender, args) => _playerModel.Direction.Y += 1;
            _inputHandler.OnMoveLeft += (sender, args) => _playerModel.Direction.X -= 1;
            _inputHandler.OnMoveRight += (sender, args) => _playerModel.Direction.X += 1;
            _inputHandler.OnReload += (sender, args) => _playerModel.Reload();
            _inputHandler.OnShoot += (sender, args) => _playerModel.Shoot();
            _inputHandler.OnMousePositionChanged += (sender, worldPosition) => _playerModel.UpdateRotation(worldPosition);
        }

        private void SetDeaflutParameters()
        {
            _playerModel.Health = 1f;
            _playerModel.Ammo = 30;
            _playerModel.MaxAmmo = 90;
            _playerModel.Position = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
            _playerModel.Direction = Vector2.Zero;
            _playerModel.Rotation = 0f;
            _playerModel.Bullets.Clear();
            _ammoHandler.AmmoBoxes.Clear();
            _zombieSpanwer.Wave = 0;
            _zombieSpanwer.Zombies.Clear();
            _zombieSpanwer.ZombiesViews.Clear();
            _planeModel.IsActive = false;
            _planeModel.DropComplete = false;

        }

    }
}
