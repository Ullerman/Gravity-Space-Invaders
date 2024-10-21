using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pleasework
{
    public class Timer
    {
        private double elapsedtime;
        private double duration;
        private bool isrunning;

        public Timer(double DurationInSeconds)
        {
            duration = DurationInSeconds;
            elapsedtime = 0;
            isrunning = false;
            
        }

        public void Start()
        {
            if (!isrunning)
            {
                isrunning = true;
                elapsedtime = 0;
            }
        }

        public void Stop()
        {
            isrunning = false;
        }

        public void Reset()
        {
            elapsedtime = 0;
            isrunning = false;
        }

        public void Update(GameTime gameTime)
        {
            if (isrunning)
            {
                elapsedtime += gameTime.ElapsedGameTime.TotalSeconds;

                if (elapsedtime >= duration)
                {
                    Stop();
                }
            }
        }

        public bool IsFinished()
        {
            return elapsedtime >= duration;
        }

        public double GetRemainingTime()
        {
            return duration - elapsedtime;
        }

        public bool IsRunning()
        {
            return isrunning;
        }
    }
}
