using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Game_Project_2.Sprites;


namespace GameArchitectureExample.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;
        private readonly Random _random = new Random();
        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private SpriteBatch _spriteBatch;
        private CoinSprite[] coins;
        private int coinsCollected;
        private BatSprite batSprite;
        private SpriteFont bangers;
        private ExitSprite exitSprite;
        private bool exit;
        private bool exitDrawn = false;
        public SoundEffect coinPickup;
        private Song backgroundMusic;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("gamefont");
            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            batSprite = new BatSprite();
            exitSprite = new ExitSprite();

            //Creates initial coins.
            System.Random rand = new System.Random();
            coins = new CoinSprite[]
            {
                new CoinSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height))
            };

            foreach (var coin in coins) coin.LoadContent(_content);
            batSprite.LoadContent(_content);
            bangers = _content.Load<SpriteFont>("bangers");
            exitSprite.LoadContent(_content);
            coinPickup = _content.Load<SoundEffect>("Pickup_Coin14");
            backgroundMusic = _content.Load<Song>("Bio Unit - Docking");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // TODO: Add your update logic here
                batSprite.Update(gameTime);

                System.Random rand = new System.Random();
                foreach (var coin in coins)
                {
                    if (!coin.Collected && coin.Bounds.CollidesWith(batSprite.Bounds))
                    {
                        coinsCollected++;
                        coinPickup.Play();
                        coin.Move(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height));
                    }
                }
                if (coinsCollected >= 10) exit = true;
                if (!exitDrawn)
                {
                    exitSprite.position = new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height);
                    exitDrawn = true;
                }
                if (exit && batSprite.Bounds.CollidesWith(exitSprite.Bounds))
                {
                    batSprite.level = 2;
                    LoadingScreen.Load(ScreenManager, true, 0, new GameplayScreen2());
                    
                    MediaPlayer.Stop();
                }

                foreach (var coin in coins)
                {
                    coin.Update(gameTime);
                    if (!coin.Collected && coin.Bounds.Center.X <= 0)
                    {

                        coin.Velocity.X = 100;
                        coin.Acceleration.X = 20;

                    }
                    if (!coin.Collected && coin.Bounds.Center.X >= ScreenManager.GraphicsDevice.Viewport.Width)
                    {
                        coin.Velocity.X = -100;
                        coin.Acceleration.X = -20;
                    }

                    if (!coin.Collected && coin.Bounds.Center.Y <= 0)
                    {
                        coin.Velocity.Y = 100;
                        coin.Acceleration.Y = 20;
                    }

                    if (!coin.Collected && coin.Bounds.Center.Y >= ScreenManager.GraphicsDevice.Viewport.Height)
                    {
                        coin.Velocity.Y = -100;
                        coin.Acceleration.Y = -20;

                    }

                }
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

              
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            if (exit)
            {
                exitSprite.Draw(gameTime, _spriteBatch);
            }
            foreach (var coin in coins) coin.Draw(gameTime, _spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            batSprite.Draw(gameTime, _spriteBatch);
            spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(bangers, $"Coins Collected: {coinsCollected} / 10", new Vector2(2, 2), Color.Gold);
            _spriteBatch.End();

            

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
