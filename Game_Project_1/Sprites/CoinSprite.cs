using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Game_Project_1.Collisions;

namespace Game_Project_2.Sprites
{
    public class CoinSprite
    {
        private const float ANIMATION_SPEED = 0.1f;

        private double animationTimer;

        private int animationFrame;

        private Vector2 position;

        private Texture2D texture;

        private BoundingCircle bounds;

        public Vector2 Velocity;
        public Vector2 Acceleration;

        public BoundingCircle Bounds => bounds;


        

        public bool Collected { get; set; }
        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public CoinSprite(Vector2 position)
        {
            this.position = position;
            this.bounds = new BoundingCircle(position + new Vector2(8, 8), 8);
            this.Velocity = new Vector2(50, 50);
            this.Acceleration = new Vector2(20, 20);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("coins");
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Collected) return;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }
            var source = new Rectangle(animationFrame * 16, 0, 16, 16);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, source, Color.White);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity += Acceleration * t;
            position += Velocity * t;
            this.bounds = new BoundingCircle(position + new Vector2(8, 8), 8);
        }

        /// <summary>
        /// Moves the coin to a new location
        /// </summary>
        /// <param name="newPosition">the random new location for the coin</param>
        public void Move(Vector2 newPosition)
        {
            this.position = newPosition;
            this.bounds = new BoundingCircle(newPosition + new Vector2(8, 8), 8);
        }


    }
}

