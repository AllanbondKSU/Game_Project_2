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
    public class ExitSprite
    {
        public Vector2 position;
        private BoundingRectangle bounds;
        private Texture2D texture;
        public Color ExitColor = Color.Red;
        public BoundingRectangle Bounds => bounds;
        /// <summary>
        /// Loads the Exit texture
        /// </summary>
        /// <param name="content">the atlas</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("colored_packed");
        }
        /// <summary>
        /// Draws the Exit
        /// </summary>
        /// <param name="gameTime">total elapsed time</param>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            bounds = new BoundingRectangle(new Vector2(position.X, position.Y), 8, 8);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, new Rectangle(48, 96, 16, 16), ExitColor);
            spriteBatch.End();
        }
    }
}
