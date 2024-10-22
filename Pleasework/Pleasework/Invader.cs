using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Invader
{
    public Texture2D Texture;
    public Vector2 Scale;
    public Color Color;
    public Vector2 Position;
    public float OrbitRadius;
    public byte numberofpointsgiven;
    public float angle;
    public float anglularvelocity;
    public float SightRadius;
    public Rectangle rectangle;

    public Invader()
    {
        
    }
}
public class Bullet
{
    public Vector2 position;
    public float angle;
    public Vector2 momentum;
    
    public Bullet()
    {

    }
}
