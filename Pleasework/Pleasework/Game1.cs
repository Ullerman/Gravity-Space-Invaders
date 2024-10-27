using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        SpriteFont arial;

        byte r,
            g,
            b;
        bool iscolourforward;

        char whatcolour;
        Texture2D thingtexture;

        Vector2 thingposition;
        byte updatecoin;
        float fakecoinx;
        bool coinforward;
        Vector2 coinscale;

        Texture2D rockettexure;
        Vector2 rocketposition;
        Vector2 rocketmomentum;
        float rocketangle;
        float velocity;
        float friction;
        float angularVelocity;
        float angularAcceleration;
        float angularFriction;
        Vector2 rocketscale;
        Rectangle RocketRectangle;
        int rockethealth;

        Texture2D HealthTexture;

        Texture2D bullettexure;
        List<Bullet> Bulletlist;

        float bulletdefaultspeed;
        float bulletscale;
        Timer rocketbulletdelay;

        List<Invader> invaderlist;

        Texture2D Invader1Texture;
        Texture2D Invader2Texture;

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
        }

        protected override void Initialize()
        {
            rnd = new Random();


            r = 0;
            g = 0;
            b = 0;
            iscolourforward = true;
            coinforward = false;
            whatcolour = 'r';
            thingposition = new Vector2(600 - 52.5f, 0);
            updatecoin = 0;
            fakecoinx = 600.0f;

            rocketposition = new Vector2(300, 300);
            rocketmomentum = new Vector2(0, 0);
            rocketangle = 0f;
            velocity = 0.5f;
            friction = 0.99f; // .1f;
            angularVelocity = 0f;
            angularAcceleration = 0.09f;
            angularFriction = .99f;
            rockethealth = 3;

            bulletscale = 0.125f;

            Bulletlist = new List<Bullet>();

            bulletdefaultspeed = 10f;
            rocketbulletdelay = new Timer(0.25f);

            earthposition = new Vector2(Constants.SCREENWIDTH / 2, Constants.SCREENHEIGHT / 2);

            moonposition = earthposition + new Vector2(0, earthposition.Y / 2 + 100);
            moonscale = new Vector2(0.0625f, 0.0625f);
            moonanglularvelocity = 0.01f;
            moonangle = 0f;
            moonorbitradius = 500f;

            coinscale = new Vector2(.125f, .125f);
            rocketscale = new Vector2(.0625f, .0625f);
            earthscale = new Vector2(.25f, .25f);

            invaderlist = new List<Invader>();

            rocketcameraoffset = Vector2.Zero;

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            arial = Content.Load<SpriteFont>("File");

            effect = Content.Load<Effect>("CRT");

            thingtexture = Content.Load<Texture2D>("coin");

            rockettexure = Content.Load<Texture2D>("rocket");
            bullettexure = Content.Load<Texture2D>("bullet");
            HealthTexture = Content.Load<Texture2D>("heart");

            earthTexture = Content.Load<Texture2D>("earth");
            moontexture = Content.Load<Texture2D>("moon");

            Invader1Texture = Content.Load<Texture2D>("Invader1");
        }


        private void KeyHandling(GameTime gameTime)
        {

            var kstate = Keyboard.GetState();
            var gamepadState = GamePad.GetState(PlayerIndex.One);

            Vector2 leftThumbstick = gamepadState.ThumbSticks.Left;
            Vector2 rightThumbstick = gamepadState.ThumbSticks.Right;



            float leftTriggerValue = gamepadState.Triggers.Left;
            float rightTriggerValue = gamepadState.Triggers.Right;

            angularVelocity += angularAcceleration * leftThumbstick.X;


            float triangleAnglecontrol = (float)(rocketangle - Math.PI / 2);
            rocketmomentum.X += (float)(Math.Cos(triangleAnglecontrol) * velocity)*rightTriggerValue;
            rocketmomentum.Y += (float)(Math.Sin(triangleAnglecontrol) * velocity)*rightTriggerValue;

            rocketmomentum.X -= (float)(Math.Cos(triangleAnglecontrol) * velocity)*leftTriggerValue;
            rocketmomentum.Y -= (float)(Math.Sin(triangleAnglecontrol) * velocity)*leftTriggerValue;



            if (
                kstate.IsKeyDown(Keys.Left)
                
                || kstate.IsKeyDown(Keys.A)
            )
            {
                angularVelocity -= angularAcceleration;
            }
        
            if (
                kstate.IsKeyDown(Keys.Right)
                
                || kstate.IsKeyDown(Keys.D)
            )
            {
                angularVelocity += angularAcceleration;
            }

            if (
                kstate.IsKeyDown(Keys.Up)
                || kstate.IsKeyDown(Keys.W)
            )
            {
                float triangleAngle = (float)(rocketangle - Math.PI / 2);
                rocketmomentum.X += (float)(Math.Cos(triangleAngle) * velocity);
                rocketmomentum.Y += (float)(Math.Sin(triangleAngle) * velocity);
            }

            if (
                kstate.IsKeyDown(Keys.Down)
                || gamepadState.Triggers.Left > 0.5f
                || kstate.IsKeyDown(Keys.S)
            )
            {
                float triangleAngle = (float)(rocketangle - Math.PI / 2);
                rocketmomentum.X -= (float)(Math.Cos(triangleAngle) * velocity);
                rocketmomentum.Y -= (float)(Math.Sin(triangleAngle) * velocity);
            }

            if (kstate.IsKeyDown(Keys.Space) || gamepadState.Buttons.A == ButtonState.Pressed)
            {
                FireBullet(
                    rocketposition,
                    rocketangle,
                    rocketmomentum,
                    ref RocketRectangle,
                    ref invaderlist,
                    ref rocketbulletdelay,
                    gameTime
                );
            }

            if (kstate.IsKeyDown(Keys.R) || gamepadState.Buttons.Start == ButtonState.Pressed)
            {
                rocketposition = new Vector2(Constants.SCREENWIDTH / 8, Constants.SCREENHEIGHT / 8);
                rocketmomentum = Vector2.Zero;
                angularVelocity = 0;
                rockethealth = 3;
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
                Bullet bullet = new Bullet();

                float angle = fireangle + rnd.Next(-60, 60) / 100;
                float triangleAngle = (float)(angle - Math.PI / 2);

                bullet.position = position;
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
                Console.WriteLine("FIREBULLET");
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
                    bullet.angle = (float)(
                        MathF.Atan2(bullet.momentum.X, bullet.momentum.Y) + Math.PI / 2
                    );
                    Rectangle bulletrect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        (int)(bullettexure.Width * bulletscale),
                        (int)(bullettexure.Height * bulletscale)
                    );

                    if (
                        IsRectCollidingWithCircle(earthposition, earthradius, bulletrect)
                        & !RocketRectangle.Intersects(bulletrect)
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
                                rockethealth -= 1;
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

        private void Coin()
        {
            if (updatecoin % 1 == 0)
                thingposition.Y = (float)Math.Sin(fakecoinx) * 200 + 200;
            updatecoin++;

            if (thingposition.X <= 0)
            {
                coinforward = true;
            }
            else if (thingposition.X >= Constants.SCREENWIDTH)
            {
                coinforward = false;
            }

            if (coinforward)
            {
                thingposition.X += 2;
                fakecoinx += 0.03125f;
            }
            else
            {
                thingposition.X -= 2;
                fakecoinx -= 0.03125f;
            }
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
                invader.anglularvelocity = 0.01f;
                invader.Color = new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255));
                invader.OrbitRadius = rnd.Next(400, 800);
                invader.SightRadius = rnd.Next(100, 300);
                invader.delay = new Timer(0.75f);
            }
        }

        private void Physics(double deltatime)
        {
            float gravityOffset = 100f;

            rocketmomentum = GravityCalculation(
                earthposition,
                rocketposition,
                rocketmomentum,
                gravityOffset,
                6
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
            rocketmomentum = GravityCalculation(
                moonposition,
                rocketposition,
                rocketmomentum,
                gravityOffset,
                3.9f
            );

            rocketangle += (float)(angularVelocity * deltatime);
            angularVelocity *= angularFriction;

            rocketposition += rocketmomentum;
            rocketmomentum *= friction;

            Vector2 earthcentre = earthposition;
            float earthradius = (earthTexture.Width * earthscale.X) / 2 - 5;

            if (IsRectCollidingWithCircle(earthcentre, earthradius, RocketRectangle))
            {
                Vector2 collisionNormal = rocketposition - earthcentre;
                collisionNormal.Normalize();
                // rocketmomentum *= friction;
                float penetration = earthradius - (rocketposition - earthcentre).Length();
                if (penetration > 0)
                {
                    rocketposition += collisionNormal * penetration;
                }
                if (rocketmomentum.X > 1 || rocketmomentum.Y > 1)
                {
                    rocketmomentum = Vector2.Reflect(rocketmomentum, collisionNormal) * 0.8f;
                    // angularVelocity = -angularVelocity * 0.5f;
                }
                if (rocketmomentum.Length() < 0.1f)
                {
                    //rocketmomentum = Vector2.Zero;
                }
                if (
                    DistancesquaredBetweenpointandrect(RocketRectangle, earthcentre)
                    < earthradius * earthradius + 36
                )
                {
                    rocketmomentum *= friction;
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
                if (
                    IsRectCollidingWithCircle(
                        invader.Position,
                        invader.SightRadius,
                        RocketRectangle
                    )
                )
                {
                    // Console.WriteLine("invadersee");
                    float angle = CalculateAngleBetweenPoints(invader.Position, rocketposition);
                    FireBullet(
                        invader.Position,
                        angle,
                        Vector2.Zero,
                        ref invaderlist,
                        ref RocketRectangle,
                        ref invader.delay,
                        gameTime
                    );
                }
            }
        }

        private void Camera()
        {
            Vector2 targetScreenPosition = new Vector2(
                Constants.SCREENWIDTH / 2,
                Constants.SCREENHEIGHT / 2
            );

            Vector2 rocketScreenPosition = rocketposition + rocketcameraoffset;

            Vector2 directionToCenter = targetScreenPosition - rocketScreenPosition;
            float distanceX = rocketScreenPosition.X - targetScreenPosition.X;
            float distanceY = rocketScreenPosition.Y - targetScreenPosition.X;
            float distance = distanceX * distanceY;
            float returnSpeed = 0.23f;
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
        }

        protected override void Update(GameTime gameTime)
        {
            double deltatime = gameTime.ElapsedGameTime.TotalSeconds;

            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
            )
                Exit();

            Coin();
            if (invaderlist.Count == 0)
            {
                SpawnInvaders();
            }
            rocketbulletdelay.Update(gameTime);
            KeyHandling(gameTime);
            Physics(deltatime);
            UpdateBullet();
            InvaderCompute(gameTime);
            BackroundColour();

            moonangle += moonanglularvelocity;
            moonposition.X = earthposition.X + (float)(moonorbitradius * Math.Cos(moonangle));
            moonposition.Y = earthposition.Y + (float)(moonorbitradius * Math.Sin(moonangle));
            foreach (Invader invader in invaderlist)
            {
                invader.angle += invader.anglularvelocity;
                invader.Position.X =
                    earthposition.X + (float)(invader.OrbitRadius * Math.Cos(invader.angle));
                invader.Position.Y =
                    earthposition.Y + (float)(invader.OrbitRadius * Math.Sin(invader.angle));
                invader.rectangle.X = (int)invader.Position.X;
                invader.rectangle.Y = (int)invader.Position.Y;
            }

            Camera();
            base.Update(gameTime);
        }
        private void DrawWithCrt(Texture2D texture)
            {
                effect.Parameters["textureSize"].SetValue(new Vector2(texture.Width, texture.Height));


            }

        protected override void Draw(GameTime gameTime)
        {
            Color mycolour = new Color(r, g, b);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            Vector2 cameraoffset = (
                -rocketposition + new Vector2(Constants.SCREENWIDTH / 2, Constants.SCREENHEIGHT / 2)
            );
            
            // effect.Parameters["videoSize"].SetValue(new Vector2(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height));
            // effect.Parameters["outputSize"].SetValue(new Vector2(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height));

            // effect.Parameters["hardScan"].SetValue(-8.0f);
            // effect.Parameters["hardPix"].SetValue(-3.0f);
            // effect.Parameters["warp"].SetValue(new Vector2(0.03f, 0.04f));
            // effect.Parameters["maskDark"].SetValue(0.5f);
            // effect.Parameters["maskLight"].SetValue(1.5f);
            // effect.Parameters["bloomAmount"].SetValue(0.1f);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null);
            RocketRectangle = new Rectangle(
                (int)rocketposition.X,
                (int)rocketposition.Y,
                (int)(rockettexure.Width * rocketscale.X),
                (int)(rockettexure.Height * rocketscale.Y - 50)
            );
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
            _spriteBatch.Draw(
                rockettexure,
                rocketposition + rocketcameraoffset,
                null,
                Color.White,
                rocketangle,
                new Vector2(rockettexure.Width / 2, rockettexure.Height / 2),
                rocketscale,
                SpriteEffects.None,
                0
            );

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
                        1
                    );
                }
            }

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
                    new Vector2(0, 0),
                    invader.Scale,
                    SpriteEffects.None,
                    0
                );
            }

            Vector2 heartscale = new Vector2(0.0625f, 0.0625f);

            for (int i = 0; i < rockethealth; i++)
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
                $"X : {Math.Round(rocketposition.X, 3)}",
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
                $"Y : {Math.Round(rocketposition.Y, 3)}",
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
                $"X Momentum : {Math.Round(rocketmomentum.X, 3)}",
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
                $"Y Momentum : {Math.Round(rocketmomentum.Y, 3)}",
                new Vector2(10, 55),
                Color.LightGreen,
                0,
                FontOriginy,
                1.0f,
                SpriteEffects.None,
                0.5f
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
