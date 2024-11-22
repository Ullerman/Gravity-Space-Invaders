using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pleasework
{
    public class PrimitiveBatch
    {
        Texture2D WhitePixel;
        Texture2D WhiteCircle;

        public void Primitive(Texture2D whitePixel, Texture2D whiteCircle)
        {
            this.WhitePixel = whitePixel;
            this.WhiteCircle = whiteCircle;
        }

        public class Line
        {
            public Vector2 Start;
            public Vector2 End;
            public Color Color;
            public float Width;

            public Line(Vector2 start, Vector2 end, Color color, float width)
            {
                Start = start;
                End = end;
                Color = color;
                Width = width;
            }

            public Line(Vector2 start, float angle, float distance, Color color, float width)
            {
                Start = start;
                End = new Vector2(
                    (float)Math.Cos(angle) * distance,
                    (float)Math.Sin(angle) * distance
                );
                Color = color;
                Width = width;
            }

            public void Draw(
                SpriteBatch spriteBatch,
                PrimitiveBatch primitiveBatch,
                Vector2 CameraOffset
            )
            {
                Vector2 edge = End - Start;
                float angle = (float)Math.Atan2(edge.Y, edge.X);
                spriteBatch.Draw(
                    primitiveBatch.WhitePixel,
                    new Microsoft.Xna.Framework.Rectangle(
                        (int)(Start.X + CameraOffset.X),
                        (int)(Start.Y + CameraOffset.Y),
                        (int)edge.Length(),
                        (int)Width
                    ),
                    null,
                    Color,
                    angle,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0
                );
            }
        }

        public class Circle
        {
            public Vector2 Position;
            public float Radius;
            public Color Color;

            public Circle(Vector2 position, float radius, Color color)
            {
                Position = position;
                Radius = radius;
                Color = color;
            }

            public void Draw(
                SpriteBatch spriteBatch,
                PrimitiveBatch primitiveBatch,
                Vector2 CameraOffset
            )
            {
                spriteBatch.Draw(
                    primitiveBatch.WhiteCircle,
                    Position + CameraOffset,
                    null,
                    Color,
                    0,
                    new Vector2(
                        primitiveBatch.WhiteCircle.Width / 2,
                        primitiveBatch.WhiteCircle.Height / 2
                    ),
                    new Vector2(
                        Radius / primitiveBatch.WhiteCircle.Width,
                        Radius / primitiveBatch.WhiteCircle.Height
                    ),
                    SpriteEffects.None,
                    0
                );
            }
        }

        public class Rectangle
        {
            public Vector2 Position;
            public Vector2 Size;
            public Color Color;

            public Rectangle(Vector2 position, Vector2 size, Color color)
            {
                Position = position;
                Size = size;
                Color = color;
            }

            public void Draw(
                SpriteBatch spriteBatch,
                PrimitiveBatch primitiveBatch,
                Vector2 CameraOffset
            )
            {
                spriteBatch.Draw(
                    primitiveBatch.WhitePixel,
                    new Microsoft.Xna.Framework.Rectangle(
                        (int)(Position.X + CameraOffset.X),
                        (int)(Position.Y + CameraOffset.Y),
                        (int)Size.X,
                        (int)Size.Y
                    ),
                    Color
                );
            }
        }

        public class Pixel
        {
            public Vector2 Position;
            public Color Color;

            public Pixel(Vector2 position, Color color)
            {
                Position = position;
                Color = color;
            }

            public void Draw(
                SpriteBatch spriteBatch,
                PrimitiveBatch primitiveBatch,
                Vector2 CameraOffset
            )
            {
                spriteBatch.Draw(primitiveBatch.WhitePixel, Position + CameraOffset, Color);
            }
        }
    }
}
