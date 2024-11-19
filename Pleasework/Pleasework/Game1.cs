using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using AssetManagementBase;
using FontStashSharp.RichText;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using Pleasework;

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
        
        

        // private Desktop _desktop;

        const string filePath = "Main_Menu.xmmp";

        // string data;
        // Project project;

        byte level;

        Vector2 cameraPosition;



        SpriteFont arial;

        byte r,
            g,
            b;
        bool iscolourforward;

        char whatcolour;

        Texture2D Pixel;
        Texture2D Circle;

        Texture2D Gameover;

        Texture2D BackroundTexture;
        Texture2D thingtexture;

        Texture2D AnnaRocket;
        bool togglerocket;

        Vector2 thingposition;
        float updatecoin;
        float fakecoinx;
        float coinT;
        bool coinforward;
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
        Texture2D Invader2Texture;
        SoundEffect[] InvaderNoiseArray = new SoundEffect[4];
        SoundEffect ShootSound;

        Texture2D earthTexture;
        Vector2 earthscale;
        Vector2 earthposition;

        Texture2D moontexture;
        Vector2 moonposition;
        Vector2 moonvelocity;
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

            MyraEnvironment.Game = this;
        }

        protected override void Initialize()
        {
            rnd = new Random();

            togglerocket = true;

            r = 0;
            g = 0;
            b = 0;

            iscolourforward = true;
            coinforward = false;
            whatcolour = 'r';
            thingposition = new Vector2(600 - 52.5f, 0);
            updatecoin = 30;
            fakecoinx = 600.0f;
            coinT = 0;

            rocket = new Rocket();
            rocket.Position = new Vector2(300, 300);
            rocket.Velocity = new Vector2(0, 0);
            rocket.Angle = 0f;
            rocket.Acceleration = 0.5f;
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

            _graphics.ApplyChanges();

            base.Initialize();
        }

        private void LoadMenu() { }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadMenu();

            Pixel = Content.Load<Texture2D>("Pixel");
            Circle = Content.Load<Texture2D>("Circle");
            

            _primitiveBatch = new PrimitiveBatch();
            _primitiveBatch.Primitive(Pixel,Circle);

            arial = Content.Load<SpriteFont>("File");

            effect = Content.Load<Effect>("CRT");

            Gameover = Content.Load<Texture2D>("game_over");
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

            Vector2 leftThumbstick = gamepadState.ThumbSticks.Left;
            Vector2 rightThumbstick = gamepadState.ThumbSticks.Right;

            float leftTriggerValue = gamepadState.Triggers.Left;
            float rightTriggerValue = gamepadState.Triggers.Right;

            rocket.AngularVelocity += rocket.AngularAcceleration * leftThumbstick.X;

            float triangleAnglecontrol = (float)(rocket.Angle - Math.PI / 2);
            rocket.Velocity.X +=
                (float)(MathF.Cos(triangleAnglecontrol) * rocket.Acceleration) * rightTriggerValue;
            rocket.Velocity.Y +=
                (float)(MathF.Sin(triangleAnglecontrol) * rocket.Acceleration) * rightTriggerValue;

            rocket.Velocity.X -=
                (float)(Math.Cos(triangleAnglecontrol) * rocket.Acceleration) * leftTriggerValue;
            rocket.Velocity.Y -=
                (float)(Math.Sin(triangleAnglecontrol) * rocket.Acceleration) * leftTriggerValue;

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

            if (kstate.IsKeyDown(Keys.R) || gamepadState.Buttons.Start == ButtonState.Pressed)
            {
                rocket.Position = new Vector2(
                    Constants.SCREENWIDTH / 8,
                    Constants.SCREENHEIGHT / 8
                );
                rocket.Velocity = Vector2.Zero;
                rocket.AngularVelocity = 0;
                rocket.Health = 3;
            }
            if (kstate.IsKeyDown(Keys.P))
            {
                togglerocket = !togglerocket;
            }
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
                ShootSound.Play(1, rnd.Next(-100, 100) / 100, 0);

                Bullet bullet = new Bullet();

                float angle = fireangle + rnd.Next(-60, 60) / 100;
                float triangleAngle = (float)(angle - Math.PI / 2);
                Vector2 offset = new Vector2(
                    MathF.Cos(triangleAngle) * 20,
                    MathF.Sin(triangleAngle) * 20
                );
                bullet.position = new Vector2(position.X, position.Y) + offset;
                bullet.angle = angle;
                bullet.momentum = new Vector2(
                    (float)(Math.Cos(triangleAngle) * bulletdefaultspeed + velocity.X),
                    (float)(Math.Sin(triangleAngle) * bulletdefaultspeed + velocity.Y)
                );
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
                float triangleAngle = (float)(angle - Math.PI / 2);

                bullet.position = position;
                bullet.angle = angle;
                bullet.momentum = new Vector2(
                    (float)(Math.Cos(triangleAngle) * bulletdefaultspeed + velocity.X),
                    (float)(Math.Sin(triangleAngle) * bulletdefaultspeed + velocity.Y)
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

                    Rectangle bulletrect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        (int)(bullettexure.Width * bulletscale),
                        (int)(bullettexure.Height * bulletscale)
                    );

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

        private Vector2 parametricmovement(
            Vector2 center,
            int radius,
            float timetocomplete,
            GameTime gameTime
        )
        {
            Vector2 position = new Vector2();

            position.Y = center.Y + radius * MathF.Sin(3 * coinT);
            position.X = center.X + radius * MathF.Cos(5 * coinT);

            coinT += 2 * MathF.PI / (60 * timetocomplete);
            return position;
        }

        private void Coin(GameTime gameTime)
        {
            thingposition = parametricmovement(earthposition, 200, 30, gameTime);
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
                invader.angle = rnd.Next(0, 200);
                invader.Color = new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255));
                byte parametric = (byte)rnd.Next(0, 3);
                invader.isparametric = parametric == 1;
                if (invader.isparametric)
                {
                    invader.OrbitRadius = rnd.Next(400, 800);
                    invader.anglularvelocity = rnd.Next(30, 150);
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
                    // rocket.AngularVelocity = -rocket.AngularVelocity * 0.5f;
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

        private void BackroundColour()
        {
            //if (r == 255 || r == 0)
            //    iscolourforward = !iscolourforward;


            //if (iscolourforward)
            //    r++;
            //else
            //    r--;

            if (r < 255 & g == 0 & b == 0)
                r++;
            else if (r == 255 & g < 255 & b == 0)
                g++;
            else if (r == 255 & g == 255 & b < 255)
                b++;
            else if (r <= 255 & g == 255 & b == 255)
                r--;

            //else if r = 2 & g < 255 & b == 0
            //    g++
            //else if r = 255 & g = 255 & b < 255
            //    b++

            //g++;
            //b++;
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

            float angle = MathF.Atan2(deltaX, deltaY);

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
                    lines.Add(new PrimitiveBatch.Line(invader.Position, rocket.Position, Color.White, 5));
                    FireBullet(
                        invader.Position,
                        angle,
                        Vector2.Zero,
                        ref invaderlist,
                        ref rocket.Rectangle,
                        ref invader.delay,
                        gameTime
                    );
                }
                if (!invader.isparametric)
                {
                    invader.angle += invader.anglularvelocity;
                    invader.Position.X =
                        earthposition.X + (float)(invader.OrbitRadius * Math.Cos(invader.angle));
                    invader.Position.Y =
                        earthposition.Y + (float)(invader.OrbitRadius * Math.Sin(invader.angle));
                    invader.rectangle.X = (int)invader.Position.X;
                    invader.rectangle.Y = (int)invader.Position.Y;
                }
                else
                {
                    invader.Position = parametricmovement(
                        earthposition,
                        (int)invader.OrbitRadius,
                        invader.anglularvelocity,
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
                BackroundColour();
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
                Constants.SCREENWIDTH / 2,
                Constants.SCREENHEIGHT / 2
            );

            Vector2 rocketScreenPosition = rocket.Position + rocketcameraoffset;

            Vector2 directionToCenter = targetScreenPosition - rocketScreenPosition;
            float distanceX = rocketScreenPosition.X - targetScreenPosition.X;
            float distanceY = rocketScreenPosition.Y - targetScreenPosition.X;
            float distance = distanceX * distanceY;
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
            cameraPosition = rocket.Position + rocketcameraoffset;
        }

        void DrawBackground(SpriteBatch _spriteBatch, Vector2 CameraOffset)
        {
            Vector2 tileSize = new Vector2(BackroundTexture.Width, BackroundTexture.Height);

            int tilesX = (int)Math.Ceiling(Constants.SCREENWIDTH / tileSize.X) + 2;
            int tilesY = (int)Math.Ceiling(Constants.SCREENHEIGHT / tileSize.Y) + 2;

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

                    _spriteBatch.Draw(BackroundTexture, tilePosition, Color.White);
                }
            }
        }

        private void DrawHUD(SpriteBatch _spriteBatch)
        {
            Vector2 heartscale = new Vector2(0.0625f, 0.0625f);

            for (int i = 0; i < rocket.Health; i++)
            {
                _spriteBatch.Draw(
                    HealthTexture,
                    new Vector2(HealthTexture.Width * heartscale.X * i, 00),
                    null,
                    Color.White,
                    0,
                    new Vector2(0, 0),
                    heartscale,
                    SpriteEffects.None,
                    0
                );
            }
            Vector2 FontOriginx = Vector2.Zero; //arial.MeasureString(xword) / 2;
            Vector2 FontOriginy = Vector2.Zero; //arial.MeasureString(yword) / 2;
            _spriteBatch.DrawString(
                arial,
                $"X : {Math.Round(rocket.Position.X, 3)}",
                new Vector2(10, 10),
                Color.LightGreen,
                0,
                FontOriginx,
                1.0f,
                SpriteEffects.None,
                0.5f
            );
            _spriteBatch.DrawString(
                arial,
                $"Y : {Math.Round(rocket.Position.Y, 3)}",
                new Vector2(10, 25),
                Color.LightGreen,
                0,
                FontOriginy,
                1.0f,
                SpriteEffects.None,
                0.5f
            );
            _spriteBatch.DrawString(
                arial,
                $"X Momentum : {Math.Round(rocket.Velocity.X, 3)}",
                new Vector2(10, 40),
                Color.LightGreen,
                0,
                FontOriginy,
                1.0f,
                SpriteEffects.None,
                0.5f
            );
            _spriteBatch.DrawString(
                arial,
                $"Y Momentum : {Math.Round(rocket.Velocity.Y, 3)}",
                new Vector2(10, 55),
                Color.LightGreen,
                0,
                FontOriginy,
                1.0f,
                SpriteEffects.None,
                0.5f
            );

            if (rocket.Health == 0)
            {
                _spriteBatch.Draw(
                    Gameover,
                    new Vector2(Constants.SCREENWIDTH / 2, Constants.SCREENHEIGHT / 2),
                    null,
                    Color.Red,
                    0,
                    new Vector2(Gameover.Width / 2, Gameover.Height / 2),
                    new Vector2(.5f, .5f),
                    SpriteEffects.None,
                    0
                );
            }
        }
        private void DrawDebugGraphics(SpriteBatch _spriteBatch, Vector2 cameraoffset)
        {
            foreach (PrimitiveBatch.Line line in lines)
            {
                line.Draw(_spriteBatch, _primitiveBatch,cameraoffset);
            }
            lines.Clear();
            foreach (PrimitiveBatch.Circle circle in circles)
            {
                circle.Draw(_spriteBatch, _primitiveBatch,cameraoffset);
            }
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
                    invader.Position + cameraoffset,
                    null,
                    invader.Color,
                    0,
                    new Vector2(Invader1Texture.Width/2, Invader1Texture.Height/2),
                    invader.Scale,
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
                    ;

                    _spriteBatch.Draw(
                        bullettexure,
                        bullet.position + cameraoffset,
                        null,
                        Color.White,
                        bullet.angle,
                        new Vector2(bullettexure.Width / 2, bullettexure.Height / 2),
                        bulletscale,
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
                earthposition + cameraoffset,
                null,
                Color.White,
                0,
                new Vector2(earthTexture.Width / 2, earthTexture.Height / 2),
                earthscale,
                SpriteEffects.None,
                0
            );
            _spriteBatch.Draw(
                moontexture,
                moonposition + cameraoffset,
                null,
                Color.White,
                0,
                new Vector2(moontexture.Width / 2, moontexture.Height / 2),
                moonscale,
                SpriteEffects.None,
                0
            );
        }

        private void DrawConsumables(SpriteBatch _spriteBatch, Vector2 cameraoffset)
        {
            _spriteBatch.Draw(
                thingtexture,
                thingposition + cameraoffset,
                null,
                Color.White,
                0,
                Vector2.Zero,
                coinscale,
                SpriteEffects.None,
                0
            );
        }

        protected override void Draw(GameTime gameTime)
        {
            Color mycolour = new Color(r, g, b);

            GraphicsDevice.Clear(Color.Black);
            Vector2 cameraoffset = (
                -rocket.Position
                + new Vector2(Constants.SCREENWIDTH / 2, Constants.SCREENHEIGHT / 2)
            );

            _spriteBatch.Begin();

            DrawBackground(_spriteBatch, cameraoffset);

            DrawEnviroment(_spriteBatch, cameraoffset);
            DrawEnemies(_spriteBatch, cameraoffset);
            DrawBullets(_spriteBatch, cameraoffset);
            DrawConsumables(_spriteBatch, cameraoffset);
            DrawDebugGraphics(_spriteBatch, cameraoffset);

            rocket.Rectangle = new Rectangle(
                (int)rocket.Position.X,
                (int)rocket.Position.Y,
                (int)(rocket.Texture.Width * rocket.Scale.X),
                (int)(rocket.Texture.Height * rocket.Scale.Y)
            );

            _spriteBatch.Draw(
                rocket.Texture,
                rocket.Position + rocketcameraoffset,
                null,
                Color.White,
                rocket.Angle,
                new Vector2(rocket.Texture.Width / 2, rocket.Texture.Height / 2),
                rocket.Scale,
                SpriteEffects.None,
                0
            );

            DrawHUD(_spriteBatch);
            _spriteBatch.End();
            //_desktop.Render();

            base.Draw(gameTime);
        }

        private void StartGame() { }

        protected override void UnloadContent() { }
    }
}
