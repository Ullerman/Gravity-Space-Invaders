﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Constants
{
    public const int SCREENHEIGHT = 720;
    public const int SCREENWIDTH = 960;
    public const float GRAVITATIONALSTRENGTH = 7000f;
}

// public bool isEven(int num) {

// }

namespace Pleasework
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private PrimitiveBatch _primitiveBatch;

        List<PrimitiveBatch.Line> lines = new List<PrimitiveBatch.Line>();
        List<PrimitiveBatch.Circle> circles = new List<PrimitiveBatch.Circle>();
        List<PrimitiveBatch.Rectangle> drawRect = new List<PrimitiveBatch.Rectangle>();
        List<PrimitiveBatch.Rectangle> hitBoxes = new List<PrimitiveBatch.Rectangle>();

        // private Desktop _desktop;

        const string filePath = "Main_Menu.xmmp";

        // string data;
        // Project project;

        // // byte level;

        Vector2 cameraPosition;
        float previousScrollWheelValue;
        float rawZoom;
        float smoothZoom;

        byte points;

        SpriteFont debugFont;
        TextBox debugText;

        Texture2D Pixel;
        Texture2D Circle;

        Texture2D arrow;
        Vector2 arrowScale;

        Texture2D Gameover;
        Texture2D Speedometrenopintexture;
        Texture2D speedOmeterpintexture;

        Texture2D BackroundTexture;
        Texture2D thingtexture;

        Texture2D AnnaRocket;
        bool togglerocket;
        bool toggleDebug;

        Vector2 thingposition;
        float coinT;
        bool coinbool;
        Vector2 coinscale;

        Rocket rocket;

        float friction;
        float angularFriction;

        Texture2D rocketTexture;
        Texture2D HealthTexture;

        Texture2D bullettexure;
        List<Bullet> Bulletlist;

        float bulletdefaultspeed;
        float bulletscale;
        Timer rocketbulletdelay;

        List<Invader> invaderlist;
        float invaderspeedmultiplyer;
        float invadertimermultiplyer;

        Texture2D Invader1Texture;

        // Texture2D Invader2Texture;
        SoundEffect[] InvaderNoiseArray = new SoundEffect[4];
        SoundEffect ShootSound;

        Texture2D earthTexture;
        Vector2 earthscale;
        Vector2 earthposition;

        Texture2D moontexture;
        Vector2 moonposition;
        Vector2 moonscale;
        float moonorbitradius;
        float moonangle;
        float moonanglularvelocity;

        Vector2 rocketcameraoffset;
        Random rnd;

        Effect effect;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
            Window.Title = "Asteroid Invader";
            Window.IsBorderless = false;

            // IsFixedTimeStep = false;
            // _graphics.SynchronizeWithVerticalRetrace = false;

            _graphics.PreferredBackBufferWidth = Constants.SCREENWIDTH;
            _graphics.PreferredBackBufferHeight = Constants.SCREENHEIGHT;
        }

        protected override void Initialize()
        {
            rnd = new Random();
            rawZoom = 1;
            smoothZoom = 1;

            previousScrollWheelValue = 0;

            togglerocket = true;
            toggleDebug = false;

            points = 0;
            arrowScale = new Vector2(.25f);

            thingposition = new Vector2(600 - 52.5f, 0);

            coinT = 0;
            coinbool = true;

            rocket = new Rocket();
            rocket.Position = new Vector2(300, 300);

            rocket.Velocity = new Vector2(0, 0);
            rocket.Angle = 0f;

            rocket.Acceleration = 0.4f;
            friction = 0.9999999f; // .1f;
            rocket.AngularVelocity = 0f;
            rocket.AngularAcceleration = 0.09f;
            angularFriction = .99f;
            rocket.Health = 3;

            bulletscale = 0.125f;

            Bulletlist = new List<Bullet>();

            bulletdefaultspeed = 10f;
            rocketbulletdelay = new Timer(0.4f);

            earthposition = new Vector2(Constants.SCREENWIDTH / 2, Constants.SCREENHEIGHT / 2);

            moonposition = earthposition + new Vector2(0, earthposition.Y / 2 + 100);
            moonscale = new Vector2(0.0625f, 0.0625f);
            moonanglularvelocity = 0.01f;
            moonangle = 0f;
            moonorbitradius = 650f;

            coinscale = new Vector2(.125f, .125f);
            rocket.Scale = new Vector2(.125f, .125f);
            earthscale = new Vector2(.25f, .25f);

            invaderlist = new List<Invader>();
            invaderspeedmultiplyer = 1.0f;
            invadertimermultiplyer = 1.0f;

            rocketcameraoffset = Vector2.Zero;

            debugText = new TextBox();

            _graphics.ApplyChanges();

            base.Initialize();
        }

        //private void LoadMenu() { }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //LoadMenu();

            Pixel = Content.Load<Texture2D>("Pixel");
            Circle = Content.Load<Texture2D>("Circle");

            arrow = Content.Load<Texture2D>("arrow");

            _primitiveBatch = new PrimitiveBatch();
            _primitiveBatch.Primitive(Pixel, Circle);

            debugFont = Content.Load<SpriteFont>("File");
            debugFont.LineSpacing = 10;

            effect = Content.Load<Effect>("CRT");

            Gameover = Content.Load<Texture2D>("game_over");
            Speedometrenopintexture = Content.Load<Texture2D>("speedometrenopin");
            speedOmeterpintexture = Content.Load<Texture2D>("pin");
            BackroundTexture = Content.Load<Texture2D>("Backround");

            thingtexture = Content.Load<Texture2D>("coin");

            rocketTexture = Content.Load<Texture2D>("rocket");

            AnnaRocket = Content.Load<Texture2D>("AnnaRocket");
            bullettexure = Content.Load<Texture2D>("bullet");
            HealthTexture = Content.Load<Texture2D>("heart");

            earthTexture = Content.Load<Texture2D>("earth");
            moontexture = Content.Load<Texture2D>("moon");

            Invader1Texture = Content.Load<Texture2D>("Invader1");

            InvaderNoiseArray[0] = Content.Load<SoundEffect>("fastinvader1");
            InvaderNoiseArray[1] = Content.Load<SoundEffect>("fastinvader2");
            InvaderNoiseArray[2] = Content.Load<SoundEffect>("fastinvader3");
            InvaderNoiseArray[3] = Content.Load<SoundEffect>("fastinvader4");
            ShootSound = Content.Load<SoundEffect>("shoot");
        }

        private void KeyHandling(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            var gamepadState = GamePad.GetState(PlayerIndex.One);
            var mouseState = Mouse.GetState();

            Vector2 leftThumbstick = gamepadState.ThumbSticks.Left;
            Vector2 rightThumbstick = gamepadState.ThumbSticks.Right;

            float leftTriggerValue = gamepadState.Triggers.Left;
            float rightTriggerValue = gamepadState.Triggers.Right;

            rocket.AngularVelocity += rocket.AngularAcceleration * leftThumbstick.X;
            rawZoom += .1f * rightThumbstick.Y;

            float triangleAnglecontrol = (float)(rocket.Angle - Math.PI / 2);
            rocket.Velocity.X +=
                (float)(MathF.Cos(triangleAnglecontrol) * rocket.Acceleration) * rightTriggerValue;
            rocket.Velocity.Y +=
                (float)(MathF.Sin(triangleAnglecontrol) * rocket.Acceleration) * rightTriggerValue;

            rocket.Velocity.X -=
                (float)(MathF.Cos(triangleAnglecontrol) * rocket.Acceleration) * leftTriggerValue;
            rocket.Velocity.Y -=
                (float)(MathF.Sin(triangleAnglecontrol) * rocket.Acceleration) * leftTriggerValue;

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                rocket.AngularVelocity -= rocket.AngularAcceleration;
            }

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                rocket.AngularVelocity += rocket.AngularAcceleration;
            }

            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))
            {
                float triangleAngle = (float)(rocket.Angle - Math.PI / 2);
                rocket.Velocity.X += (float)(MathF.Cos(triangleAngle) * rocket.Acceleration);
                rocket.Velocity.Y += (float)(MathF.Sin(triangleAngle) * rocket.Acceleration);
            }

            if (
                kstate.IsKeyDown(Keys.Down)
                || gamepadState.Triggers.Left > 0.5f
                || kstate.IsKeyDown(Keys.S)
            )
            {
                float triangleAngle = (float)(rocket.Angle - Math.PI / 2);
                rocket.Velocity.X -= (float)(MathF.Cos(triangleAngle) * rocket.Acceleration);
                rocket.Velocity.Y -= (float)(MathF.Sin(triangleAngle) * rocket.Acceleration);
            }

            if (kstate.IsKeyDown(Keys.Space) || gamepadState.Buttons.A == ButtonState.Pressed)
            {
                FireBullet(
                    rocket.Position,
                    rocket.Angle,
                    rocket.Velocity,
                    ref rocket.Rectangle,
                    ref invaderlist,
                    ref rocketbulletdelay,
                    gameTime
                );
            }

            if (mouseState.ScrollWheelValue > previousScrollWheelValue)
            {
                rawZoom += 0.1f;
            }
            else if (mouseState.ScrollWheelValue < previousScrollWheelValue)
            {
                rawZoom -= 0.1f;
            }

            rawZoom = Math.Clamp(rawZoom, 0.1f, 3.0f);  

            previousScrollWheelValue = mouseState.ScrollWheelValue;

            smoothZoom = MathHelper.Lerp(smoothZoom, rawZoom, .1f);

            if (kstate.IsKeyDown(Keys.R) || gamepadState.Buttons.Start == ButtonState.Pressed)
            {
                rocket.Position = new Vector2(
                    Constants.SCREENWIDTH / 8,
                    Constants.SCREENHEIGHT / 8
                );
                rocket.Velocity = Vector2.Zero;
                rocket.AngularVelocity = 0;
                rocket.Health = 3;
                invaderlist.Clear();
                Bulletlist.Clear();

                invaderspeedmultiplyer = 1;
                invadertimermultiplyer = 1;
            }
            if (kstate.IsKeyDown(Keys.P))
            {
                togglerocket = !togglerocket;
            }
            if (kstate.IsKeyDown(Keys.F3))
            {
                toggleDebug = !toggleDebug;
            }
            if (kstate.IsKeyDown(Keys.F4) && kstate.IsKeyDown(Keys.L))
            {
                drawRect.Clear();
            }
            if (kstate.IsKeyDown(Keys.F11))
            {
                _graphics.ToggleFullScreen();
            }
        }

        private static Vector2 RadialToVector(float angle, float length)
        {
            return new Vector2(MathF.Cos(angle) * length, MathF.Sin(angle) * length);
        }

        private void FireBullet(
            Vector2 position,
            float fireangle,
            Vector2 velocity,
            ref Rectangle selfrect,
            ref List<Invader> enemyrectlist,
            ref Timer delay,
            GameTime gameTime
        )
        {
            delay.Update(gameTime);

            if (delay.IsFinished() || !delay.IsRunning())
            {
                this.ShootSound.Play(1, rnd.Next(-100, 100) / 100, 0);

                Bullet bullet = new Bullet();

                float angle = fireangle + rnd.Next(-60, 60) / 100;
                float triangleAngle = (float)(angle - Math.PI / 2);
                Vector2 offset = RadialToVector(triangleAngle, 20);
                bullet.position = new Vector2(position.X, position.Y) + offset;
                bullet.angle = angle;
                bullet.momentum = RadialToVector(triangleAngle, bulletdefaultspeed) + velocity;
                bullet.selfrect = selfrect;
                bullet.enemyrectlist = enemyrectlist;
                bullet.hascollided = false;
                Bulletlist.Add(bullet);

                delay.Start();
            }
        }

        private void FireBullet(
            Vector2 position,
            float fireangle,
            Vector2 velocity,
            ref List<Invader> selfrectlist,
            ref Rectangle enemyrect,
            ref Timer delay,
            GameTime gameTime
        )
        {
            delay.Update(gameTime);
            // Console.WriteLine(delay.GetRemainingTime() + "" );
            if (delay.IsFinished() || !delay.IsRunning())
            {
                ShootSound.Play(1, rnd.Next(-100, 100) / 100, 0);
                Bullet bullet = new Bullet();

                float angle = fireangle + rnd.Next(-60, 60) / 100;
                float triangleAngle = (float)(angle + Math.PI / 2);

                bullet.position = position;
                bullet.angle = angle;
                bullet.momentum = new Vector2(
                    (float)(Math.Cos(angle) * bulletdefaultspeed + velocity.X),
                    (float)(Math.Sin(angle) * bulletdefaultspeed + velocity.Y)
                );
                float momentumDirection = MathF.Atan2(bullet.momentum.Y, bullet.momentum.X);
                lines.Add(
                    new PrimitiveBatch.Line(bullet.position, momentumDirection, 500, Color.Red, 10)
                );
                bullet.selfrectlist = selfrectlist;
                bullet.enemyrect = enemyrect;
                Bulletlist.Add(bullet);

                delay.Start();
            }
        }

        private void UpdateBullet()
        {
            float earthradius = (earthTexture.Width * earthscale.X) / 2 - 5;
            if (Bulletlist != null)
            {
                List<Bullet> removebullets = new List<Bullet>();
                List<Invader> removeinvader = new List<Invader>();
                foreach (Bullet bullet in Bulletlist)
                {
                    bullet.position += bullet.momentum;
                    bullet.angle = MathF.Atan2(bullet.momentum.Y, bullet.momentum.X);

                    Rectangle bulletrect = bullet.BoundingBox;

                    if (
                        IsRectCollidingWithCircle(earthposition, earthradius, bulletrect)
                        & !rocket.Rectangle.Intersects(bulletrect)
                    )
                    {
                        removebullets.Add(bullet);
                    }
                    if (bullet.enemyrectlist != null)
                    {
                        foreach (Invader enemy in bullet.enemyrectlist)
                        {
                            if (
                                enemy.rectangle.Intersects(bulletrect)
                                & !bullet.selfrect.Intersects(bulletrect)
                                & !bullet.hascollided
                            )
                            {
                                removebullets.Add(bullet);
                                removeinvader.Add(enemy);
                                bullet.hascollided = true;
                                points += 10;
                            }
                        }
                    }
                    else
                    {
                        foreach (Invader self in bullet.selfrectlist)
                        {
                            if (
                                bullet.enemyrect.Intersects(bulletrect)
                                & !self.rectangle.Intersects(bulletrect)
                                & !bullet.hascollided
                            )
                            {
                                removebullets.Add(bullet);
                                //removeinvader.Add(enemy);
                                rocket.Health -= 1;
                                bullet.hascollided = true;
                            }
                        }
                    }
                }
                foreach (Bullet bullet in removebullets)
                {
                    Bulletlist.Remove(bullet);
                }
                foreach (Invader invader in removeinvader)
                {
                    invaderlist.Remove(invader);
                }
            }
        }

        private (Vector2, float, bool) parametricmovement(
            Vector2 center,
            int radius,
            float timetocomplete,
            float increment,
            bool max,
            GameTime gameTime
        )
        {
            //https://www.desmos.com/calculator/bxgdwxj6xs
            Vector2 position = new Vector2();

            position.X = center.X + radius * MathF.Cos(9.95f * increment);
            position.Y = center.Y + radius * MathF.Sin(3.34f * increment);

            if (max)
                increment += 2 * MathF.PI / (60 * timetocomplete);
            else
                increment -= 2 * MathF.PI / (60 * timetocomplete);
            if (increment >= 10)
                max = false;
            else if (increment <= 0)
                max = true;
            drawRect.Add(
                new PrimitiveBatch.Rectangle(position, new Vector2(10), Color.MonoGameOrange)
            );

            return (position, increment, max);
        }

        private void Coin(GameTime gameTime)
        {
            // thingposition = parametricmovement(earthposition, 200, 30, gameTime);
            (thingposition, coinT, coinbool) = parametricmovement(
                earthposition,
                784,
                30,
                coinT,
                coinbool,
                gameTime
            );
        }

        private Vector2 GravityCalculation(
            Vector2 gravitysourcepos,
            Vector2 objectpos,
            Vector2 objectmomentum,
            float offset,
            float multiplier
        )
        {
            Vector2 gravitystrength;
            Vector2 direction = gravitysourcepos - objectpos;
            float distance = direction.Length();
            if (distance > .1f)
            {
                direction.Normalize();
                gravitystrength =
                    direction
                    * (
                        Constants.GRAVITATIONALSTRENGTH
                        * multiplier
                        / ((distance + offset) * (distance + offset))
                    );
                objectmomentum += gravitystrength;
            }

            return objectmomentum;
        }

        private void SpawnInvaders()
        {
            for (int i = 0; i <= 10; i++)
            {
                invaderlist.Add(new Invader());
                Invader invader = invaderlist[i];
                invader.Texture = Invader1Texture;
                invader.Position = new Vector2(
                    rnd.Next(0, Constants.SCREENWIDTH),
                    rnd.Next(0, Constants.SCREENHEIGHT)
                );
                invader.Scale = new Vector2(1, 1);
                invader.orbitAngle = rnd.Next(0, 200);
                invader.Color = new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255));
                byte parametric = (byte)rnd.Next(0, 3);
                invader.isparametric = parametric == 1;
                invader.arrowScale = new Vector2(.25f);
                if (invader.isparametric)
                {
                    invader.OrbitRadius = rnd.Next(910, 1600);
                    invader.anglularvelocity = rnd.Next(30, 150);
                    float parametricincrement =
                        2 * MathF.PI / (60 * invader.anglularvelocity) * rnd.Next(6);
                    invader.parametricincrement = parametricincrement;
                    byte parabool = (byte)rnd.Next(0, 2);
                    invader.parametricbool = parabool == 1;
                }
                else
                {
                    invader.OrbitRadius = rnd.Next(400, 800);
                    invader.anglularvelocity =
                        (float)rnd.Next(10, 15) / 1000 * invaderspeedmultiplyer;
                }
                invader.SightRadius = rnd.Next(100, 300);
                invader.delay = new Timer(0.75f * invadertimermultiplyer);
            }
        }

        private void Physics(double deltatime)
        {
            float gravityOffset = 75f;

            rocket.Velocity = GravityCalculation(
                earthposition,
                rocket.Position,
                rocket.Velocity,
                gravityOffset,
                6
            );

            rocket.Velocity = GravityCalculation(
                moonposition,
                rocket.Position,
                rocket.Velocity,
                gravityOffset * 0.6f,
                3.9f
            );
            foreach (Bullet bullet in Bulletlist)
            {
                bullet.momentum = GravityCalculation(
                    earthposition,
                    bullet.position,
                    bullet.momentum,
                    gravityOffset,
                    6
                );
                bullet.momentum = GravityCalculation(
                    moonposition,
                    bullet.position,
                    bullet.momentum,
                    gravityOffset,
                    4
                );
            }

            rocket.Angle += (float)(rocket.AngularVelocity * deltatime);
            rocket.AngularVelocity *= angularFriction;

            rocket.Position += rocket.Velocity;
            rocket.Velocity *= friction;

            Vector2 earthcentre = earthposition;
            float earthradius = (earthTexture.Width * earthscale.X) / 2 - 5;

            if (IsRectCollidingWithCircle(earthcentre, earthradius, rocket.Rectangle))
            {
                Vector2 collisionNormal = rocket.Position - earthcentre;
                collisionNormal.Normalize();
                // rocket.Velocity *= friction;
                float penetration = earthradius - (rocket.Position - earthcentre).Length();
                if (penetration > 0)
                {
                    rocket.Position += collisionNormal * penetration;
                }
                if (rocket.Velocity.X > 1 || rocket.Velocity.Y > 1)
                {
                    rocket.Velocity = Vector2.Reflect(rocket.Velocity, collisionNormal) * 0.8f;
                    rocket.AngularVelocity = -rocket.AngularVelocity * 0.5f;
                }
                if (rocket.Velocity.Length() < 0.1f)
                {
                    //rocket.Velocity = Vector2.Zero;
                }
                if (
                    DistancesquaredBetweenpointandrect(rocket.Rectangle, earthcentre)
                    < earthradius * earthradius + 36
                )
                {
                    rocket.Velocity *= friction;
                }
            }
        }

        private float DistancesquaredBetweenpointandrect(Rectangle rectangle, Vector2 point)
        {
            float closestX = Math.Clamp(point.X, rectangle.Left, rectangle.Right);
            float closestY = Math.Clamp(point.Y, rectangle.Top, rectangle.Bottom);

            float distanceX = point.X - closestX;
            float distanceY = point.Y - closestY;

            float distance = distanceX * distanceX + distanceY * distanceY;
            return distance;
        }

        private bool IsRectCollidingWithCircle(
            Vector2 circleCenter,
            float circleRadius,
            Rectangle rect
        )
        {
            float distance = DistancesquaredBetweenpointandrect(rect, circleCenter);
            return distance <= (circleRadius * circleRadius);
        }

        private float CalculateAngleBetweenPoints(Vector2 point1, Vector2 point2)
        {
            float deltaX = point2.X - point1.X;
            float deltaY = point2.Y - point1.Y;

            float angle = MathF.Atan2(deltaY, deltaX);

            return angle;
        }

        private void InvaderCompute(GameTime gameTime)
        {
            foreach (Invader invader in invaderlist)
            {
                circles.Add(
                    new PrimitiveBatch.Circle(
                        invader.Position,
                        invader.SightRadius,
                        new Color(255, 0, 0, 50)
                    )
                );
                if (
                    IsRectCollidingWithCircle(
                        invader.Position,
                        invader.SightRadius,
                        rocket.Rectangle
                    )
                )
                {
                    // Console.WriteLine("invadersee");
                    float angle = CalculateAngleBetweenPoints(invader.Position, rocket.Position);
                    //lines.Add(new PrimitiveBatch.Line(invader.Position, rocket.Position, Color.White, 5));
                    lines.Add(
                        new PrimitiveBatch.Line(
                            invader.Position,
                            invader.angle,
                            500,
                            Color.White,
                            5
                        )
                    );
                    invader.angle = MathHelper.Lerp(invader.angle, angle - MathF.PI / 2, 0.025f);
                    FireBullet(
                        invader.Position,
                        invader.angle + MathF.PI / 2,
                        Vector2.Zero,
                        ref invaderlist,
                        ref rocket.Rectangle,
                        ref invader.delay,
                        gameTime
                    );
                }
                else
                {
                    float angle = CalculateAngleBetweenPoints(invader.Position, earthposition);
                    invader.angle = MathHelper.Lerp(invader.angle, angle - MathF.PI / 2, 0.125f); //angle- MathF.PI/2;
                }
                if (!invader.isparametric)
                {
                    invader.orbitAngle += invader.anglularvelocity;
                    invader.Position.X =
                        earthposition.X
                        + (float)(invader.OrbitRadius * Math.Cos(invader.orbitAngle));
                    invader.Position.Y =
                        earthposition.Y
                        + (float)(invader.OrbitRadius * Math.Sin(invader.orbitAngle));
                    invader.rectangle.X = (int)invader.Position.X;
                    invader.rectangle.Y = (int)invader.Position.Y;
                }
                else
                {
                    (invader.Position, invader.parametricincrement, invader.parametricbool) =
                        parametricmovement(
                            earthposition,
                            (int)invader.OrbitRadius,
                            invader.anglularvelocity,
                            invader.parametricincrement,
                            invader.parametricbool,
                            gameTime
                        );
                }
            }
        }

        private void Bounds(GameTime gameTime)
        {
            float distanceSquared = DistancesquaredBetweenpointandrect(
                rocket.Rectangle,
                earthposition
            );

            float outOfBoundsDistanceSquared = 100000000;
            float maxDistanceSquared = 400000000;
            if (distanceSquared > outOfBoundsDistanceSquared)
            {
                float distanceRatio = MathHelper.Clamp(
                    (distanceSquared - outOfBoundsDistanceSquared)
                        / (maxDistanceSquared - outOfBoundsDistanceSquared),
                    0f,
                    1f
                );

                rocket.Velocity *= 1f - (distanceRatio * 0.05f);

                float lerpFactor = 0.01f * distanceRatio;
                rocket.Position = Vector2.Lerp(rocket.Position, earthposition, lerpFactor);
            }
        }

        private void Level_Update(GameTime gameTime)
        {
            if (invaderlist.Count == 0)
            {
                invaderspeedmultiplyer += .1f;
                invadertimermultiplyer -= .1f;

                SpawnInvaders();
            }
        }


        protected override void Update(GameTime gameTime)
        {
            double deltatime = gameTime.ElapsedGameTime.TotalSeconds;

            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
            )
                Exit();
            if (togglerocket)
            {
                rocket.Texture = rocketTexture;
                rocket.Scale = new Vector2(.0625f);
            }
            else
            {
                rocket.Texture = AnnaRocket;
                rocket.Scale = new Vector2(.25f);
            }
            Coin(gameTime);

            if (rocket.Health > 0)
            {
                rocketbulletdelay.Update(gameTime);
                Physics(deltatime);
                Level_Update(gameTime);
                UpdateBullet();
                InvaderCompute(gameTime);
            }
            KeyHandling(gameTime);
            Bounds(gameTime);

            moonangle += moonanglularvelocity;
            moonposition.X = earthposition.X + (float)(moonorbitradius * Math.Cos(moonangle));
            moonposition.Y = earthposition.Y + (float)(moonorbitradius * Math.Sin(moonangle));

            Camera();
            base.Update(gameTime);
        }

        private void Camera()
        {
            Vector2 targetScreenPosition = new Vector2(
                Window.ClientBounds.Width / 2,
                Window.ClientBounds.Height / 2
            );

            Vector2 rocketScreenPosition = rocket.Position + rocketcameraoffset;

            Vector2 directionToCenter = targetScreenPosition - rocketScreenPosition;
            float distanceX = rocketScreenPosition.X - targetScreenPosition.X;
            float distanceY = rocketScreenPosition.Y - targetScreenPosition.X;
            float distance = distanceX * distanceX + distanceY * distanceY;
            float returnSpeed = 0.25f;
            if (distance > 400 * 400)
            {
                rocketcameraoffset = Vector2.Lerp(
                    rocketcameraoffset,
                    rocketcameraoffset + directionToCenter,
                    returnSpeed * 2.5f
                );
            }
            else
                rocketcameraoffset = Vector2.Lerp(
                    rocketcameraoffset,
                    rocketcameraoffset + directionToCenter,
                    returnSpeed
                );
            //cameraPosition = rocket.Position + rocketcameraoffset;
            cameraPosition = rocketcameraoffset;
        }

        void DrawBackground(SpriteBatch _spriteBatch, Vector2 CameraOffset)
        {
            Vector2 tileSize = new Vector2(BackroundTexture.Width * smoothZoom, BackroundTexture.Height * smoothZoom);

            int tilesX = (int)Math.Ceiling(Window.ClientBounds.Width / tileSize.X) + 2;
            int tilesY = (int)Math.Ceiling(Window.ClientBounds.Height / tileSize.Y) + 2;

            float offsetX = -CameraOffset.X % tileSize.X;
            float offsetY = -CameraOffset.Y % tileSize.Y;

            for (int y = -1; y < tilesY; y++)
            {
                for (int x = -1; x < tilesX; x++)
                {
                    Vector2 tilePosition = new Vector2(
                        (x * tileSize.X) - offsetX,
                        (y * tileSize.Y) - offsetY
                    );
                    // new Vector2(BackroundTexture.Width/2,BackroundTexture.Height/2)
                    _spriteBatch.Draw(BackroundTexture, tilePosition, null, Color.White, rnd.Next(25 / 30) / 10, new Vector2(BackroundTexture.Width / 2, BackroundTexture.Height / 2), Vector2.One * smoothZoom, SpriteEffects.None, 0);
                }
            }
        }

        private Vector2 HUDArrowtopoint(
            Vector2 firstPoint,
            Vector2 secondPoint,
            float distancetodisapear,
            Texture2D arrowTexture,
            Vector2 arrowScale,
            Vector2 cameraoffset,
            Color color,
            SpriteBatch _spriteBatch
        )
        {
            float angle = CalculateAngleBetweenPoints(firstPoint, secondPoint);
            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 arrowPosition = firstPoint + direction * 100;
            float arrowAngle = angle + MathF.PI / 2;
            float distanceFromEarth = Vector2.Distance(firstPoint, secondPoint);
            Vector2 fullscale = new Vector2(0.25f);
            Vector2 smallscale = new Vector2(0.01f);

            if (distanceFromEarth > distancetodisapear)
            {
                arrowScale = Vector2.Lerp(arrowScale, fullscale, .1f);
            }
            else
            {
                arrowScale = Vector2.Lerp(arrowScale, smallscale, .1f);
            }
            if (arrowScale.X >= 0.02f)
                _spriteBatch.Draw(
                    arrowTexture,
                    (arrowPosition + cameraoffset) * smoothZoom,
                    null,
                    color,
                    arrowAngle,
                    new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2),
                    arrowScale * smoothZoom,
                    SpriteEffects.None,
                    0
                );
            return arrowScale;
        }

        private void DrawSpeedometer(SpriteBatch _spriteBatch)
        {
            Vector2 speedometerscale = new Vector2(0.5f);
            Vector2 pinscale = new Vector2(0.5f);
            Vector2 anglebounds = new Vector2(-135, 135);
            float speedPercentage = rocket.Velocity.Length() / 100;
            float angle = MathHelper.Lerp(anglebounds.X, anglebounds.Y, speedPercentage);
            angle = MathHelper.ToRadians(angle);

            _spriteBatch.Draw(
                Speedometrenopintexture,
                new Vector2(
                    Window.ClientBounds.Width - Speedometrenopintexture.Width * speedometerscale.X,
                    0
                ),
                null,
                Color.White,
                0,
                Vector2.Zero,
                speedometerscale,
                SpriteEffects.None,
                0
            );

            Vector2 pinPosition = new Vector2(
                Window.ClientBounds.Width - Speedometrenopintexture.Width * speedometerscale.X / 2,
                Speedometrenopintexture.Height * speedometerscale.Y / 2
            );

            _spriteBatch.Draw(
                speedOmeterpintexture,
                pinPosition,
                null,
                Color.White,
                angle,
                new Vector2(speedOmeterpintexture.Width / 2, speedOmeterpintexture.Height),
                pinscale,
                SpriteEffects.None,
                0
            );
        }

        private void DrawHUD(Vector2 cameraoffset, SpriteBatch _spriteBatch)
        {
            Vector2 heartscale = new Vector2(0.0625f, 0.0625f);
            float speed = rocket.Velocity.Length();
            string text =
                $"X: {rocket.Position.X}\n Y: {rocket.Position.Y}";//speed : {MathF.Round(speed, 3)} 
            debugText.Draw(_spriteBatch, text, debugFont, 400, new Vector2(10, 75), Color.White);
            _spriteBatch.DrawString(debugFont,
                    $"Points: {points}",
                    new Vector2(10,Window.ClientBounds.Height*.92f),
                    Color.OrangeRed,0,Vector2.Zero,new Vector2(2),
                    SpriteEffects.None,
                    0
                );

            for (int i = 0; i < rocket.Health; i++)
            {
                _spriteBatch.Draw(
                    HealthTexture,
                    new Vector2(HealthTexture.Width * heartscale.X * i, 0),
                    null,
                    Color.White,
                    0,
                    new Vector2(0, 0),
                    heartscale,
                    SpriteEffects.None,
                    0
                );
            }

            arrowScale = HUDArrowtopoint(
                rocket.Position,
                earthposition,
                500,
                arrow,
                arrowScale,
                cameraoffset,
                Color.White,
                _spriteBatch
            );

            if (rocket.Health == 0)
            {
                _spriteBatch.Draw(
                    Gameover,
                    new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2),
                    null,
                    Color.Red,
                    0,
                    new Vector2(Gameover.Width / 2, Gameover.Height / 2),
                    new Vector2(.5f, .5f),
                    SpriteEffects.None,
                    0
                );
            }
            if (invaderlist.Count < 4)
            {
                foreach (Invader invader in invaderlist)
                {
                    invader.arrowScale = HUDArrowtopoint(
                        rocket.Position,
                        invader.Position,
                        150,
                        arrow,
                        invader.arrowScale,
                        rocketcameraoffset,
                        Color.Red,
                        _spriteBatch
                    );
                }
            }
            DrawSpeedometer(_spriteBatch);
        }

        private void DrawDebugGraphics(SpriteBatch _spriteBatcht, Vector2 cameraoffset)
        {
            foreach (PrimitiveBatch.Line line in lines)
            {
                line.Start = (line.Start + cameraoffset) * smoothZoom;
                line.Width *= smoothZoom;
                

                line.Draw(_spriteBatch, _primitiveBatch);
            }
            lines.Clear();
            foreach (PrimitiveBatch.Circle circle in circles)
            {
                circle.Position = (circle.Position + cameraoffset) * smoothZoom;
                circle.Radius *= smoothZoom;

                circle.Draw(_spriteBatch, _primitiveBatch);
            }
            foreach (PrimitiveBatch.Rectangle rectangle in drawRect)
            {
                rectangle.Position = (rectangle.Position + cameraoffset) * smoothZoom;
                rectangle.Size *= smoothZoom;
                rectangle.Draw(_spriteBatch, _primitiveBatch);
            }
            foreach (PrimitiveBatch.Rectangle rectangle in hitBoxes){
                rectangle.Position = (rectangle.Position +cameraoffset)* smoothZoom;
                rectangle.Size *= smoothZoom;
                rectangle.Draw(_spriteBatch,_primitiveBatch);
            }
            hitBoxes.Clear();
            
            circles.Clear();
        }

        private void DrawEnemies(SpriteBatch _spriteBatch, Vector2 cameraoffset)
        {
            foreach (Invader invader in invaderlist)
            {
                invader.rectangle = new Rectangle(
                    (int)invader.Position.X,
                    (int)invader.Position.Y,
                    (int)(Invader1Texture.Width * invader.Scale.X),
                    (int)(Invader1Texture.Height * invader.Scale.Y)
                );

                _spriteBatch.Draw(
                    Invader1Texture,
                    (invader.Position + cameraoffset) * smoothZoom,
                    null,
                    invader.Color,
                    invader.angle,
                    new Vector2(Invader1Texture.Width / 2, Invader1Texture.Height / 2),
                    invader.Scale * smoothZoom,
                    SpriteEffects.None,
                    0
                );
            }
        }

        private void DrawBullets(SpriteBatch _spriteBatch, Vector2 cameraoffset)
        {
            if (Bulletlist != null)
            {
                foreach (Bullet bullet in Bulletlist)
                {
                    
                    _spriteBatch.Draw(
                        bullettexure,
                        (bullet.position + cameraoffset) * smoothZoom,
                        null,
                        Color.White,
                        bullet.angle,
                        new Vector2(bullettexure.Width / 2, bullettexure.Height / 2),
                        bulletscale * smoothZoom,
                        SpriteEffects.None,
                        0
                    );
                }
            }
        }

        private void DrawEnviroment(SpriteBatch _spriteBatch, Vector2 cameraoffset)
        {
            _spriteBatch.Draw(
                earthTexture,
                (earthposition + cameraoffset) * smoothZoom,
                null,
                Color.White,
                0,
                new Vector2(earthTexture.Width / 2, earthTexture.Height / 2),
                earthscale * smoothZoom,
                SpriteEffects.None,
                0
            );
            _spriteBatch.Draw(
                moontexture,
                (moonposition + cameraoffset) * smoothZoom,
                null,
                Color.White,
                0,
                new Vector2(moontexture.Width / 2, moontexture.Height / 2),
                moonscale * smoothZoom,
                SpriteEffects.None,
                0
            );
        }

        private void DrawConsumables(SpriteBatch _spriteBatch, Vector2 cameraoffset)
        {
            _spriteBatch.Draw(
                thingtexture,
                (thingposition + cameraoffset) * smoothZoom,
                null,
                Color.White,
                0,
                Vector2.Zero,
                coinscale * smoothZoom,
                SpriteEffects.None,
                0
            );
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //Vector2 cameraoffset = (
            //    -rocket.Position
            //    + new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2) / smoothZoom
            //);
            Vector2 cameraoffset = cameraPosition + new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2) / smoothZoom - new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);

            _spriteBatch.Begin();

            DrawBackground(_spriteBatch, cameraoffset);

            DrawEnviroment(_spriteBatch, cameraoffset);
            DrawBullets(_spriteBatch, cameraoffset);
            DrawConsumables(_spriteBatch, cameraoffset);
            if (toggleDebug)
                DrawDebugGraphics(_spriteBatch, cameraoffset);
            DrawEnemies(_spriteBatch, cameraoffset);

            rocket.Rectangle = rocket.BoundingBox;
            hitBoxes.Add(new PrimitiveBatch.Rectangle(rocket.Rectangle,Color.HotPink));
            _spriteBatch.Draw(
                rocket.Texture,
                (rocket.Position + cameraoffset) * smoothZoom,
                null,
                Color.White,
                rocket.Angle,
                new Vector2(rocket.Texture.Width / 2, rocket.Texture.Height / 2),
                rocket.Scale * smoothZoom,
                SpriteEffects.None,
                0
            );

            DrawHUD(cameraoffset, _spriteBatch);
            _spriteBatch.End();
            //_desktop.Render();

            base.Draw(gameTime);
        }

        protected override void UnloadContent() { }
    }
}
