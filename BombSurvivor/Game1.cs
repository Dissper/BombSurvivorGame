using System;
using System.Collections.Generic;
using BombSurvivor.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BombSurvivor
{
    public class Game1 : Game
    {
        //Made By: Dylan Valdez Garcia
                
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Random Random;
        
        public static int ScreenWidth;
        public static int ScreenHeight;

        private int _score = 0;
        private int _highScore = 0;
        private List<Sprite> _sprites;
        private Texture2D _background;
        private SpriteFont _font;
        private SpriteFont _highScoreFont;
        private float _timer;
        private bool _hasStarted = false;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Random = new Random();
            
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>("sea");
            Restart();

            // TODO: use this.Content to load your game content here
        }

        private void Restart()
        {
            _score = 0;
            var playerTexture = Content.Load<Texture2D>("girl");
            _font = Content.Load<SpriteFont>("Score");
            _highScoreFont = Content.Load<SpriteFont>("HighScore");

            _sprites = new List<Sprite>()
            {
                new Player(playerTexture)
                {
                    
                    Position = new Vector2((ScreenWidth / 2) - (playerTexture.Width / 2),
                        ScreenHeight - (playerTexture.Height/3 - 30)),
                    Input = new Input()
                    {
                        Left = Keys.A,
                        Right = Keys.D,
                    },
                    Speed = 10f,
                }
            };

            _hasStarted = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _hasStarted = true;

            _score++;

            if (!_hasStarted)
                return;

            _timer += (float) gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime, _sprites);
            }

            if (_timer > 0.25f)
            {
                _timer = 0;
                _sprites.Add(new Bomb(Content.Load<Texture2D>("bomb")));
            }

            for (int i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];
                if (sprite.IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }

                if (sprite is Player)
                {
                    var player = sprite as Player;
                    if (player.HasDied)
                    {
                        if(_score>_highScore)
                            _highScore = _score;
                        
                        Restart();
                    }
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, 800, 600), Color.White);
            if (_highScore>0)
            {
                _spriteBatch.DrawString(_highScoreFont,"High Score: " + _highScore, new Vector2(30,35), Color.White);
            }
            _spriteBatch.DrawString(_font,"Score: " + _score ,new Vector2(30,50),Color.White);
            foreach(var sprite in _sprites)
                sprite.Draw(_spriteBatch);
            
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}