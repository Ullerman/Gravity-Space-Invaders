using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pleasework;

public class Rocket
{
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
    public Timer Delay;
    public int Health;

    public Rocket() { }

    public virtual Rectangle BoundingBox
    {
        get
        {
            int x, y, w, h;
            if (Angle != 0)
            {
                var cos = Math.Abs(Math.Cos(Angle));
                var sin = Math.Abs(Math.Sin(Angle));
                var t1_opp = Texture.Width * Scale.X * cos;
                var t1_adj = Math.Sqrt(Texture.Width * Scale.X * Texture.Width * Scale.X - t1_opp * t1_opp);
                var t2_opp = Texture.Height * Scale.Y * sin;
                var t2_adj = Math.Sqrt(Texture.Height * Scale.Y * Texture.Height * Scale.Y - t2_opp * t2_opp);

                w = (int)(t1_opp + t2_opp);
                h = (int)(t1_adj + t2_adj);
                x = (int)(Position.X - w / 2);
                y = (int)(Position.Y - h / 2);
            }
            else
            {
                x = (int)Position.X;
                y = (int)Position.Y;
                w = (int)(Texture.Width * Scale.X);
                h = (int)(Texture.Height * Scale.Y);
            }
            return new Rectangle(x, y, w, h);
        }
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
    public bool parametricbool;
    public Vector2 arrowScale;

    public Invader() { }

    public virtual Rectangle BoundingBox
    {
        get
        {
            int x, y, w, h;
            if (angle != 0)
            {
                var cos = Math.Abs(Math.Cos(angle));
                var sin = Math.Abs(Math.Sin(angle));
                var t1_opp = Texture.Width * Scale.X * cos;
                var t1_adj = Math.Sqrt(Texture.Width * Scale.X * Texture.Width * Scale.X - t1_opp * t1_opp);
                var t2_opp = Texture.Height * Scale.Y * sin;
                var t2_adj = Math.Sqrt(Texture.Height * Scale.Y * Texture.Height * Scale.Y - t2_opp * t2_opp);

                w = (int)(t1_opp + t2_opp);
                h = (int)(t1_adj + t2_adj);
                x = (int)(Position.X - w / 2);
                y = (int)(Position.Y - h / 2);
            }
            else
            {
                x = (int)Position.X;
                y = (int)Position.Y;
                w = (int)(Texture.Width * Scale.X);
                h = (int)(Texture.Height * Scale.Y);
            }
            return new Rectangle(x, y, w, h);
        }
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

    public Bullet() { }

    public virtual Rectangle BoundingBox
    {
        get
        {
            int x, y, w, h;
            if (angle != 0)
            {
                var cos = Math.Abs(Math.Cos(angle));
                var sin = Math.Abs(Math.Sin(angle));
                var t1_opp = selfrect.Width * cos;
                var t1_adj = Math.Sqrt(selfrect.Width * selfrect.Width - t1_opp * t1_opp);
                var t2_opp = selfrect.Height * sin;
                var t2_adj = Math.Sqrt(selfrect.Height * selfrect.Height - t2_opp * t2_opp);

                w = (int)(t1_opp + t2_opp);
                h = (int)(t1_adj + t2_adj);
                x = (int)(position.X - w / 2);
                y = (int)(position.Y - h / 2);
            }
            else
            {
                x = (int)position.X;
                y = (int)position.Y;
                w = selfrect.Width;
                h = selfrect.Height;
            }
            return new Rectangle(x, y, w, h);
        }
    }
}