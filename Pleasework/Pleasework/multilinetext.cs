using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pleasework
{
    public class TextBox
    {
        string text;
        SpriteFont font;
        int width;
        public TextBox()
        {

        }
        public void Draw(SpriteBatch spriteBatch,string text, SpriteFont font, int width, Vector2 position, Color color)
        {
            this.text = text;
            this.font = font;
            this.width = width;
            string[] words = text.Split(' ');
            string line = string.Empty;
            Vector2 size = Vector2.Zero;
            foreach (string word in words)
            {
                if (word.Contains("\n"))
                {
                    string[] splitWords = word.Split('\n');
                    foreach (string splitWord in splitWords)
                    {
                        Vector2 tempSize = font.MeasureString(line + splitWord);
                        if (size.X + tempSize.X < width)
                        {
                            line += splitWord + " ";
                            size = font.MeasureString(line);
                        }
                        else
                        {
                            spriteBatch.DrawString(font, line, position, color);
                            position.Y += font.LineSpacing;
                            line = splitWord + " ";
                            size = font.MeasureString(line);
                        }
                        spriteBatch.DrawString(font, line, position, color);
                        position.Y += font.LineSpacing;
                        line = string.Empty;
                        size = Vector2.Zero;
                    }
                }
                else
                {
                    Vector2 tempSize = font.MeasureString(line + word);
                    if (size.X + tempSize.X < width)
                    {
                        line += word + " ";
                        size = font.MeasureString(line);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, line, position, color);
                        position.Y += font.LineSpacing;
                        line = word + " ";
                        size = font.MeasureString(line);
                    }
                }
            }
            spriteBatch.DrawString(font, line, position, color);
        }
    }
}