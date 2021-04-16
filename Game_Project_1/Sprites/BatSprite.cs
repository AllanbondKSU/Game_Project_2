using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Game_Project_2.Collisions;

namespace Game_Project_2.Sprites
{
    public enum Direction
    {
        Down = 0,
        Right = 1,
        Up = 2,
        Left = 3
    }
    public class BatSprite : IParticleEmitter
    {
        private double animationTimer;
        private short animationFrame = 1;
        private KeyboardState keyboardState;
        private Texture2D texture;
        private Vector2 velocity;

        public int level = 1;

        private Direction direction;
        public Vector2 position = new Vector2(200,200);
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 32, 200 - 32), 32, 32);

        public BoundingRectangle Bounds => bounds;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("32x32-bat-sprite");
        }
        /// <summary>
        /// Updates our Bat Sprite
        /// </summary>
        /// <param name="gameTime">total elapsed time</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();


            // Apply keyboard movement and track direction
            if (level == 1)
            {
                Vector2 batPostion = new Vector2(0, 0);
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    position += new Vector2(0, -1);
                    direction = Direction.Up;
                }
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    position += new Vector2(0, 1);
                    direction = Direction.Down;
                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    position += new Vector2(-1, 0);
                    direction = Direction.Left;
                    batPostion = new Vector2(position.X + 12, position.Y + 13);
                }
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    position += new Vector2(1, 0);
                    direction = Direction.Right;
                    batPostion = new Vector2(position.X + 24, position.Y + 13);
                }
                Velocity = batPostion - Position;
                Position = batPostion;
            }
            else
            {
                
                
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) position -= Vector2.UnitX * 100*t;
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) position += Vector2.UnitX * 100*t;
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) position -= Vector2.UnitY * 60*t;
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += Vector2.UnitY * 120*t;
                /*
                direction = Direction.Right;
                KeyboardState keyboardState = Keyboard.GetState();

                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 acceleration = new Vector2(0, 50);
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    acceleration += new Vector2(0, -100);
                }

                velocity += acceleration * t;
                position += velocity * t;
                */
            }
            bounds.Y = position.Y;
            bounds.X = position.X;
        }
        /// <summary>
        /// Draws the Bat Sprite
        /// </summary>
        /// <param name="gameTime">total elapsed time</param>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > 0.3)
            {
                animationFrame++;
                if(animationFrame > 3)
                {
                    animationFrame = 1;
                }
                animationTimer -= 0.3;
            }

            var sourceRectangle = new Rectangle(animationFrame*32,(int)direction*32,32,32);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }

}
