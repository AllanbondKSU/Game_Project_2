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
    public enum Direction
    {
        Down = 0,
        Right = 1,
        Up = 2,
        Left = 3
    }
    public class BatSprite
    {
        private double animationTimer;
        private short animationFrame = 1;
        private KeyboardState keyboardState;
        private Texture2D texture;

        private Direction direction;
        private Vector2 position = new Vector2(200, 200);
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 32, 200 - 32), 32, 32);

        public BoundingRectangle Bounds => bounds;

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
               
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(1, 0);
                direction = Direction.Right;
                
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
