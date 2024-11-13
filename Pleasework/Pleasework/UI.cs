using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
// I want one function to update all of the ui elements and one function to draw all of the ui elements.
namespace UI
{
    public class Button
    {
        private Rectangle bounds;
        private Texture2D texture;
        private bool ishovering;
        private bool ispressed;
        private bool waspressed;

        public Button(Rectangle Bounds, Texture2D Texture)
        {
            bounds = Bounds;
            texture = Texture;
            ishovering = false;
            ispressed = false;
            waspressed = false;
        }

        public void Update(MouseState mouse)
        {
            ishovering = bounds.Contains(mouse.Position);

            waspressed = ispressed;
            ispressed = ishovering && mouse.LeftButton == ButtonState.Pressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, ispressed ? Color.Gray : ishovering ? Color.White : Color.DarkGray);
        }

        public bool IsPressed()
        {
            return ispressed && !waspressed;
        }
    }

    public class desktop
    {
        private GameTime gameTime;
        private SpriteBatch _spriteBatch;
        //List elements = new List<VariantType();
        public desktop(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            this.gameTime = gameTime;
            this._spriteBatch = _spriteBatch;
        }



    }
}
