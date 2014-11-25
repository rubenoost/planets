using System;
using System.Drawing;
using System.Threading;
using Planets.Controller.Subcontrollers;
using Planets.Model;

namespace Planets.Controller
{
    public class Autodemo
    {
        /// <summary>
        /// The field used by this autodemo
        /// </summary>
        internal Playfield Field;

        /// <summary>
        /// The shootprojectilecontroller used by this autodemo
        /// </summary>
        internal ShootProjectileController Spc;

        /// <summary>
        /// Create new autodemo on given playfield that uses given ShootProjectileController
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        public Autodemo(Playfield p, ShootProjectileController s)
        {
            Field = p;
            Spc = s;
        }

        /// <summary>
        /// Whether the key is pressed
        /// </summary>
        public bool Kpressed
        {
            get;
            private set;
        }

        /// <summary>
        /// Whether this autodemo is running
        /// </summary>
        public bool running
        {
            get;
            set;
        }

        /// <summary>
        /// Run the autodemo
        /// </summary>
        public void Run()
        {
            // Create vars
            var p = new Point();
            var r = new Random();

            // While key is pressed
            while (Kpressed)
            {
                // Set running to true
                running = true;

                // Determine next click
                p = new Point(r.Next(0, Field.Size.Width), r.Next(0, Field.Size.Height));

                // Click 3 times and wait
                for (int i = 0; i < 3; i++)
                {
                    // Click
                    Spc.Clicked(p);
                    Field.LastAutoClickLocation = p;
                    Field.LastAutoClickMoment = DateTime.Now;
                    Thread.Sleep(400);
                }
                Thread.Sleep(1500);
            }

            // Set running to false
            running = false;
        }

        /// <summary>
        /// Stop autodemo
        /// </summary>
        public void Stop()
        {
            Kpressed = false;
        }

        /// <summary>
        /// Start autodemo
        /// </summary>
        public void Start()
        {
            Kpressed = true;
        }

    }
}
