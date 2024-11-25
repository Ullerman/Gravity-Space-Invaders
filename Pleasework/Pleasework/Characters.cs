using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pleasework;

public class Rocket{
    public Texture2D Texture;
    public Vector2 Scale;
    public Color Color;
    public Vector2 Position;
    public float Angle;
    public Vector2 Velocity;
    public float Acceleration;
    public float AngularVelocity;
    public float AngularAcceleration;
    public Rectangle Rectangle;
    public RotRectangle rotRect;
    public Timer Delay;
    public int Health;
    public Rocket(){

    }
}


public class Invader
{
    public Texture2D Texture;
    public Vector2 Scale;
    public Color Color;
    public Vector2 Position;
    public float OrbitRadius;
    public byte numberofpointsgiven;
    public float angle;
    public float orbitAngle;
    public float anglularvelocity;
    public float SightRadius;
    public Rectangle rectangle;
    public Timer delay;
    public int Health;
    public bool isparametric;
    public float parametricincrement;

    public Invader()
    {
        
    }
}
public class Bullet
{
    public Vector2 position;
    public float angle;
    public Vector2 momentum;
    
    public Rectangle selfrect;
    public List<Invader> selfrectlist;

    public Rectangle enemyrect;
    public List<Invader> enemyrectlist;
    public bool hascollided;
    public Timer alive_time;
    public Bullet()
    {

    }
}
