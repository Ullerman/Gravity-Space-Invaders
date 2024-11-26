using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class RotRectangle
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public float Rotation { get; set; }
    public Texture2D Texture { get; set; }

    public RotRectangle(Texture2D texture, Vector2 position, Vector2 size, float rotation = 0f)
    {
        Texture = texture;
        Position = position;
        Size = size;
        Rotation = rotation;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            Position,
            null,
            Color.White,
            Rotation,
            new Vector2(Texture.Width / 2, Texture.Height / 2),
            new Vector2(Size.X / Texture.Width, Size.Y / Texture.Height),
            SpriteEffects.None,
            0f
        );
    }

    public bool Intersects(RotRectangle other)
    {
        // Get the corners of both rectangles
        Vector2[] corners1 = GetCorners();
        Vector2[] corners2 = other.GetCorners();

        // Check for separation on all axes
        Vector2[] axes = GetAxes(corners1, corners2);
        foreach (var axis in axes)
        {
            if (!IsOverlapping(axis, corners1, corners2))
            {
                return false; // Separation found, no collision
            }
        }

        return true; // No separation found, collision detected
    }
    // public bool Intersects(Circle circle)
    // {
    //     // Get the corners of the rectangle
    //     Vector2[] corners = GetCorners();

    //     // Check for separation on all axes
    //     Vector2[] axes = GetAxes(corners, circle);
    //     foreach (var axis in axes)
    //     {
    //         if (!IsOverlapping(axis, corners, circle))
    //         {
    //             return false; // Separation found, no collision
    //         }
    //     }

    //     return true; // No separation found, collision detected
    // }
    public bool Intersects(Vector2 point)
    {
        // Get the corners of the rectangle
        Vector2[] corners = GetCorners();

        // Check for separation on all axes
        Vector2[] axes = GetAxes(corners, new Vector2[] { point });
        foreach (var axis in axes)
        {
            if (!IsOverlapping(axis, corners,  new Vector2[] { point }))
            {
                return false; // Separation found, no collision
            }
        }

        return true; // No separation found, collision detected
    }
    public bool Intersects(Rectangle rectangle){
        Vector2[] corners = GetCorners();
        Vector2[] axes = GetAxes(corners, new Vector2[] { new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), new Vector2(rectangle.X, rectangle.Y + rectangle.Height) });
        foreach (var axis in axes)
        {
            if (!IsOverlapping(axis, corners, new Vector2[] { new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), new Vector2(rectangle.X, rectangle.Y + rectangle.Height) }))
            {
                return false; // Separation found, no collision
            }
        }

        return true; // No separation found, collision detected
    }

    public Vector2[] GetCorners()
    {
        Vector2[] corners = new Vector2[4];
        Vector2 halfSize = Size / 2;
        Matrix rotationMatrix = Matrix.CreateRotationZ(Rotation);

        corners[0] = Position + Vector2.Transform(new Vector2(-halfSize.X, -halfSize.Y), rotationMatrix);
        corners[1] = Position + Vector2.Transform(new Vector2(halfSize.X, -halfSize.Y), rotationMatrix);
        corners[2] = Position + Vector2.Transform(new Vector2(halfSize.X, halfSize.Y), rotationMatrix);
        corners[3] = Position + Vector2.Transform(new Vector2(-halfSize.X, halfSize.Y), rotationMatrix);

        return corners;
    }

    private Vector2[] GetAxes(Vector2[] corners1, Vector2[] corners2)
    {
        Vector2[] axes = new Vector2[4];
        axes[0] = corners1[1] - corners1[0];
        axes[1] = corners1[3] - corners1[0];
        axes[2] = corners2[1] - corners2[0];
        axes[3] = corners2[3] - corners2[0];

        for (int i = 0; i < axes.Length; i++)
        {
            axes[i] = new Vector2(-axes[i].Y, axes[i].X); // Perpendicular vector
            axes[i].Normalize();
        }

        return axes;
    }

    private bool IsOverlapping(Vector2 axis, Vector2[] corners1, Vector2[] corners2)
    {
        float min1, max1, min2, max2;
        ProjectOntoAxis(axis, corners1, out min1, out max1);
        ProjectOntoAxis(axis, corners2, out min2, out max2);

        return !(min1 > max2 || min2 > max1);
    }

    private void ProjectOntoAxis(Vector2 axis, Vector2[] corners, out float min, out float max)
    {
        min = Vector2.Dot(axis, corners[0]);
        max = min;

        for (int i = 1; i < corners.Length; i++)
        {
            float projection = Vector2.Dot(axis, corners[i]);
            if (projection < min)
            {
                min = projection;
            }
            if (projection > max)
            {
                max = projection;
            }
        }
    }
}