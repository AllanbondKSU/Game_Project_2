using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Game_Project_1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private CoinSprite[] coins;
        private int coinsCollected;

        private BatSprite batSprite;

        private SpriteFont bangers;

        private ExitSprite exitSprite;
        private bool exit;

        private SoundEffect coinPickup;
        private Song backgroundMusic;
       

        /// <summary>
        /// Our game in which the player must collect 10 coins to make the exit appear.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            batSprite = new BatSprite();
            exitSprite = new ExitSprite();

            //Creates initial coins.
            System.Random rand = new System.Random();
            coins = new CoinSprite[]
            {
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height))
            };
           


            base.Initialize();
        }
        /// <summary>
        /// Loads the textures for our assets
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

            // TODO: use this.Content to load your game content here
            foreach (var coin in coins) coin.LoadContent(Content);
            batSprite.LoadContent(Content);
            bangers = Content.Load<SpriteFont>("bangers");
            exitSprite.LoadContent(Content);
            coinPickup = Content.Load<SoundEffect>("Pickup_Coin14");
            backgroundMusic = Content.Load<Song>("Bio Unit - Docking");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }
        /// <summary>
        /// Updates our game assets
        /// </summary>
        /// <param name="gameTime">total elapsed time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            batSprite.Update(gameTime);

            System.Random rand = new System.Random();
            foreach (var coin in coins)
            {
                if(!coin.Collected && coin.Bounds.CollidesWith(batSprite.Bounds))
                {
                    coinsCollected++;
                    coinPickup.Play();
                    coin.Move(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height));
                }
            }
            if (coinsCollected >= 10) exit = true;
            
            if (exit && batSprite.Bounds.CollidesWith(exitSprite.Bounds))
            {
                
                Exit();
            }
            
            foreach(var coin in coins)
            {
                coin.Update(gameTime);
                if(!coin.Collected && coin.Bounds.Center.X <= 0)
                {

                        coin.Velocity.X = 100;
                        coin.Acceleration.X = 20;

                }
                if (!coin.Collected && coin.Bounds.Center.X >= GraphicsDevice.Viewport.Width)
                {
                        coin.Velocity.X = -100;
                        coin.Acceleration.X = -20;
                }

                if (!coin.Collected && coin.Bounds.Center.Y <= 0)
                {
                        coin.Velocity.Y = 100;
                        coin.Acceleration.Y = 20;
                }

                if (!coin.Collected && coin.Bounds.Center.Y >= GraphicsDevice.Viewport.Height)
                {
                        coin.Velocity.Y = -100;
                        coin.Acceleration.Y = -20;

                }

            }

            base.Update(gameTime);
        }
        /// <summary>
        /// Draws our game assets
        /// </summary>
        /// <param name="gameTime">total elapsed time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (exit)
            {
                exitSprite.Draw(gameTime,_spriteBatch);
            }
            foreach (var coin in coins) coin.Draw(gameTime, _spriteBatch);
            
            batSprite.Draw(gameTime, _spriteBatch);

            _spriteBatch.DrawString(bangers, $"Coins Collected: {coinsCollected} / 10", new Vector2(2, 2), Color.Gold);

            

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
